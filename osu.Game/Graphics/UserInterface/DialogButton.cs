﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Numerics;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Graphics.UserInterface
{
    public class DialogButton : OsuClickableContainer, IStateful<SelectionState>
    {
        private const float idle_width = 0.8f;
        private const float hover_width = 0.9f;

        private const float hover_duration = 500;
        private const float click_duration = 200;

        public event Action<SelectionState> StateChanged;

        private SelectionState state;

        public SelectionState State
        {
            get => state;
            set
            {
                if (state == value)
                    return;

                state = value;
                StateChanged?.Invoke(value);
            }
        }

        private readonly Container backgroundContainer;
        private readonly Container colourContainer;
        private readonly Container glowContainer;
        private readonly Box leftGlow;
        private readonly Box centerGlow;
        private readonly Box rightGlow;
        private readonly Box background;
        private readonly SpriteText spriteText;
        private Vector2 hoverSpacing => new Vector2(3f, 0f);

        public DialogButton()
            : base(HoverSampleSet.Submit)
        {
            RelativeSizeAxes = Axes.X;

            Children = new Drawable[]
            {
                backgroundContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 1f,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = backgroundColour,
                        },
                    },
                },
                glowContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 1f,
                    Alpha = 0f,
                    Children = new Drawable[]
                    {
                        leftGlow = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Origin = Anchor.TopLeft,
                            Anchor = Anchor.TopLeft,
                            Width = 0.125f,
                        },
                        centerGlow = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Width = 0.75f,
                        },
                        rightGlow = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Origin = Anchor.TopRight,
                            Anchor = Anchor.TopRight,
                            Width = 0.125f,
                        },
                    },
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        colourContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Width = idle_width,
                            Masking = true,
                            MaskingSmoothness = 2,
                            EdgeEffect = new EdgeEffectParameters
                            {
                                Type = EdgeEffectType.Shadow,
                                Colour = Colour4.Black.Opacity(0.2f),
                                Radius = 5,
                            },
                            Colour = ButtonColour,
                            Shear = new Vector2(0.2f, 0),
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    EdgeSmoothness = new Vector2(2, 0),
                                    RelativeSizeAxes = Axes.Both,
                                },
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Masking = true,
                                    MaskingSmoothness = 0,
                                    Children = new[]
                                    {
                                        new Triangles
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            TriangleScale = 4,
                                            ColourDark = OsuColour.Gray(0.88f),
                                            Shear = new Vector2(-0.2f, 0),
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                spriteText = new OsuSpriteText
                {
                    Text = Text,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = OsuFont.GetFont(size: 28, weight: FontWeight.Bold),
                    Shadow = true,
                    ShadowColour = new Colour4(0, 0, 0, 0.1f),
                    Colour = Colour4.White,
                },
            };

            updateGlow();

            StateChanged += selectionChanged;
        }

        private Colour4 buttonColour;

        public Colour4 ButtonColour
        {
            get => buttonColour;
            set
            {
                buttonColour = value;
                updateGlow();
                colourContainer.Colour = value;
            }
        }

        private Colour4 backgroundColour = OsuColour.Gray(34);

        public Colour4 BackgroundColour
        {
            get => backgroundColour;
            set
            {
                backgroundColour = value;
                background.Colour = value;
            }
        }

        private LocalisableString text;

        public LocalisableString Text
        {
            get => text;
            set
            {
                text = value;
                spriteText.Text = Text;
            }
        }

        public float TextSize
        {
            get => spriteText.Font.Size;
            set => spriteText.Font = spriteText.Font.With(size: value);
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => backgroundContainer.ReceivePositionalInputAt(screenSpacePos);

        private bool clickAnimating;

        protected override bool OnClick(ClickEvent e)
        {
            var flash = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = ButtonColour,
                Blending = BlendingParameters.Additive,
                Alpha = 0.05f
            };

            colourContainer.Add(flash);
            flash.FadeOutFromOne(100).Expire();

            clickAnimating = true;
            colourContainer.ResizeWidthTo(colourContainer.Width * 1.05f, 100, Easing.OutQuint)
                           .OnComplete(_ =>
                           {
                               clickAnimating = false;
                               StateChanged?.Invoke(State);
                           });

            return base.OnClick(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            colourContainer.ResizeWidthTo(hover_width * 0.98f, click_duration * 4, Easing.OutQuad);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            if (State == SelectionState.Selected)
                colourContainer.ResizeWidthTo(hover_width, click_duration, Easing.In);
            base.OnMouseUp(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            base.OnHover(e);
            State = SelectionState.Selected;

            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
            State = SelectionState.NotSelected;
        }

        private void selectionChanged(SelectionState newState)
        {
            if (clickAnimating)
                return;

            if (newState == SelectionState.Selected)
            {
                spriteText.TransformSpacingTo(hoverSpacing, hover_duration, Easing.OutElastic);
                colourContainer.ResizeWidthTo(hover_width, hover_duration, Easing.OutElastic);
                glowContainer.FadeIn(hover_duration, Easing.OutQuint);
            }
            else
            {
                colourContainer.ResizeWidthTo(idle_width, hover_duration, Easing.OutElastic);
                spriteText.TransformSpacingTo(Vector2.Zero, hover_duration, Easing.OutElastic);
                glowContainer.FadeOut(hover_duration, Easing.OutQuint);
            }
        }

        private void updateGlow()
        {
            leftGlow.Colour = ColourInfo.GradientHorizontal(new Colour4(ButtonColour.R, ButtonColour.G, ButtonColour.B, 0f), ButtonColour);
            centerGlow.Colour = ButtonColour;
            rightGlow.Colour = ColourInfo.GradientHorizontal(ButtonColour, new Colour4(ButtonColour.R, ButtonColour.G, ButtonColour.B, 0f));
        }
    }
}
