﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;

namespace osu.Game.Screens.Play.HUD.HitErrorMeters
{
    public abstract class HitErrorMeter : CompositeDrawable, ISkinnableDrawable
    {
        protected HitWindows HitWindows { get; private set; }

        [Resolved]
        private ScoreProcessor processor { get; set; }

        [Resolved]
        private OsuColour colours { get; set; }

        [Resolved(canBeNull: true)]
        private GameplayClockContainer gameplayClockContainer { get; set; }

        public bool UsesFixedAnchor { get; set; }

        [BackgroundDependencyLoader(true)]
        private void load(DrawableRuleset drawableRuleset)
        {
            HitWindows = drawableRuleset?.FirstAvailableHitWindows ?? HitWindows.Empty;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (gameplayClockContainer != null)
                gameplayClockContainer.OnSeek += Clear;

            processor.NewJudgement += OnNewJudgement;
        }

        protected abstract void OnNewJudgement(JudgementResult judgement);

        protected Colour4 GetColourForHitResult(HitResult result)
        {
            switch (result)
            {
                case HitResult.SmallTickMiss:
                case HitResult.LargeTickMiss:
                case HitResult.Miss:
                    return colours.Red;

                case HitResult.Meh:
                    return colours.Yellow;

                case HitResult.Ok:
                    return colours.Green;

                case HitResult.Good:
                    return colours.GreenLight;

                case HitResult.SmallTickHit:
                case HitResult.LargeTickHit:
                case HitResult.Great:
                    return colours.Blue;

                default:
                    return colours.BlueLight;
            }
        }

        /// <summary>
        /// Invoked by <see cref="GameplayClockContainer.OnSeek"/>.
        /// Any inheritors of <see cref="HitErrorMeter"/> should have this method clear their container that displays the hit error results.
        /// </summary>
        public abstract void Clear();

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (processor != null)
                processor.NewJudgement -= OnNewJudgement;

            if (gameplayClockContainer != null)
                gameplayClockContainer.OnSeek -= Clear;
        }
    }
}
