// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Mania.Skinning.Legacy
{
    public class LegacyHitTarget : CompositeDrawable
    {
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        private Container directionContainer;

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, IScrollingInfo scrollingInfo)
        {
            string targetImage = skin.GetManiaSkinConfig<string>(LegacyManiaSkinConfigurationLookups.HitTargetImage)?.Value
                                 ?? "mania-stage-hint";

            bool showJudgementLine = skin.GetManiaSkinConfig<bool>(LegacyManiaSkinConfigurationLookups.ShowJudgementLine)?.Value
                                     ?? true;

            Colour4 lineColour = skin.GetManiaSkinConfig<Colour4>(LegacyManiaSkinConfigurationLookups.JudgementLineColour)?.Value
                                ?? Colour4.White;

            InternalChild = directionContainer = new Container
            {
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Texture = skin.GetTexture(targetImage),
                        Scale = new Vector2(1, 0.9f * 1.6025f),
                        RelativeSizeAxes = Axes.X,
                        Width = 1
                    },
                    new Box
                    {
                        Anchor = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Height = 1,
                        Colour = LegacyColourCompatibility.DisallowZeroAlpha(lineColour),
                        Alpha = showJudgementLine ? 0.9f : 0
                    }
                }
            };

            direction.BindTo(scrollingInfo.Direction);
            direction.BindValueChanged(onDirectionChanged, true);
        }

        private void onDirectionChanged(ValueChangedEvent<ScrollingDirection> direction)
        {
            if (direction.NewValue == ScrollingDirection.Up)
            {
                directionContainer.Anchor = Anchor.TopLeft;
                directionContainer.Scale = new Vector2(1, -1);
            }
            else
            {
                directionContainer.Anchor = Anchor.BottomLeft;
                directionContainer.Scale = Vector2.One;
            }
        }
    }
}
