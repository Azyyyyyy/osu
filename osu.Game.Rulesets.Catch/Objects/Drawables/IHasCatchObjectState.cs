// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Catch.Objects.Drawables
{
    /// <summary>
    /// Provides a visual state of a <see cref="PalpableCatchHitObject"/>.
    /// </summary>
    public interface IHasCatchObjectState
    {
        PalpableCatchHitObject HitObject { get; }

        Bindable<Colour4> AccentColour { get; }

        Bindable<bool> HyperDash { get; }

        Bindable<int> IndexInBeatmap { get; }

        Vector2 DisplaySize { get; }

        float DisplayRotation { get; }
    }
}
