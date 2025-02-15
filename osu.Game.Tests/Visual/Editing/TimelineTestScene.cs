// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Tests.Visual.Editing
{
    public abstract class TimelineTestScene : EditorClockTestScene
    {
        protected TimelineArea TimelineArea { get; private set; }

        protected HitObjectComposer Composer { get; private set; }

        protected EditorBeatmap EditorBeatmap { get; private set; }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Beatmap.Value = new WaveformTestBeatmap(audio);

            var playable = Beatmap.Value.GetPlayableBeatmap(Beatmap.Value.BeatmapInfo.Ruleset);
            EditorBeatmap = new EditorBeatmap(playable);

            Dependencies.Cache(EditorBeatmap);
            Dependencies.CacheAs<IBeatSnapProvider>(EditorBeatmap);

            Composer = playable.BeatmapInfo.Ruleset.CreateInstance().CreateHitObjectComposer().With(d => d.Alpha = 0);

            AddRange(new Drawable[]
            {
                EditorBeatmap,
                Composer,
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 5),
                    Children = new Drawable[]
                    {
                        new StartStopButton(),
                        new AudioVisualiser(),
                    }
                },
                TimelineArea = new TimelineArea(CreateTestComponent())
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Clock.Seek(2500);
        }

        public abstract Drawable CreateTestComponent();

        private class AudioVisualiser : CompositeDrawable
        {
            private readonly Drawable marker;

            [Resolved]
            private IBindable<WorkingBeatmap> beatmap { get; set; }

            [Resolved]
            private EditorClock editorClock { get; set; }

            public AudioVisualiser()
            {
                Size = new Vector2(250, 25);

                InternalChildren = new[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.25f,
                    },
                    marker = new Box
                    {
                        RelativePositionAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Width = 2,
                    }
                };
            }

            protected override void Update()
            {
                base.Update();

                if (beatmap.Value.Track.IsLoaded)
                    marker.X = (float)(editorClock.CurrentTime / beatmap.Value.Track.Length);
            }
        }

        private class StartStopButton : OsuButton
        {
            [Resolved]
            private EditorClock editorClock { get; set; }

            public StartStopButton()
            {
                BackgroundColour = Colour4.SlateGray;
                Size = new Vector2(100, 50);
                Text = "Start";

                Action = onClick;
            }

            private void onClick()
            {
                if (editorClock.IsRunning)
                    editorClock.Stop();
                else
                    editorClock.Start();
            }

            protected override void Update()
            {
                base.Update();

                Text = editorClock.IsRunning ? "Stop" : "Start";
            }
        }
    }
}
