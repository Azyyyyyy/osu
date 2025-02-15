﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Tests.Visual.UserInterface
{
    [TestFixture]
    public class TestSceneWaveContainer : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            WaveContainer container;
            Add(container = new WaveContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(400),
                FirstWaveColour = colours.Red,
                SecondWaveColour = colours.Green,
                ThirdWaveColour = colours.Blue,
                FourthWaveColour = colours.Pink,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Black.Opacity(0.5f),
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = OsuFont.GetFont(size: 20),
                        Text = @"Wave Container",
                    },
                },
            });

            AddStep(@"show", container.Show);
            AddStep(@"hide", container.Hide);
        }
    }
}
