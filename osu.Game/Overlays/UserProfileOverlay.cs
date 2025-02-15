﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Containers;
using osu.Game.Online.API.Requests;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Overlays.Profile;
using osu.Game.Overlays.Profile.Sections;
using osu.Game.Users;

namespace osu.Game.Overlays
{
    public class UserProfileOverlay : FullscreenOverlay<ProfileHeader>
    {
        private ProfileSection lastSection;
        private ProfileSection[] sections;
        private GetUserRequest userReq;
        private ProfileSectionsContainer sectionsContainer;
        private ProfileSectionTabControl tabs;

        public const float CONTENT_X_MARGIN = 70;

        public UserProfileOverlay()
            : base(OverlayColourScheme.Pink)
        {
        }

        protected override ProfileHeader CreateHeader() => new ProfileHeader();

        protected override Colour4 BackgroundColour => ColourProvider.Background6;

        public void ShowUser(IUser user)
        {
            if (user == APIUser.SYSTEM_USER)
                return;

            Show();

            if (user.OnlineID == Header?.User.Value?.Id)
                return;

            if (sectionsContainer != null)
                sectionsContainer.ExpandableHeader = null;

            userReq?.Cancel();
            Clear();
            lastSection = null;

            sections = !user.IsBot
                ? new ProfileSection[]
                {
                    //new AboutSection(),
                    new RecentSection(),
                    new RanksSection(),
                    //new MedalsSection(),
                    new HistoricalSection(),
                    new BeatmapsSection(),
                    new KudosuSection()
                }
                : Array.Empty<ProfileSection>();

            tabs = new ProfileSectionTabControl
            {
                RelativeSizeAxes = Axes.X,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
            };

            Add(sectionsContainer = new ProfileSectionsContainer
            {
                ExpandableHeader = Header,
                FixedHeader = tabs,
                HeaderBackground = new Box
                {
                    // this is only visible as the ProfileTabControl background
                    Colour = ColourProvider.Background5,
                    RelativeSizeAxes = Axes.Both
                },
            });
            sectionsContainer.SelectedSection.ValueChanged += section =>
            {
                if (lastSection != section.NewValue)
                {
                    lastSection = section.NewValue;
                    tabs.Current.Value = lastSection;
                }
            };

            tabs.Current.ValueChanged += section =>
            {
                if (lastSection == null)
                {
                    lastSection = sectionsContainer.Children.FirstOrDefault();
                    if (lastSection != null)
                        tabs.Current.Value = lastSection;
                    return;
                }

                if (lastSection != section.NewValue)
                {
                    lastSection = section.NewValue;
                    sectionsContainer.ScrollTo(lastSection);
                }
            };

            sectionsContainer.ScrollToTop();

            // Check arbitrarily whether this user has already been populated.
            // This is only generally used by tests, but should be quite safe unless we want to force a refresh on loading a previous user in the future.
            if (user is APIUser apiUser && apiUser.JoinDate != default)
            {
                userReq = null;
                userLoadComplete(apiUser);
                return;
            }

            userReq = user.OnlineID > 1 ? new GetUserRequest(user.OnlineID) : new GetUserRequest(user.Username);
            userReq.Success += userLoadComplete;
            API.Queue(userReq);
        }

        private void userLoadComplete(APIUser user)
        {
            Header.User.Value = user;

            if (user.ProfileOrder != null)
            {
                foreach (string id in user.ProfileOrder)
                {
                    var sec = sections.FirstOrDefault(s => s.Identifier == id);

                    if (sec != null)
                    {
                        sec.User.Value = user;

                        sectionsContainer.Add(sec);
                        tabs.AddItem(sec);
                    }
                }
            }
        }

        private class ProfileSectionTabControl : OverlayTabControl<ProfileSection>
        {
            private const float bar_height = 2;

            public ProfileSectionTabControl()
            {
                TabContainer.RelativeSizeAxes &= ~Axes.X;
                TabContainer.AutoSizeAxes |= Axes.X;
                TabContainer.Anchor |= Anchor.x1;
                TabContainer.Origin |= Anchor.x1;

                Height = 36 + bar_height;
                BarHeight = bar_height;
            }

            protected override TabItem<ProfileSection> CreateTabItem(ProfileSection value) => new ProfileSectionTabItem(value)
            {
                AccentColour = AccentColour,
            };

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                AccentColour = colourProvider.Highlight1;
            }

            protected override bool OnClick(ClickEvent e) => true;

            protected override bool OnHover(HoverEvent e) => true;

            private class ProfileSectionTabItem : OverlayTabItem
            {
                public ProfileSectionTabItem(ProfileSection value)
                    : base(value)
                {
                    Text.Text = value.Title;
                    Text.Font = Text.Font.With(size: 16);
                    Text.Margin = new MarginPadding { Bottom = 10 + bar_height };
                    Bar.ExpandedSize = 10;
                    Bar.Margin = new MarginPadding { Bottom = bar_height };
                }
            }
        }

        private class ProfileSectionsContainer : SectionsContainer<ProfileSection>
        {
            public ProfileSectionsContainer()
            {
                RelativeSizeAxes = Axes.Both;
            }

            protected override UserTrackingScrollContainer CreateScrollContainer() => new OverlayScrollContainer();

            protected override FlowContainer<ProfileSection> CreateScrollContentContainer() => new FillFlowContainer<ProfileSection>
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Spacing = new Vector2(0, 20),
            };
        }
    }
}
