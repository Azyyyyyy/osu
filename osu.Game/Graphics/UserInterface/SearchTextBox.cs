﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using Silk.NET.Input;

namespace osu.Game.Graphics.UserInterface
{
    public class SearchTextBox : FocusedTextBox
    {
        protected virtual bool AllowCommit => false;

        public SearchTextBox()
        {
            Height = 35;
            Add(new SpriteIcon
            {
                Icon = FontAwesome.Solid.Search,
                Origin = Anchor.CentreRight,
                Anchor = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 10 },
                Size = new Vector2(20),
            });

            TextFlow.Padding = new MarginPadding { Right = 35 };
            PlaceholderText = "type to search";
        }

        public override bool OnPressed(KeyBindingPressEvent<PlatformAction> e)
        {
            switch (e.Action)
            {
                case PlatformAction.MoveBackwardLine:
                case PlatformAction.MoveForwardLine:
                // Shift+delete is handled via PlatformAction on macOS. this is not so useful in the context of a SearchTextBox
                // as we do not allow arrow key navigation in the first place (ie. the caret should always be at the end of text)
                // Avoid handling it here to allow other components to potentially consume the shortcut.
                case PlatformAction.DeleteForwardChar:
                    return false;
            }

            return base.OnPressed(e);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.ControlPressed && !e.ShiftPressed)
            {
                switch (e.Key)
                {
                    case Key.Left:
                    case Key.Right:
                    case Key.Up:
                    case Key.Down:
                        return false;
                }
            }

            if (!AllowCommit)
            {
                switch (e.Key)
                {
                    case Key.KeypadEnter:
                    case Key.Enter:
                        return false;
                }
            }

            if (e.ShiftPressed)
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        return false;
                }
            }

            return base.OnKeyDown(e);
        }
    }
}
