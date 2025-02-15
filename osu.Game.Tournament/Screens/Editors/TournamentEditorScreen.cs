// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Tournament.Components;

namespace osu.Game.Tournament.Screens.Editors
{
    public abstract class TournamentEditorScreen<TDrawable, TModel> : TournamentScreen, IProvideVideo
        where TDrawable : Drawable, IModelBacked<TModel>
        where TModel : class, new()
    {
        protected abstract BindableList<TModel> Storage { get; }

        private FillFlowContainer<TDrawable> flow;

        [Resolved(canBeNull: true)]
        private TournamentSceneManager sceneManager { get; set; }

        protected ControlPanel ControlPanel;

        private readonly TournamentScreen parentScreen;
        private BackButton backButton;

        protected TournamentEditorScreen(TournamentScreen parentScreen = null)
        {
            this.parentScreen = parentScreen;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = OsuColour.Gray(0.2f),
                },
                new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Child = flow = new FillFlowContainer<TDrawable>
                    {
                        Direction = FillDirection.Vertical,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Spacing = new Vector2(20),
                    },
                },
                ControlPanel = new ControlPanel
                {
                    Children = new Drawable[]
                    {
                        new TourneyButton
                        {
                            RelativeSizeAxes = Axes.X,
                            Text = "Add new",
                            Action = () => Storage.Add(new TModel())
                        },
                        new DangerousSettingsButton
                        {
                            RelativeSizeAxes = Axes.X,
                            Text = "Clear all",
                            Action = Storage.Clear
                        },
                    }
                }
            });

            if (parentScreen != null)
            {
                AddInternal(backButton = new BackButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    State = { Value = Visibility.Visible },
                    Action = () => sceneManager?.SetScreen(parentScreen.GetType())
                });

                flow.Padding = new MarginPadding { Bottom = backButton.Height * 2 };
            }

            Storage.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        args.NewItems.Cast<TModel>().ForEach(i => flow.Add(CreateDrawable(i)));
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        args.OldItems.Cast<TModel>().ForEach(i => flow.RemoveAll(d => d.Model == i));
                        break;
                }
            };

            foreach (var model in Storage)
                flow.Add(CreateDrawable(model));
        }

        protected abstract TDrawable CreateDrawable(TModel model);
    }
}
