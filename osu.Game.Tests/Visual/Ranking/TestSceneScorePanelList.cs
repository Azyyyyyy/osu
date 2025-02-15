// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Framework.Utils;
using osu.Game.Rulesets.Osu;
using osu.Game.Scoring;
using osu.Game.Screens.Ranking;
using Silk.NET.Input;

namespace osu.Game.Tests.Visual.Ranking
{
    public class TestSceneScorePanelList : OsuManualInputManagerTestScene
    {
        private ScorePanelList list;

        [Test]
        public void TestEmptyList()
        {
            createListStep(() => new ScorePanelList());
        }

        [Test]
        public void TestEmptyListWithSelectedScore()
        {
            createListStep(() => new ScorePanelList
            {
                SelectedScore = { Value = new TestScoreInfo(new OsuRuleset().RulesetInfo) }
            });
        }

        [Test]
        public void TestAddPanelAfterSelectingScore()
        {
            var score = new TestScoreInfo(new OsuRuleset().RulesetInfo);

            createListStep(() => new ScorePanelList
            {
                SelectedScore = { Value = score }
            });

            AddStep("add panel", () => list.AddScore(score));

            assertScoreState(score, true);
            assertExpandedPanelCentred();
        }

        [Test]
        public void TestAddPanelBeforeSelectingScore()
        {
            var score = new TestScoreInfo(new OsuRuleset().RulesetInfo);

            createListStep(() => new ScorePanelList());

            AddStep("add panel", () => list.AddScore(score));

            assertScoreState(score, false);
            assertFirstPanelCentred();

            AddStep("select score", () => list.SelectedScore.Value = score);

            assertScoreState(score, true);
            assertExpandedPanelCentred();
        }

        [Test]
        public void TestAddManyNonExpandedPanels()
        {
            createListStep(() => new ScorePanelList());

            AddStep("add many scores", () =>
            {
                for (int i = 0; i < 20; i++)
                    list.AddScore(new TestScoreInfo(new OsuRuleset().RulesetInfo));
            });

            assertFirstPanelCentred();
        }

        [Test]
        public void TestAddManyScoresAfterExpandedPanel()
        {
            var initialScore = new TestScoreInfo(new OsuRuleset().RulesetInfo);

            createListStep(() => new ScorePanelList());

            AddStep("add initial panel and select", () =>
            {
                list.AddScore(initialScore);
                list.SelectedScore.Value = initialScore;
            });

            AddStep("add many scores", () =>
            {
                for (int i = 0; i < 20; i++)
                    list.AddScore(new TestScoreInfo(new OsuRuleset().RulesetInfo) { TotalScore = initialScore.TotalScore - i - 1 });
            });

            assertScoreState(initialScore, true);
            assertExpandedPanelCentred();
        }

        [Test]
        public void TestAddManyScoresBeforeExpandedPanel()
        {
            var initialScore = new TestScoreInfo(new OsuRuleset().RulesetInfo);

            createListStep(() => new ScorePanelList());

            AddStep("add initial panel and select", () =>
            {
                list.AddScore(initialScore);
                list.SelectedScore.Value = initialScore;
            });

            AddStep("add scores", () =>
            {
                for (int i = 0; i < 20; i++)
                    list.AddScore(new TestScoreInfo(new OsuRuleset().RulesetInfo) { TotalScore = initialScore.TotalScore + i + 1 });
            });

            assertScoreState(initialScore, true);
            assertExpandedPanelCentred();
        }

        [Test]
        public void TestAddManyPanelsOnBothSidesOfExpandedPanel()
        {
            var initialScore = new TestScoreInfo(new OsuRuleset().RulesetInfo);

            createListStep(() => new ScorePanelList());

            AddStep("add initial panel and select", () =>
            {
                list.AddScore(initialScore);
                list.SelectedScore.Value = initialScore;
            });

            AddStep("add scores after", () =>
            {
                for (int i = 0; i < 20; i++)
                    list.AddScore(new TestScoreInfo(new OsuRuleset().RulesetInfo) { TotalScore = initialScore.TotalScore - i - 1 });

                for (int i = 0; i < 20; i++)
                    list.AddScore(new TestScoreInfo(new OsuRuleset().RulesetInfo) { TotalScore = initialScore.TotalScore + i + 1 });
            });

            assertScoreState(initialScore, true);
            assertExpandedPanelCentred();
        }

        [Test]
        public void TestSelectMultipleScores()
        {
            var firstScore = new TestScoreInfo(new OsuRuleset().RulesetInfo);
            var secondScore = new TestScoreInfo(new OsuRuleset().RulesetInfo);

            firstScore.User.Username = "A";
            secondScore.User.Username = "B";

            createListStep(() => new ScorePanelList());

            AddStep("add scores and select first", () =>
            {
                list.AddScore(firstScore);
                list.AddScore(secondScore);
                list.SelectedScore.Value = firstScore;
            });

            AddUntilStep("wait for load", () => list.AllPanelsVisible);

            assertScoreState(firstScore, true);
            assertScoreState(secondScore, false);

            AddStep("select second score", () =>
            {
                InputManager.MoveMouseTo(list.ChildrenOfType<ScorePanel>().Single(p => p.Score == secondScore));
                InputManager.Click(MouseButton.Left);
            });

            assertScoreState(firstScore, false);
            assertScoreState(secondScore, true);
            assertExpandedPanelCentred();
        }

        [Test]
        public void TestAddScoreImmediately()
        {
            var score = new TestScoreInfo(new OsuRuleset().RulesetInfo);

            createListStep(() =>
            {
                var newList = new ScorePanelList { SelectedScore = { Value = score } };
                newList.AddScore(score);
                return newList;
            });

            assertScoreState(score, true);
            assertExpandedPanelCentred();
        }

        [Test]
        public void TestKeyboardNavigation()
        {
            var lowestScore = new TestScoreInfo(new OsuRuleset().RulesetInfo) { MaxCombo = 100 };
            var middleScore = new TestScoreInfo(new OsuRuleset().RulesetInfo) { MaxCombo = 200 };
            var highestScore = new TestScoreInfo(new OsuRuleset().RulesetInfo) { MaxCombo = 300 };

            createListStep(() => new ScorePanelList());

            AddStep("add scores and select middle", () =>
            {
                // order of addition purposefully scrambled.
                list.AddScore(middleScore);
                list.AddScore(lowestScore);
                list.AddScore(highestScore);
                list.SelectedScore.Value = middleScore;
            });

            AddUntilStep("wait for all scores to be visible", () => list.ChildrenOfType<ScorePanelTrackingContainer>().All(t => t.IsPresent));

            assertScoreState(highestScore, false);
            assertScoreState(middleScore, true);
            assertScoreState(lowestScore, false);

            AddStep("press left", () => InputManager.Key(Key.Left));

            assertScoreState(highestScore, true);
            assertScoreState(middleScore, false);
            assertScoreState(lowestScore, false);
            assertExpandedPanelCentred();

            AddStep("press left at start of list", () => InputManager.Key(Key.Left));

            assertScoreState(highestScore, true);
            assertScoreState(middleScore, false);
            assertScoreState(lowestScore, false);
            assertExpandedPanelCentred();

            AddStep("press right", () => InputManager.Key(Key.Right));

            assertScoreState(highestScore, false);
            assertScoreState(middleScore, true);
            assertScoreState(lowestScore, false);
            assertExpandedPanelCentred();

            AddStep("press right again", () => InputManager.Key(Key.Right));

            assertScoreState(highestScore, false);
            assertScoreState(middleScore, false);
            assertScoreState(lowestScore, true);
            assertExpandedPanelCentred();

            AddStep("press right at end of list", () => InputManager.Key(Key.Right));

            assertScoreState(highestScore, false);
            assertScoreState(middleScore, false);
            assertScoreState(lowestScore, true);
            assertExpandedPanelCentred();

            AddStep("press left", () => InputManager.Key(Key.Left));

            assertScoreState(highestScore, false);
            assertScoreState(middleScore, true);
            assertScoreState(lowestScore, false);
            assertExpandedPanelCentred();
        }

        private void createListStep(Func<ScorePanelList> creationFunc)
        {
            AddStep("create list", () => Child = list = creationFunc().With(d =>
            {
                d.Anchor = Anchor.Centre;
                d.Origin = Anchor.Centre;
            }));

            AddUntilStep("wait for load", () => list.IsLoaded);
        }

        private void assertExpandedPanelCentred() => AddUntilStep("expanded panel centred", () =>
        {
            var expandedPanel = list.ChildrenOfType<ScorePanel>().Single(p => p.State == PanelState.Expanded);
            return Precision.AlmostEquals(expandedPanel.ScreenSpaceDrawQuad.Centre.X, list.ScreenSpaceDrawQuad.Centre.X, 1);
        });

        private void assertFirstPanelCentred()
            => AddUntilStep("first panel centred", () => Precision.AlmostEquals(list.ChildrenOfType<ScorePanel>().First().ScreenSpaceDrawQuad.Centre.X, list.ScreenSpaceDrawQuad.Centre.X, 1));

        private void assertScoreState(ScoreInfo score, bool expanded)
            => AddUntilStep($"score expanded = {expanded}", () => (list.ChildrenOfType<ScorePanel>().Single(p => p.Score == score).State == PanelState.Expanded) == expanded);
    }
}
