﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Batches;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps;
using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Utils;
using osu.Framework.Extensions.MatrixExtensions;
using Vector2Extensions = osu.Framework.Graphics.Vector2Extensions;

namespace osu.Game.Screens.Menu
{
    /// <summary>
    /// A visualiser that reacts to music coming from beatmaps.
    /// </summary>
    public class LogoVisualisation : Drawable
    {
        private readonly IBindable<WorkingBeatmap> beatmap = new Bindable<WorkingBeatmap>();

        /// <summary>
        /// The number of bars to jump each update iteration.
        /// </summary>
        private const int index_change = 5;

        /// <summary>
        /// The maximum length of each bar in the visualiser. Will be reduced when kiai is not activated.
        /// </summary>
        private const float bar_length = 600;

        /// <summary>
        /// The number of bars in one rotation of the visualiser.
        /// </summary>
        private const int bars_per_visualiser = 200;

        /// <summary>
        /// How many times we should stretch around the circumference (overlapping overselves).
        /// </summary>
        private const float visualiser_rounds = 5;

        /// <summary>
        /// How much should each bar go down each millisecond (based on a full bar).
        /// </summary>
        private const float decay_per_milisecond = 0.0024f;

        /// <summary>
        /// Number of milliseconds between each amplitude update.
        /// </summary>
        private const float time_between_updates = 50;

        /// <summary>
        /// The minimum amplitude to show a bar.
        /// </summary>
        private const float amplitude_dead_zone = 1f / bar_length;

        private int indexOffset;

        /// <summary>
        /// The relative movement of bars based on input amplification. Defaults to 1.
        /// </summary>
        public float Magnitude { get; set; } = 1;

        private readonly float[] frequencyAmplitudes = new float[256];

        private IShader shader;
        private readonly Texture texture;

        public LogoVisualisation()
        {
            texture = Texture.WhitePixel;
            Blending = BlendingParameters.Additive;
        }

        private readonly List<IHasAmplitudes> amplitudeSources = new List<IHasAmplitudes>();

        public void AddAmplitudeSource(IHasAmplitudes amplitudeSource)
        {
            amplitudeSources.Add(amplitudeSource);
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders, IBindable<WorkingBeatmap> beatmap)
        {
            this.beatmap.BindTo(beatmap);
            shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
        }

        private readonly float[] temporalAmplitudes = new float[ChannelAmplitudes.AMPLITUDES_SIZE];

        private void updateAmplitudes()
        {
            var effect = beatmap.Value.BeatmapLoaded && beatmap.Value.TrackLoaded
                ? beatmap.Value.Beatmap?.ControlPointInfo.EffectPointAt(beatmap.Value.Track.CurrentTime)
                : null;

            for (int i = 0; i < temporalAmplitudes.Length; i++)
                temporalAmplitudes[i] = 0;

            if (beatmap.Value.TrackLoaded)
                addAmplitudesFromSource(beatmap.Value.Track);

            foreach (var source in amplitudeSources)
                addAmplitudesFromSource(source);

            for (int i = 0; i < bars_per_visualiser; i++)
            {
                float targetAmplitude = (temporalAmplitudes[(i + indexOffset) % bars_per_visualiser]) * (effect?.KiaiMode == true ? 1 : 0.5f);
                if (targetAmplitude > frequencyAmplitudes[i])
                    frequencyAmplitudes[i] = targetAmplitude;
            }

            indexOffset = (indexOffset + index_change) % bars_per_visualiser;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            var delayed = Scheduler.AddDelayed(updateAmplitudes, time_between_updates, true);
            delayed.PerformRepeatCatchUpExecutions = false;
        }

        protected override void Update()
        {
            base.Update();

            float decayFactor = (float)Time.Elapsed * decay_per_milisecond;

            for (int i = 0; i < bars_per_visualiser; i++)
            {
                //3% of extra bar length to make it a little faster when bar is almost at it's minimum
                frequencyAmplitudes[i] -= decayFactor * (frequencyAmplitudes[i] + 0.03f);
                if (frequencyAmplitudes[i] < 0)
                    frequencyAmplitudes[i] = 0;
            }

            Invalidate(Invalidation.DrawNode);
        }

        protected override DrawNode CreateDrawNode() => new VisualisationDrawNode(this);

        private void addAmplitudesFromSource([NotNull] IHasAmplitudes source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var amplitudes = source.CurrentAmplitudes.FrequencyAmplitudes.Span;

            for (int i = 0; i < amplitudes.Length; i++)
            {
                if (i < temporalAmplitudes.Length)
                    temporalAmplitudes[i] += amplitudes[i];
            }
        }

        private class VisualisationDrawNode : DrawNode
        {
            protected new LogoVisualisation Source => (LogoVisualisation)base.Source;

            private IShader shader;
            private Texture texture;

            // Assuming the logo is a circle, we don't need a second dimension.
            private float size;

            private static readonly Colour4 transparent_white = Colour4.White.Opacity(0.2f);

            private float[] audioData;

            private readonly QuadBatch<TexturedVertex2D> vertexBatch = new QuadBatch<TexturedVertex2D>(100, 10);

            public VisualisationDrawNode(LogoVisualisation source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                shader = Source.shader;
                texture = Source.texture;
                size = Source.DrawSize.X;
                audioData = Source.frequencyAmplitudes;
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                shader.Bind();

                Vector2 inflation = DrawInfo.MatrixInverse.ExtractScale().Xy();

                ColourInfo colourInfo = DrawColourInfo.Colour;
                colourInfo.ApplyChild(transparent_white);

                if (audioData != null)
                {
                    for (int j = 0; j < visualiser_rounds; j++)
                    {
                        for (int i = 0; i < bars_per_visualiser; i++)
                        {
                            if (audioData[i] < amplitude_dead_zone)
                                continue;

                            float rotation = MathUtils.DegreesToRadians(i / (float)bars_per_visualiser * 360 + j * 360 / visualiser_rounds);
                            float rotationCos = MathF.Cos(rotation);
                            float rotationSin = MathF.Sin(rotation);
                            // taking the cos and sin to the 0..1 range
                            var barPosition = new Vector2(rotationCos / 2 + 0.5f, rotationSin / 2 + 0.5f) * size;

                            var barSize = new Vector2(size * MathF.Sqrt(2 * (1 - MathF.Cos(MathUtils.DegreesToRadians(360f / bars_per_visualiser)))) / 2f, bar_length * audioData[i]);
                            // The distance between the position and the sides of the bar.
                            var bottomOffset = new Vector2(-rotationSin * barSize.X / 2, rotationCos * barSize.X / 2);
                            // The distance between the bottom side of the bar and the top side.
                            var amplitudeOffset = new Vector2(rotationCos * barSize.Y, rotationSin * barSize.Y);

                            var rectangle = new Quad(
                                Vector2Extensions.Transform(barPosition - bottomOffset, DrawInfo.Matrix),
                                Vector2Extensions.Transform(barPosition - bottomOffset + amplitudeOffset, DrawInfo.Matrix),
                                Vector2Extensions.Transform(barPosition + bottomOffset, DrawInfo.Matrix),
                                Vector2Extensions.Transform(barPosition + bottomOffset + amplitudeOffset, DrawInfo.Matrix)
                            );

                            DrawQuad(
                                texture,
                                rectangle,
                                colourInfo,
                                null,
                                vertexBatch.AddAction,
                                // barSize by itself will make it smooth more in the X axis than in the Y axis, this reverts that.
                                Vector2.Divide(inflation, barSize));
                        }
                    }
                }

                shader.Unbind();
            }

            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);

                vertexBatch.Dispose();
            }
        }
    }
}
