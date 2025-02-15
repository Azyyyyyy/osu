// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NUnit.Framework;
using osu.Framework.Testing;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Osu.Mods;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using osu.Game.Rulesets.Osu.Skinning.Default;

namespace osu.Game.Rulesets.Osu.Tests.Mods
{
    public class TestSceneOsuModSpunOut : OsuModTestScene
    {
        protected override bool AllowFail => true;

        [Test]
        public void TestSpinnerAutoCompleted() => CreateModTest(new ModTestData
        {
            Mod = new OsuModSpunOut(),
            Autoplay = false,
            Beatmap = singleSpinnerBeatmap,
            PassCondition = () => Player.ChildrenOfType<DrawableSpinner>().SingleOrDefault()?.Progress >= 1
        });

        [TestCase(null)]
        [TestCase(typeof(OsuModDoubleTime))]
        [TestCase(typeof(OsuModHalfTime))]
        public void TestSpinRateUnaffectedByMods(Type additionalModType)
        {
            var mods = new List<Mod> { new OsuModSpunOut() };
            if (additionalModType != null)
                mods.Add((Mod)Activator.CreateInstance(additionalModType));

            CreateModTest(new ModTestData
            {
                Mods = mods,
                Autoplay = false,
                Beatmap = singleSpinnerBeatmap,
                PassCondition = () =>
                {
                    var counter = Player.ChildrenOfType<SpinnerSpmCalculator>().SingleOrDefault();
                    return counter != null && Precision.AlmostEquals(counter.Result.Value, 286, 1);
                }
            });
        }

        private Beatmap singleSpinnerBeatmap => new Beatmap
        {
            HitObjects = new List<HitObject>
            {
                new Spinner
                {
                    Position = new Vector2(256, 192),
                    StartTime = 500,
                    Duration = 2000
                }
            }
        };
    }
}
