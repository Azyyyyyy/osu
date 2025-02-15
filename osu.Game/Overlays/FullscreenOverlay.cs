// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Graphics.Containers;
using osu.Game.Online.API;

namespace osu.Game.Overlays
{
    public abstract class FullscreenOverlay<T> : WaveOverlayContainer, INamedOverlayComponent
        where T : OverlayHeader
    {
        public virtual string IconTexture => Header.Title.IconTexture ?? string.Empty;
        public virtual LocalisableString Title => Header.Title.Title;
        public virtual LocalisableString Description => Header.Title.Description;

        public T Header { get; }

        protected virtual Colour4 BackgroundColour => ColourProvider.Background5;

        [Resolved]
        protected IAPIProvider API { get; private set; }

        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        protected override Container<Drawable> Content => content;

        private readonly Container content;

        protected FullscreenOverlay(OverlayColourScheme colourScheme)
        {
            Header = CreateHeader();

            ColourProvider = new OverlayColourProvider(colourScheme);

            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Width = 0.85f;
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            Masking = true;

            EdgeEffect = new EdgeEffectParameters
            {
                Colour = Colour4.Black.Opacity(0),
                Type = EdgeEffectType.Shadow,
                Radius = 10
            };

            base.Content.AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = BackgroundColour
                },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Waves.FirstWaveColour = ColourProvider.Light4;
            Waves.SecondWaveColour = ColourProvider.Light3;
            Waves.ThirdWaveColour = ColourProvider.Dark4;
            Waves.FourthWaveColour = ColourProvider.Dark3;
        }

        [NotNull]
        protected abstract T CreateHeader();

        public override void Show()
        {
            if (State.Value == Visibility.Visible)
            {
                // re-trigger the state changed so we can potentially surface to front
                State.TriggerChange();
            }
            else
            {
                base.Show();
            }
        }

        protected override void PopIn()
        {
            base.PopIn();
            FadeEdgeEffectTo(0.4f, WaveContainer.APPEAR_DURATION, Easing.Out);
        }

        protected override void PopOut()
        {
            base.PopOut();
            FadeEdgeEffectTo(0, WaveContainer.DISAPPEAR_DURATION, Easing.In).OnComplete(_ => PopOutComplete());
        }

        protected virtual void PopOutComplete()
        {
        }
    }
}
