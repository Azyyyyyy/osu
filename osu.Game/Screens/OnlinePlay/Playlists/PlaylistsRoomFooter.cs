// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Numerics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Screens.OnlinePlay.Playlists
{
    public class PlaylistsRoomFooter : CompositeDrawable
    {
        public Action OnStart;

        public PlaylistsRoomFooter()
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new[]
            {
                new PlaylistsReadyButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(600, 1),
                    Action = () => OnStart?.Invoke()
                }
            };
        }
    }
}
