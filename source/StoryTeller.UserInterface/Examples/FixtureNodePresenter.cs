using System;
using System.Collections.Generic;
using StoryTeller.Domain;
using StoryTeller.Execution;
using StoryTeller.Model;
using StoryTeller.UserInterface.Actions;
using StoryTeller.UserInterface.Screens;

namespace StoryTeller.UserInterface.Examples
{
    public class FixtureNodePresenter : IScreen<IFixtureNode>, IListener<BinaryRecycleFinished>
    {
        private readonly IFixtureNode _subject;
        private readonly UsageService _usageService;
        private readonly IFixtureNodeView _view;

        public FixtureNodePresenter(IFixtureNodeView view, IFixtureNode subject, UsageService usageService)
        {
            _view = view;
            _view.RefreshRequested += ViewRefreshRequested;
            _subject = subject;
            _usageService = usageService;
        }

        public void Handle(BinaryRecycleFinished message)
        {
        }


        public IFixtureNode Subject
        {
            get { return _subject; }
        }

        public object View
        {
            get { return _view; }
        }

        public string Title
        {
            get { return _subject.Name; }
        }

        public void Activate(IScreenObjectRegistry screenObjects)
        {
            UpdateView();
        }

        public bool CanClose()
        {
            return true;
        }

        public void Dispose()
        {
        }

        private void ViewRefreshRequested(object sender, EventArgs e)
        {
            _usageService.RebuildUsages();
            UpdateView();
        }

        private void UpdateView()
        {
            List<Test> tests = new List<Test>(_usageService.FindUsages(_subject));
            tests.Sort(new TestSorter());

            string caption = string.Format("{0} tests using {1}:", tests.Count, _subject.GetType().Name);
            string description = _subject.Label;

            _view.ShowUsageDescription(caption, description);

            _view.ShowTests(tests, GetLinkText);
        }

        private static string GetLinkText(Test test)
        {
            return test.LocatorPath().Replace(@"/", " / ");
        }
    }

    internal class TestSorter : IComparer<Test>
    {
        public int Compare(Test x, Test y)
        {
            return x.LocatorPath().CompareTo(y.LocatorPath());
        }
    }

}