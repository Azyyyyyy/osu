// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;

namespace osu.Game.Beatmaps.Formats
{
    public interface IHasCustomColours
    {
        Dictionary<string, Colour4> CustomColours { get; }
    }
}
