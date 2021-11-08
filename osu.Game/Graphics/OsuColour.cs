// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions.Colour4Extensions;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Overlays;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Game.Utils;

namespace osu.Game.Graphics
{
    public class OsuColour
    {
        public static Colour4 Gray(float amt) => new Colour4(amt, amt, amt, 1f);
        public static Colour4 Gray(byte amt) => new Colour4(amt, amt, amt, 255);

        /// <summary>
        /// Retrieves the colour for a <see cref="DifficultyRating"/>.
        /// </summary>
        /// <remarks>
        /// Sourced from the @diff-{rating} variables in https://github.com/ppy/osu-web/blob/71fbab8936d79a7929d13854f5e854b4f383b236/resources/assets/less/variables.less.
        /// </remarks>
        public Colour4 ForDifficultyRating(DifficultyRating difficulty, bool useLighterColour = false)
        {
            switch (difficulty)
            {
                case DifficultyRating.Easy:
                    return Colour4Extensions.FromHex("4ebfff");

                case DifficultyRating.Normal:
                    return Colour4Extensions.FromHex("66ff91");

                case DifficultyRating.Hard:
                    return Colour4Extensions.FromHex("f7e85d");

                case DifficultyRating.Insane:
                    return Colour4Extensions.FromHex("ff7e68");

                case DifficultyRating.Expert:
                    return Colour4Extensions.FromHex("fe3c71");

                case DifficultyRating.ExpertPlus:
                    return Colour4Extensions.FromHex("6662dd");

                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty));
            }
        }

        public Colour4 ForStarDifficulty(double starDifficulty) => ColourUtils.SampleFromLinearGradient(new[]
        {
            (1.5f, Colour4Extensions.FromHex("4fc0ff")),
            (2.0f, Colour4Extensions.FromHex("4fffd5")),
            (2.5f, Colour4Extensions.FromHex("7cff4f")),
            (3.25f, Colour4Extensions.FromHex("f6f05c")),
            (4.5f, Colour4Extensions.FromHex("ff8068")),
            (6.0f, Colour4Extensions.FromHex("ff3c71")),
            (7.0f, Colour4Extensions.FromHex("6563de")),
            (8.0f, Colour4Extensions.FromHex("18158e")),
            (8.0f, Colour4.Black),
        }, (float)Math.Round(starDifficulty, 2, MidpointRounding.AwayFromZero));

        /// <summary>
        /// Retrieves the colour for a <see cref="ScoreRank"/>.
        /// </summary>
        public static Colour4 ForRank(ScoreRank rank)
        {
            switch (rank)
            {
                case ScoreRank.XH:
                case ScoreRank.X:
                    return Colour4Extensions.FromHex(@"de31ae");

                case ScoreRank.SH:
                case ScoreRank.S:
                    return Colour4Extensions.FromHex(@"02b5c3");

                case ScoreRank.A:
                    return Colour4Extensions.FromHex(@"88da20");

                case ScoreRank.B:
                    return Colour4Extensions.FromHex(@"e3b130");

                case ScoreRank.C:
                    return Colour4Extensions.FromHex(@"ff8e5d");

                default:
                    return Colour4Extensions.FromHex(@"ff5a5a");
            }
        }

        /// <summary>
        /// Retrieves the colour for a <see cref="HitResult"/>.
        /// </summary>
        public Colour4 ForHitResult(HitResult judgement)
        {
            switch (judgement)
            {
                case HitResult.Perfect:
                case HitResult.Great:
                    return Blue;

                case HitResult.Ok:
                case HitResult.Good:
                    return Green;

                case HitResult.Meh:
                    return Yellow;

                case HitResult.Miss:
                    return Red;

                default:
                    return Colour4.White;
            }
        }

        /// <summary>
        /// Retrieves a colour for the given <see cref="BeatmapSetOnlineStatus"/>.
        /// A <see langword="null"/> value indicates that a "background" shade from the local <see cref="OverlayColourProvider"/>
        /// (or another fallback colour) should be used.
        /// </summary>
        /// <remarks>
        /// Sourced from web: https://github.com/ppy/osu-web/blob/007eebb1916ed5cb6a7866d82d8011b1060a945e/resources/assets/less/layout.less#L36-L50
        /// </remarks>
        public static Colour4? ForBeatmapSetOnlineStatus(BeatmapSetOnlineStatus status)
        {
            switch (status)
            {
                case BeatmapSetOnlineStatus.Ranked:
                case BeatmapSetOnlineStatus.Approved:
                    return Colour4Extensions.FromHex(@"b3ff66");

                case BeatmapSetOnlineStatus.Loved:
                    return Colour4Extensions.FromHex(@"ff66ab");

                case BeatmapSetOnlineStatus.Qualified:
                    return Colour4Extensions.FromHex(@"66ccff");

                case BeatmapSetOnlineStatus.Pending:
                    return Colour4Extensions.FromHex(@"ffd966");

                case BeatmapSetOnlineStatus.WIP:
                    return Colour4Extensions.FromHex(@"ff9966");

                case BeatmapSetOnlineStatus.Graveyard:
                    return Colour4.Black;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns a foreground text colour that is supposed to contrast well with
        /// the supplied <paramref name="backgroundColour"/>.
        /// </summary>
        public static Colour4 ForegroundTextColourFor(Colour4 backgroundColour)
        {
            // formula taken from the RGB->YIQ conversions: https://en.wikipedia.org/wiki/YIQ
            // brightness here is equivalent to the Y component in the above colour model, which is a rough estimate of lightness.
            float brightness = 0.299f * backgroundColour.R + 0.587f * backgroundColour.G + 0.114f * backgroundColour.B;
            return Gray(brightness > 0.5f ? 0.2f : 0.9f);
        }

        public readonly Colour4 TeamColourRed = Colour4Extensions.FromHex("#AA1414");
        public readonly Colour4 TeamColourBlue = Colour4Extensions.FromHex("#1462AA");

        // See https://github.com/ppy/osu-web/blob/master/resources/assets/less/colors.less
        public readonly Colour4 PurpleLighter = Colour4Extensions.FromHex(@"eeeeff");
        public readonly Colour4 PurpleLight = Colour4Extensions.FromHex(@"aa88ff");
        public readonly Colour4 PurpleLightAlternative = Colour4Extensions.FromHex(@"cba4da");
        public readonly Colour4 Purple = Colour4Extensions.FromHex(@"8866ee");
        public readonly Colour4 PurpleDark = Colour4Extensions.FromHex(@"6644cc");
        public readonly Colour4 PurpleDarkAlternative = Colour4Extensions.FromHex(@"312436");
        public readonly Colour4 PurpleDarker = Colour4Extensions.FromHex(@"441188");

        public readonly Colour4 PinkLighter = Colour4Extensions.FromHex(@"ffddee");
        public readonly Colour4 PinkLight = Colour4Extensions.FromHex(@"ff99cc");
        public readonly Colour4 Pink = Colour4Extensions.FromHex(@"ff66aa");
        public readonly Colour4 PinkDark = Colour4Extensions.FromHex(@"cc5288");
        public readonly Colour4 PinkDarker = Colour4Extensions.FromHex(@"bb1177");

        public readonly Colour4 BlueLighter = Colour4Extensions.FromHex(@"ddffff");
        public readonly Colour4 BlueLight = Colour4Extensions.FromHex(@"99eeff");
        public readonly Colour4 Blue = Colour4Extensions.FromHex(@"66ccff");
        public readonly Colour4 BlueDark = Colour4Extensions.FromHex(@"44aadd");
        public readonly Colour4 BlueDarker = Colour4Extensions.FromHex(@"2299bb");

        public readonly Colour4 YellowLighter = Colour4Extensions.FromHex(@"ffffdd");
        public readonly Colour4 YellowLight = Colour4Extensions.FromHex(@"ffdd55");
        public readonly Colour4 Yellow = Colour4Extensions.FromHex(@"ffcc22");
        public readonly Colour4 YellowDark = Colour4Extensions.FromHex(@"eeaa00");
        public readonly Colour4 YellowDarker = Colour4Extensions.FromHex(@"cc6600");

        public readonly Colour4 GreenLighter = Colour4Extensions.FromHex(@"eeffcc");
        public readonly Colour4 GreenLight = Colour4Extensions.FromHex(@"b3d944");
        public readonly Colour4 Green = Colour4Extensions.FromHex(@"88b300");
        public readonly Colour4 GreenDark = Colour4Extensions.FromHex(@"668800");
        public readonly Colour4 GreenDarker = Colour4Extensions.FromHex(@"445500");

        public readonly Colour4 Sky = Colour4Extensions.FromHex(@"6bb5ff");
        public readonly Colour4 GreySkyLighter = Colour4Extensions.FromHex(@"c6e3f4");
        public readonly Colour4 GreySkyLight = Colour4Extensions.FromHex(@"8ab3cc");
        public readonly Colour4 GreySky = Colour4Extensions.FromHex(@"405461");
        public readonly Colour4 GreySkyDark = Colour4Extensions.FromHex(@"303d47");
        public readonly Colour4 GreySkyDarker = Colour4Extensions.FromHex(@"21272c");

        public readonly Colour4 Seafoam = Colour4Extensions.FromHex(@"05ffa2");
        public readonly Colour4 GreySeafoamLighter = Colour4Extensions.FromHex(@"9ebab1");
        public readonly Colour4 GreySeafoamLight = Colour4Extensions.FromHex(@"4d7365");
        public readonly Colour4 GreySeafoam = Colour4Extensions.FromHex(@"33413c");
        public readonly Colour4 GreySeafoamDark = Colour4Extensions.FromHex(@"2c3532");
        public readonly Colour4 GreySeafoamDarker = Colour4Extensions.FromHex(@"1e2422");

        public readonly Colour4 Cyan = Colour4Extensions.FromHex(@"05f4fd");
        public readonly Colour4 GreyCyanLighter = Colour4Extensions.FromHex(@"77b1b3");
        public readonly Colour4 GreyCyanLight = Colour4Extensions.FromHex(@"436d6f");
        public readonly Colour4 GreyCyan = Colour4Extensions.FromHex(@"293d3e");
        public readonly Colour4 GreyCyanDark = Colour4Extensions.FromHex(@"243536");
        public readonly Colour4 GreyCyanDarker = Colour4Extensions.FromHex(@"1e2929");

        public readonly Colour4 Lime = Colour4Extensions.FromHex(@"82ff05");
        public readonly Colour4 GreyLimeLighter = Colour4Extensions.FromHex(@"deff87");
        public readonly Colour4 GreyLimeLight = Colour4Extensions.FromHex(@"657259");
        public readonly Colour4 GreyLime = Colour4Extensions.FromHex(@"3f443a");
        public readonly Colour4 GreyLimeDark = Colour4Extensions.FromHex(@"32352e");
        public readonly Colour4 GreyLimeDarker = Colour4Extensions.FromHex(@"2e302b");

        public readonly Colour4 Violet = Colour4Extensions.FromHex(@"bf04ff");
        public readonly Colour4 GreyVioletLighter = Colour4Extensions.FromHex(@"ebb8fe");
        public readonly Colour4 GreyVioletLight = Colour4Extensions.FromHex(@"685370");
        public readonly Colour4 GreyViolet = Colour4Extensions.FromHex(@"46334d");
        public readonly Colour4 GreyVioletDark = Colour4Extensions.FromHex(@"2c2230");
        public readonly Colour4 GreyVioletDarker = Colour4Extensions.FromHex(@"201823");

        public readonly Colour4 Carmine = Colour4Extensions.FromHex(@"ff0542");
        public readonly Colour4 GreyCarmineLighter = Colour4Extensions.FromHex(@"deaab4");
        public readonly Colour4 GreyCarmineLight = Colour4Extensions.FromHex(@"644f53");
        public readonly Colour4 GreyCarmine = Colour4Extensions.FromHex(@"342b2d");
        public readonly Colour4 GreyCarmineDark = Colour4Extensions.FromHex(@"302a2b");
        public readonly Colour4 GreyCarmineDarker = Colour4Extensions.FromHex(@"241d1e");

        public readonly Colour4 Gray0 = Colour4Extensions.FromHex(@"000");
        public readonly Colour4 Gray1 = Colour4Extensions.FromHex(@"111");
        public readonly Colour4 Gray2 = Colour4Extensions.FromHex(@"222");
        public readonly Colour4 Gray3 = Colour4Extensions.FromHex(@"333");
        public readonly Colour4 Gray4 = Colour4Extensions.FromHex(@"444");
        public readonly Colour4 Gray5 = Colour4Extensions.FromHex(@"555");
        public readonly Colour4 Gray6 = Colour4Extensions.FromHex(@"666");
        public readonly Colour4 Gray7 = Colour4Extensions.FromHex(@"777");
        public readonly Colour4 Gray8 = Colour4Extensions.FromHex(@"888");
        public readonly Colour4 Gray9 = Colour4Extensions.FromHex(@"999");
        public readonly Colour4 GrayA = Colour4Extensions.FromHex(@"aaa");
        public readonly Colour4 GrayB = Colour4Extensions.FromHex(@"bbb");
        public readonly Colour4 GrayC = Colour4Extensions.FromHex(@"ccc");
        public readonly Colour4 GrayD = Colour4Extensions.FromHex(@"ddd");
        public readonly Colour4 GrayE = Colour4Extensions.FromHex(@"eee");
        public readonly Colour4 GrayF = Colour4Extensions.FromHex(@"fff");

        /// <summary>
        /// Equivalent to <see cref="OverlayColourProvider.Pink"/>'s <see cref="OverlayColourProvider.Colour3"/>.
        /// </summary>
        public readonly Colour4 Pink3 = Colour4Extensions.FromHex(@"cc3378");

        /// <summary>
        /// Equivalent to <see cref="OverlayColourProvider.Blue"/>'s <see cref="OverlayColourProvider.Colour3"/>.
        /// </summary>
        public readonly Colour4 Blue3 = Colour4Extensions.FromHex(@"3399cc");

        public readonly Colour4 Lime0 = Colour4Extensions.FromHex(@"ccff99");

        /// <summary>
        /// Equivalent to <see cref="OverlayColourProvider.Lime"/>'s <see cref="OverlayColourProvider.Colour1"/>.
        /// </summary>
        public readonly Colour4 Lime1 = Colour4Extensions.FromHex(@"b2ff66");

        /// <summary>
        /// Equivalent to <see cref="OverlayColourProvider.Lime"/>'s <see cref="OverlayColourProvider.Colour3"/>.
        /// </summary>
        public readonly Colour4 Lime3 = Colour4Extensions.FromHex(@"7fcc33");

        /// <summary>
        /// Equivalent to <see cref="OverlayColourProvider.Orange"/>'s <see cref="OverlayColourProvider.Colour1"/>.
        /// </summary>
        public readonly Colour4 Orange1 = Colour4Extensions.FromHex(@"ffd966");

        // Content Background
        public readonly Colour4 B5 = Colour4Extensions.FromHex(@"222a28");

        public readonly Colour4 RedLighter = Colour4Extensions.FromHex(@"ffeded");
        public readonly Colour4 RedLight = Colour4Extensions.FromHex(@"ed7787");
        public readonly Colour4 Red = Colour4Extensions.FromHex(@"ed1121");
        public readonly Colour4 RedDark = Colour4Extensions.FromHex(@"ba0011");
        public readonly Colour4 RedDarker = Colour4Extensions.FromHex(@"870000");

        public readonly Colour4 ChatBlue = Colour4Extensions.FromHex(@"17292e");

        public readonly Colour4 ContextMenuGray = Colour4Extensions.FromHex(@"223034");
    }
}
