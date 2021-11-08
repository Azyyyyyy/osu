// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Extensions.Colour4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Osu;
using osu.Game.Screens.Ranking.Expanded;

namespace osu.Game.Tests.Visual.Ranking
{
    public class TestSceneExpandedPanelTopContent : OsuTestScene
    {
        public TestSceneExpandedPanelTopContent()
        {
            Child = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(500, 200),
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4Extensions.FromHex("#444"),
                    },
                    new ExpandedPanelTopContent(new TestScoreInfo(new OsuRuleset().RulesetInfo).User),
                }
            };
        }
    }
}
