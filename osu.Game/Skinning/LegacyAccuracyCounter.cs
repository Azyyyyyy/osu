// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Screens.Play;
using osu.Game.Screens.Play.HUD;

namespace osu.Game.Skinning
{
    public class LegacyAccuracyCounter : GameplayAccuracyCounter, ISkinnableDrawable
    {
        public bool UsesFixedAnchor { get; set; }

        public LegacyAccuracyCounter()
        {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;

            Scale = new Vector2(0.6f);
            Margin = new MarginPadding(10);
        }

        [Resolved(canBeNull: true)]
        private HUDOverlay hud { get; set; }

        protected sealed override OsuSpriteText CreateSpriteText() => new LegacySpriteText(LegacyFont.Score)
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
        };
    }
}
