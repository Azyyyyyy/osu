// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Tests.Visual.Online
{
    public class TestSceneExternalLinkButton : OsuTestScene
    {
        public TestSceneExternalLinkButton()
        {
            Child = new ExternalLinkButton("https://osu.ppy.sh/home")
            {
                Size = new Vector2(50)
            };
        }
    }
}
