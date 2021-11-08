// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics;

namespace osu.Game.Overlays.Dashboard.Friends
{
    public class FriendsOnlineStatusItem : OverlayStreamItem<FriendStream>
    {
        public FriendsOnlineStatusItem(FriendStream value)
            : base(value)
        {
        }

        protected override LocalisableString MainText => Value.Status.GetLocalisableDescription();

        protected override LocalisableString AdditionalText => Value.Count.ToString();

        protected override Colour4 GetBarColour(OsuColour colours)
        {
            switch (Value.Status)
            {
                case OnlineStatus.All:
                    return Colour4.White;

                case OnlineStatus.Online:
                    return colours.GreenLight;

                case OnlineStatus.Offline:
                    return Colour4.Black;

                default:
                    throw new ArgumentException($@"{Value.Status} status does not provide a colour in {nameof(GetBarColour)}.");
            }
        }
    }
}
