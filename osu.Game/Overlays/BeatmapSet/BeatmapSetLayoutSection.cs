// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;

namespace osu.Game.Overlays.BeatmapSet
{
    public class BeatmapSetLayoutSection : Container
    {
        public BeatmapSetLayoutSection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Masking = true;
            EdgeEffect = new EdgeEffectParameters
            {
                Colour = Colour4.Black.Opacity(0.25f),
                Type = EdgeEffectType.Shadow,
                Radius = 3,
                Offset = new Vector2(0f, 1f),
            };
        }
    }
}
