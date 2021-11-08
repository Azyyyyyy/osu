// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Colour4Extensions;
using osu.Framework.Extensions.LocalisationExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Resources.Localisation.Web;

namespace osu.Game.Overlays.BeatmapSet
{
    public class BasicStats : Container
    {
        private readonly Statistic length, bpm, circleCount, sliderCount;

        private APIBeatmapSet beatmapSet;

        public APIBeatmapSet BeatmapSet
        {
            get => beatmapSet;
            set
            {
                if (value == beatmapSet) return;

                beatmapSet = value;

                updateDisplay();
            }
        }

        private IBeatmapInfo beatmapInfo;

        public IBeatmapInfo BeatmapInfo
        {
            get => beatmapInfo;
            set
            {
                if (value == beatmapInfo) return;

                beatmapInfo = value;

                updateDisplay();
            }
        }

        private void updateDisplay()
        {
            bpm.Value = BeatmapSet?.BPM.ToLocalisableString(@"0.##") ?? (LocalisableString)"-";

            if (beatmapInfo == null)
            {
                length.Value = string.Empty;
                circleCount.Value = string.Empty;
                sliderCount.Value = string.Empty;
            }
            else
            {
                length.TooltipText = BeatmapsetsStrings.ShowStatsTotalLength(TimeSpan.FromMilliseconds(beatmapInfo.Length).ToFormattedDuration());
                length.Value = TimeSpan.FromMilliseconds(beatmapInfo.Length).ToFormattedDuration();

                var onlineInfo = beatmapInfo as IBeatmapOnlineInfo;

                circleCount.Value = (onlineInfo?.CircleCount ?? 0).ToLocalisableString(@"N0");
                sliderCount.Value = (onlineInfo?.SliderCount ?? 0).ToLocalisableString(@"N0");
            }
        }

        public BasicStats()
        {
            Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Horizontal,
                Children = new[]
                {
                    length = new Statistic(BeatmapStatisticsIconType.Length)
                    {
                        Width = 0.25f,
                        TooltipText = default,
                    },
                    bpm = new Statistic(BeatmapStatisticsIconType.Bpm)
                    {
                        Width = 0.25f,
                        TooltipText = BeatmapsetsStrings.ShowStatsBpm
                    },
                    circleCount = new Statistic(BeatmapStatisticsIconType.Circles)
                    {
                        Width = 0.25f,
                        TooltipText = BeatmapsetsStrings.ShowStatsCountCircles
                    },
                    sliderCount = new Statistic(BeatmapStatisticsIconType.Sliders)
                    {
                        Width = 0.25f,
                        TooltipText = BeatmapsetsStrings.ShowStatsCountSliders
                    },
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            updateDisplay();
        }

        private class Statistic : Container, IHasTooltip
        {
            private readonly OsuSpriteText value;

            public LocalisableString TooltipText { get; set; }

            public LocalisableString Value
            {
                get => value.Text;
                set => this.value.Text = value;
            }

            public Statistic(BeatmapStatisticsIconType icon)
            {
                RelativeSizeAxes = Axes.X;
                Height = 24f;

                Children = new Drawable[]
                {
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            new SpriteIcon
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.Centre,
                                Icon = FontAwesome.Solid.Square,
                                Size = new Vector2(12),
                                Rotation = 45,
                                Colour = Colour4Extensions.FromHex(@"441288"),
                            },
                            new SpriteIcon
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.Centre,
                                Icon = FontAwesome.Regular.Circle,
                                Size = new Vector2(10),
                                Rotation = 0,
                                Colour = Colour4Extensions.FromHex(@"f7dd55"),
                            },
                            new BeatmapStatisticIcon(icon)
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.Centre,
                                Size = new Vector2(10),
                                Colour = Colour4Extensions.FromHex(@"f7dd55"),
                                Scale = new Vector2(0.8f),
                            },
                            value = new OsuSpriteText
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Margin = new MarginPadding { Left = 10 },
                                Font = OsuFont.GetFont(size: 12, weight: FontWeight.Bold),
                            },
                        },
                    },
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colour)
            {
                value.Colour = colour.Yellow;
            }
        }
    }
}
