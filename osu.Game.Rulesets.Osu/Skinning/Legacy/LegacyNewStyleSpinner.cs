// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Utils;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Osu.Skinning.Legacy
{
    /// <summary>
    /// Legacy skinned spinner with two main spinning layers, one fixed overlay and one final spinning overlay.
    /// No background layer.
    /// </summary>
    public class LegacyNewStyleSpinner : LegacySpinner
    {
        private Sprite glow;
        private Sprite discBottom;
        private Sprite discTop;
        private Sprite spinningMiddle;
        private Sprite fixedMiddle;

        private readonly Colour4 glowColour = new Colour4(3, 151, 255, 255);

        private Container scaleContainer;

        [BackgroundDependencyLoader]
        private void load(ISkinSource source)
        {
            AddInternal(scaleContainer = new Container
            {
                Scale = new Vector2(SPRITE_SCALE),
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Y = SPINNER_Y_CENTRE,
                Children = new Drawable[]
                {
                    glow = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = source.GetTexture("spinner-glow"),
                        Blending = BlendingParameters.Additive,
                        Colour = glowColour,
                    },
                    discBottom = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = source.GetTexture("spinner-bottom"),
                    },
                    discTop = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = source.GetTexture("spinner-top"),
                    },
                    fixedMiddle = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = source.GetTexture("spinner-middle"),
                    },
                    spinningMiddle = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = source.GetTexture("spinner-middle2"),
                    },
                }
            });

            if (!(source.FindProvider(s => s.GetTexture("spinner-top") != null) is DefaultLegacySkin))
            {
                AddInternal(ApproachCircle = new Sprite
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    Texture = source.GetTexture("spinner-approachcircle"),
                    Scale = new Vector2(SPRITE_SCALE * 1.86f),
                    Y = SPINNER_Y_CENTRE,
                });
            }
        }

        protected override void UpdateStateTransforms(DrawableHitObject drawableHitObject, ArmedState state)
        {
            base.UpdateStateTransforms(drawableHitObject, state);

            switch (drawableHitObject)
            {
                case DrawableSpinner d:
                    Spinner spinner = d.HitObject;

                    using (BeginAbsoluteSequence(spinner.StartTime - spinner.TimePreempt))
                        this.FadeOut();

                    using (BeginAbsoluteSequence(spinner.StartTime - spinner.TimeFadeIn / 2))
                        this.FadeInFromZero(spinner.TimeFadeIn / 2);

                    using (BeginAbsoluteSequence(spinner.StartTime - spinner.TimePreempt))
                    {
                        fixedMiddle.FadeColour(Colour4.White);

                        using (BeginDelayedSequence(spinner.TimePreempt))
                            fixedMiddle.FadeColour(Colour4.Red, spinner.Duration);
                    }

                    if (state == ArmedState.Hit)
                    {
                        using (BeginAbsoluteSequence(d.HitStateUpdateTime))
                            glow.FadeOut(300);
                    }

                    break;

                case DrawableSpinnerBonusTick _:
                    if (state == ArmedState.Hit)
                        glow.FlashColour(Colour4.White, 200);

                    break;
            }
        }

        protected override void Update()
        {
            base.Update();
            spinningMiddle.Rotation = discTop.Rotation = DrawableSpinner.RotationTracker.Rotation;
            discBottom.Rotation = discTop.Rotation / 3;

            glow.Alpha = DrawableSpinner.Progress;

            scaleContainer.Scale = new Vector2(SPRITE_SCALE * (0.8f + (float)Interpolation.ApplyEasing(Easing.Out, DrawableSpinner.Progress) * 0.2f));
        }
    }
}
