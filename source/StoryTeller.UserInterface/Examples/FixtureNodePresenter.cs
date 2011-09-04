using System.Collections.Generic;
using StoryTeller.Domain;
using StoryTeller.Execution;
using StoryTeller.Model;
using StoryTeller.Usages;
using StoryTeller.UserInterface.Actions;
using StoryTeller.UserInterface.Screens;

namespace StoryTeller.UserInterface.Examples
{
    public class FixtureNodePresenter : IScreen<IFixtureNode>, IListener<BinaryRecycleFinished>
    {
        private readonly IFixtureNode _subject;
        private readonly IFixtureNodeView _view;
        private UsageService _usageService;

        public FixtureNodePresenter(IFixtureNodeView view, IFixtureNode subject, UsageService usageService)
        {
            _view = view;
            _view.RefreshRequested += ViewRefreshRequested;
            _subject = subject;
            _usageService = usageService;
        }

        void ViewRefreshRequested(object sender, System.EventArgs e)
        {
            _usageService.RebuildUsages();
            UpdateView();
        }
        
        public void Handle(BinaryRecycleFinished message)
        {
        }


        public IFixtureNode Subject { get { return _subject; } }

        public object View { get { return _view; } }

        public string Title { get { return _subject.Name; } }

        public void Activate(IScreenObjectRegistry screenObjects)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _view.ShowUsage(_subject);
            _view.ShowTests(_usageService.FindUsages(_subject as ITraceableUse));
        }

        public bool CanClose()
        {
            return true;
        }

        public void Dispose()
        {
        }
        
    }
}