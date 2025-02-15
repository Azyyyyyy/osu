// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Taiko.Objects;

namespace osu.Game.Rulesets.Taiko.Skinning.Default
{
    public class RimHitCirclePiece : CirclePiece
    {
        public RimHitCirclePiece()
        {
            Add(new RimHitSymbolPiece());
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            AccentColour = Hit.COLOUR_RIM;
        }

        /// <summary>
        /// The symbol used for rim hit pieces.
        /// </summary>
        public class RimHitSymbolPiece : CircularContainer
        {
            public RimHitSymbolPiece()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                RelativeSizeAxes = Axes.Both;
                Size = new Vector2(SYMBOL_SIZE);

                BorderThickness = SYMBOL_BORDER;
                BorderColour = Colour4.White;
                Masking = true;
                Children = new[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                };
            }
        }
    }
}
