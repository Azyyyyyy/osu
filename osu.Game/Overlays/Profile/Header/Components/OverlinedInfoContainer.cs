﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Overlays.Profile.Header.Components
{
    public class OverlinedInfoContainer : CompositeDrawable
    {
        private readonly Circle line;
        private readonly OsuSpriteText title;
        private readonly OsuSpriteText content;

        public LocalisableString Title
        {
            set => title.Text = value;
        }

        public LocalisableString Content
        {
            set => content.Text = value;
        }

        public Colour4 LineColour
        {
            set => line.Colour = value;
        }

        public OverlinedInfoContainer(bool big = false, int minimumWidth = 60)
        {
            AutoSizeAxes = Axes.Both;
            InternalChild = new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    line = new Circle
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 2,
                        Margin = new MarginPadding { Bottom = 2 }
                    },
                    title = new OsuSpriteText
                    {
                        Font = OsuFont.GetFont(size: big ? 14 : 12, weight: FontWeight.Bold)
                    },
                    content = new OsuSpriteText
                    {
                        Font = OsuFont.GetFont(size: big ? 40 : 18, weight: FontWeight.Light)
                    },
                    new Container // Add a minimum size to the FillFlowContainer
                    {
                        Width = minimumWidth,
                    }
                }
            };
        }
    }
}
