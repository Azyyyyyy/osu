﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Game.Users;

namespace osu.Game.Overlays.Profile.Header.Components
{
    [LongRunningLoad]
    public class DrawableBadge : CompositeDrawable, IHasTooltip
    {
        public static readonly Vector2 DRAWABLE_BADGE_SIZE = new Vector2(86, 40);

        private readonly Badge badge;

        public DrawableBadge(Badge badge)
        {
            this.badge = badge;
            Size = DRAWABLE_BADGE_SIZE;
        }

        [BackgroundDependencyLoader]
        private void load(LargeTextureStore textures)
        {
            InternalChild = new Sprite
            {
                FillMode = FillMode.Fit,
                RelativeSizeAxes = Axes.Both,
                Texture = textures.Get(badge.ImageUrl),
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            InternalChild.FadeInFromZero(200);
        }

        public LocalisableString TooltipText => badge.Description;
    }
}
