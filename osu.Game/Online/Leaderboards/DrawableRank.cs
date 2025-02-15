﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Colour4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Sprites;
using osu.Game.Scoring;

namespace osu.Game.Online.Leaderboards
{
    public class DrawableRank : CompositeDrawable
    {
        private readonly ScoreRank rank;

        public DrawableRank(ScoreRank rank)
        {
            this.rank = rank;

            RelativeSizeAxes = Axes.Both;
            FillMode = FillMode.Fit;
            FillAspectRatio = 2;

            var rankColour = OsuColour.ForRank(rank);
            InternalChild = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(64, 32),
                Strategy = DrawSizePreservationStrategy.Minimum,
                Child = new CircularContainer
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = rankColour,
                        },
                        new Triangles
                        {
                            RelativeSizeAxes = Axes.Both,
                            ColourDark = rankColour.Darken(0.1f),
                            ColourLight = rankColour.Lighten(0.1f),
                            Velocity = 0.25f,
                        },
                        new OsuSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Spacing = new Vector2(-3, 0),
                            Padding = new MarginPadding { Top = 5 },
                            Colour = getRankNameColour(),
                            Font = OsuFont.Numeric.With(size: 25),
                            Text = GetRankName(rank),
                            ShadowColour = Colour4.Black.Opacity(0.3f),
                            ShadowOffset = new Vector2(0, 0.08f),
                            Shadow = true,
                        },
                    }
                }
            };
        }

        public static string GetRankName(ScoreRank rank) => rank.GetDescription().TrimEnd('+');

        /// <summary>
        ///  Retrieves the grade text colour.
        /// </summary>
        private ColourInfo getRankNameColour()
        {
            switch (rank)
            {
                case ScoreRank.XH:
                case ScoreRank.SH:
                    return ColourInfo.GradientVertical(Colour4.White, Colour4Extensions.FromHex("afdff0"));

                case ScoreRank.X:
                case ScoreRank.S:
                    return ColourInfo.GradientVertical(Colour4Extensions.FromHex(@"ffe7a8"), Colour4Extensions.FromHex(@"ffb800"));

                case ScoreRank.A:
                    return Colour4Extensions.FromHex(@"275227");

                case ScoreRank.B:
                    return Colour4Extensions.FromHex(@"553a2b");

                case ScoreRank.C:
                    return Colour4Extensions.FromHex(@"473625");

                default:
                    return Colour4Extensions.FromHex(@"512525");
            }
        }
    }
}
