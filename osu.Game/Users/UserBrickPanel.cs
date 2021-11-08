﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Colour4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Online.API.Requests.Responses;

namespace osu.Game.Users
{
    public class UserBrickPanel : UserPanel
    {
        public UserBrickPanel(APIUser user)
            : base(user)
        {
            AutoSizeAxes = Axes.Both;
            CornerRadius = 6;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Background.FadeTo(0.2f);
        }

        protected override Drawable CreateLayout() => new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(5, 0),
            Margin = new MarginPadding
            {
                Horizontal = 10,
                Vertical = 3,
            },
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Children = new Drawable[]
            {
                new CircularContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Masking = true,
                    Width = 4,
                    Height = 13,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = string.IsNullOrEmpty(User.Colour) ? Colour4Extensions.FromHex("0087ca") : Colour4Extensions.FromHex(User.Colour)
                    }
                },
                CreateUsername().With(u =>
                {
                    u.Anchor = Anchor.CentreLeft;
                    u.Origin = Anchor.CentreLeft;
                    u.Font = OsuFont.GetFont(size: 13, weight: FontWeight.Bold);
                })
            }
        };
    }
}
