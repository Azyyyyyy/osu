﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Catch.Objects
{
    /// <summary>
    /// Represents a single object that can be caught by the catcher.
    /// This includes normal fruits, droplets, and bananas but excludes objects that act only as a container of nested hit objects.
    /// </summary>
    public abstract class PalpableCatchHitObject : CatchHitObject, IHasComboInformation
    {
        /// <summary>
        /// Difference between the distance to the next object
        /// and the distance that would have triggered a hyper dash.
        /// A value close to 0 indicates a difficult jump (for difficulty calculation).
        /// </summary>
        public float DistanceToHyperDash { get; set; }

        public readonly Bindable<bool> HyperDashBindable = new Bindable<bool>();

        /// <summary>
        /// Whether this fruit can initiate a hyperdash.
        /// </summary>
        public bool HyperDash => HyperDashBindable.Value;

        private CatchHitObject hyperDashTarget;

        /// <summary>
        /// The target fruit if we are to initiate a hyperdash.
        /// </summary>
        [JsonIgnore]
        public CatchHitObject HyperDashTarget
        {
            get => hyperDashTarget;
            set
            {
                hyperDashTarget = value;
                HyperDashBindable.Value = value != null;
            }
        }

        Colour4 IHasComboInformation.GetComboColour(ISkin skin) => IHasComboInformation.GetSkinComboColour(this, skin, IndexInBeatmap + 1);
    }
}
