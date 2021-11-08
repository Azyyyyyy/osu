// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Drawables;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Screens.Select
{
    internal class BeatmapInfoWedgeBackground : CompositeDrawable
    {
        private readonly WorkingBeatmap beatmap;

        public BeatmapInfoWedgeBackground(WorkingBeatmap beatmap)
        {
            this.beatmap = beatmap;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;

            InternalChild = new BufferedContainer(cachedFrameBuffer: true)
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    // We will create the white-to-black gradient by modulating transparency and having
                    // a black backdrop. This results in an sRGB-space gradient and not linear space,
                    // transitioning from white to black more perceptually uniformly.
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Black,
                    },
                    // We use a container, such that we can set the colour gradient to go across the
                    // vertices of the masked container instead of the vertices of the (larger) sprite.
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = ColourInfo.GradientVertical(Colour4.White, Colour4.White.Opacity(0.3f)),
                        Children = new[]
                        {
                            // Zoomed-in and cropped beatmap background
                            new BeatmapBackgroundSprite(beatmap)
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                FillMode = FillMode.Fill,
                            },
                        },
                    },
                }
            };
        }
    }
}
