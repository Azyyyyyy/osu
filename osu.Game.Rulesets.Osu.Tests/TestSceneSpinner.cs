﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Osu.Tests
{
    [TestFixture]
    public class TestSceneSpinner : OsuSkinnableTestScene
    {
        private int depthIndex;

        private TestDrawableSpinner drawableSpinner;

        [TestCase(true)]
        [TestCase(false)]
        public void TestVariousSpinners(bool autoplay)
        {
            string term = autoplay ? "Hit" : "Miss";
            AddStep($"{term} Big", () => SetContents(_ => testSingle(2, autoplay)));
            AddStep($"{term} Medium", () => SetContents(_ => testSingle(5, autoplay)));
            AddStep($"{term} Small", () => SetContents(_ => testSingle(7, autoplay)));
        }

        [Test]
        public void TestSpinningSamplePitchShift()
        {
            AddStep("Add spinner", () => SetContents(_ => testSingle(5, true, 4000)));
            AddUntilStep("Pitch starts low", () => getSpinningSample().Frequency.Value < 0.8);
            AddUntilStep("Pitch increases", () => getSpinningSample().Frequency.Value > 0.8);

            PausableSkinnableSound getSpinningSample() => drawableSpinner.ChildrenOfType<PausableSkinnableSound>().FirstOrDefault(s => s.Samples.Any(i => i.LookupNames.Any(l => l.Contains("spinnerspin"))));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void TestLongSpinner(bool autoplay)
        {
            AddStep("Very long spinner", () => SetContents(_ => testSingle(5, autoplay, 4000)));
            AddUntilStep("Wait for completion", () => drawableSpinner.Result.HasResult);
            AddUntilStep("Check correct progress", () => drawableSpinner.Progress == (autoplay ? 1 : 0));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void TestSuperShortSpinner(bool autoplay)
        {
            AddStep("Very short spinner", () => SetContents(_ => testSingle(5, autoplay, 200)));
            AddUntilStep("Wait for completion", () => drawableSpinner.Result.HasResult);
            AddUntilStep("Short spinner implicitly completes", () => drawableSpinner.Progress == 1);
        }

        private Drawable testSingle(float circleSize, bool auto = false, double length = 3000)
        {
            const double delay = 2000;

            var spinner = new Spinner
            {
                StartTime = Time.Current + delay,
                EndTime = Time.Current + delay + length,
                Samples = new List<HitSampleInfo>
                {
                    new HitSampleInfo("hitnormal")
                }
            };

            spinner.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty { CircleSize = circleSize });

            drawableSpinner = new TestDrawableSpinner(spinner, auto)
            {
                Anchor = Anchor.Centre,
                Depth = depthIndex++,
                Scale = new Vector2(0.75f)
            };

            foreach (var mod in SelectedMods.Value.OfType<IApplicableToDrawableHitObject>())
                mod.ApplyToDrawableHitObject(drawableSpinner);

            return drawableSpinner;
        }

        private class TestDrawableSpinner : DrawableSpinner
        {
            private readonly bool auto;

            public TestDrawableSpinner(Spinner s, bool auto)
                : base(s)
            {
                this.auto = auto;
            }

            protected override void Update()
            {
                base.Update();
                if (auto)
                    RotationTracker.AddRotation((float)(Clock.ElapsedFrameTime * 2));
            }
        }
    }
}
