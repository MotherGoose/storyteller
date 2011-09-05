using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FubuCore;
using StoryTeller.Domain;
using StoryTeller.Execution;
using StoryTeller.Model;
using StoryTeller.UserInterface.Controls;

namespace StoryTeller.UserInterface.Examples
{
    /// <summary>
    /// Interaction logic for FixtureNodeView.xaml
    /// </summary>
    public partial class FixtureNodeView : UserControl, IFixtureNodeView
    {
        public event EventHandler RefreshRequested;

        public FixtureNodeView()
        {
            InitializeComponent();

            refresh.Click += (x, y) => Refresh();
        }

        private void Refresh()
        {
            if (RefreshRequested != null)
            {
                RefreshRequested(this, EventArgs.Empty);
            }
        }

        public void ShowUsageDescription(string usageCaption, string usageDescription)
        {
            caption.Content = usageCaption;
            description.Content = usageDescription;
        }

        public void ShowTests(IEnumerable<Test> tests, Func<Test, string> linkText)
        {
            Tests.Children.Clear();
            tests.Each(x =>
            {
                var link = new Link
                {
                    Padding = new Thickness(0, 0, 0, 5)
                };

                link.WireUp(linkText(x), () => new OpenItemMessage(x));
                Tests.Children.Add(link);
            });
            
        }
    }
}