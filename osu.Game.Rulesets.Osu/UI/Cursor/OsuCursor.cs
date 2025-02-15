// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Osu.Skinning;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Osu.UI.Cursor
{
    public class OsuCursor : SkinReloadableDrawable
    {
        private const float size = 28;

        private bool cursorExpand;

        private SkinnableDrawable cursorSprite;

        private Drawable expandTarget => (cursorSprite.Drawable as OsuCursorSprite)?.ExpandTarget ?? cursorSprite;

        public OsuCursor()
        {
            Origin = Anchor.Centre;

            Size = new Vector2(size);
        }

        protected override void SkinChanged(ISkinSource skin)
        {
            cursorExpand = skin.GetConfig<OsuSkinConfiguration, bool>(OsuSkinConfiguration.CursorExpand)?.Value ?? true;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Child = cursorSprite = new SkinnableDrawable(new OsuSkinComponent(OsuSkinComponents.Cursor), _ => new DefaultCursor(), confineMode: ConfineMode.NoScaling)
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                }
            };
        }

        private const float pressed_scale = 1.2f;
        private const float released_scale = 1f;

        public void Expand()
        {
            if (!cursorExpand) return;

            expandTarget.ScaleTo(released_scale).ScaleTo(pressed_scale, 400, Easing.OutElasticHalf);
        }

        public void Contract() => expandTarget.ScaleTo(released_scale, 400, Easing.OutQuad);

        private class DefaultCursor : OsuCursorSprite
        {
            public DefaultCursor()
            {
                RelativeSizeAxes = Axes.Both;

                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                InternalChildren = new[]
                {
                    ExpandTarget = new CircularContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        BorderThickness = size / 6,
                        BorderColour = Colour4.White,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Colour = Colour4.Pink.Opacity(0.5f),
                            Radius = 5,
                        },
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Alpha = 0,
                                AlwaysPresent = true,
                            },
                            new CircularContainer
                            {
                                Origin = Anchor.Centre,
                                Anchor = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                Masking = true,
                                BorderThickness = size / 3,
                                BorderColour = Colour4.White.Opacity(0.5f),
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Alpha = 0,
                                        AlwaysPresent = true,
                                    },
                                },
                            },
                        },
                    },
                    new Circle
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(0.14f),
                        Colour = new Colour4(34, 93, 204, 255),
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Glow,
                            Radius = 8,
                            Colour = Colour4.White,
                        },
                    },
                };
            }
        }
    }
}
