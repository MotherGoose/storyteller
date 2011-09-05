using System;
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
            string caption = string.Format("Tests using {0}:", _subject.GetType().Name);
            string description = _subject.Label;

            _view.ShowUsageDescription(caption, description);
            _view.ShowTests(_usageService.FindUsages(_subject));
        }
    }
}