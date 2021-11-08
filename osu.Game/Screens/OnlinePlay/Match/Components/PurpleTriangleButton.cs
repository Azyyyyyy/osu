// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Colour4Extensions;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Screens.OnlinePlay.Match.Components
{
    public class PurpleTriangleButton : TriangleButton
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            BackgroundColour = Colour4Extensions.FromHex(@"593790");
            Triangles.ColourLight = Colour4Extensions.FromHex(@"7247b6");
            Triangles.ColourDark = Colour4Extensions.FromHex(@"593790");
        }
    }
}
