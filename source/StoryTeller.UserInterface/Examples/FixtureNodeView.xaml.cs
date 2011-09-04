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
        public FixtureNodeView()
        {
            InitializeComponent();
        }

        public void ShowUsage(IFixtureNode usage)
        {
            description.Content = usage.Label;
        }

        public void ShowTests(IEnumerable<Test> tests)
        {
            Tests.Children.Clear();
            tests.Each(x =>
            {
                var link = new Link
                {
                    ToolTip = "{0} ({1})".ToFormat(x.Name, x.ToString()),
                    Padding = new Thickness(0, 0, 0, 5)
                };

                link.WireUp(x.Name, () => new OpenItemMessage(x));
                Tests.Children.Add(link);
            });
            
        }
    }
}