// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;

namespace osu.Game.Rulesets.Osu.Skinning.Default
{
    public abstract class DrawableSliderPath : SmoothPath
    {
        protected const float BORDER_PORTION = 0.128f;
        protected const float GRADIENT_PORTION = 1 - BORDER_PORTION;

        private const float border_max_size = 8f;
        private const float border_min_size = 0f;

        private Colour4 borderColour = Colour4.White;

        public Colour4 BorderColour
        {
            get => borderColour;
            set
            {
                if (borderColour == value)
                    return;

                borderColour = value;

                InvalidateTexture();
            }
        }

        private Colour4 accentColour = Colour4.White;

        public Colour4 AccentColour
        {
            get => accentColour;
            set
            {
                if (accentColour == value)
                    return;

                accentColour = value;

                InvalidateTexture();
            }
        }

        private float borderSize = 1;

        public float BorderSize
        {
            get => borderSize;
            set
            {
                if (borderSize == value)
                    return;

                if (value < border_min_size || value > border_max_size)
                    return;

                borderSize = value;

                InvalidateTexture();
            }
        }

        protected float CalculatedBorderPortion => BorderSize * BORDER_PORTION;
    }
}
