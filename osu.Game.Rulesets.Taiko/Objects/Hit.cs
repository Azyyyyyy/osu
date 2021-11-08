// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Colour4Extensions;
using osu.Framework.Graphics;
using osu.Game.Audio;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Taiko.Objects
{
    public class Hit : TaikoStrongableHitObject, IHasDisplayColour
    {
        public readonly Bindable<HitType> TypeBindable = new Bindable<HitType>();

        public Bindable<Colour4> DisplayColour { get; } = new Bindable<Colour4>(COLOUR_CENTRE);

        /// <summary>
        /// The <see cref="HitType"/> that actuates this <see cref="Hit"/>.
        /// </summary>
        public HitType Type
        {
            get => TypeBindable.Value;
            set => TypeBindable.Value = value;
        }

        public static readonly Colour4 COLOUR_CENTRE = Colour4Extensions.FromHex(@"bb1177");
        public static readonly Colour4 COLOUR_RIM = Colour4Extensions.FromHex(@"2299bb");

        public Hit()
        {
            TypeBindable.BindValueChanged(_ =>
            {
                updateSamplesFromType();
                DisplayColour.Value = Type == HitType.Centre ? COLOUR_CENTRE : COLOUR_RIM;
            });

            SamplesBindable.BindCollectionChanged((_, __) => updateTypeFromSamples());
        }

        private void updateTypeFromSamples()
        {
            Type = getRimSamples().Any() ? HitType.Rim : HitType.Centre;
        }

        /// <summary>
        /// Returns an array of any samples which would cause this object to be a "rim" type hit.
        /// </summary>
        private HitSampleInfo[] getRimSamples() => Samples.Where(s => s.Name == HitSampleInfo.HIT_CLAP || s.Name == HitSampleInfo.HIT_WHISTLE).ToArray();

        private void updateSamplesFromType()
        {
            var rimSamples = getRimSamples();

            bool isRimType = Type == HitType.Rim;

            if (isRimType != rimSamples.Any())
            {
                if (isRimType)
                    Samples.Add(new HitSampleInfo(HitSampleInfo.HIT_CLAP));
                else
                {
                    foreach (var sample in rimSamples)
                        Samples.Remove(sample);
                }
            }
        }

        protected override StrongNestedHitObject CreateStrongNestedHit(double startTime) => new StrongNestedHit { StartTime = startTime };

        public class StrongNestedHit : StrongNestedHitObject
        {
        }
    }
}
