// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Components;

namespace osu.Game.Tests.Visual.Editing
{
    [TestFixture]
    public class TestScenePlaybackControl : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var clock = new EditorClock { IsCoupled = false };
            Dependencies.CacheAs(clock);

            var playback = new PlaybackControl
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(200, 100)
            };

            Beatmap.Value = CreateWorkingBeatmap(new Beatmap());

            Child = playback;
        }
    }
}
