// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Mania.UI.Components;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Mania.Tests.Skinning
{
    public class TestSceneKeyArea : ManiaSkinnableTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            SetContents(_ => new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.8f),
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    new ColumnTestContainer(0, ManiaAction.Key1)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Width = 0.5f,
                        Child = new SkinnableDrawable(new ManiaSkinComponent(ManiaSkinComponents.KeyArea), _ => new DefaultKeyArea())
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                    },
                    new ColumnTestContainer(1, ManiaAction.Key2)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Width = 0.5f,
                        Child = new SkinnableDrawable(new ManiaSkinComponent(ManiaSkinComponents.KeyArea), _ => new DefaultKeyArea())
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                    },
                }
            });
        }
    }
}
