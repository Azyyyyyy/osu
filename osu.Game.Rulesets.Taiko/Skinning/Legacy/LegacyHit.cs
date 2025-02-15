// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Taiko.Skinning.Legacy
{
    public class LegacyHit : LegacyCirclePiece
    {
        private readonly TaikoSkinComponents component;

        public LegacyHit(TaikoSkinComponents component)
        {
            this.component = component;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AccentColour = LegacyColourCompatibility.DisallowZeroAlpha(
                component == TaikoSkinComponents.CentreHit
                    ? new Colour4(235, 69, 44, 255)
                    : new Colour4(67, 142, 172, 255));
        }
    }
}
