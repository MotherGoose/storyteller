using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryTeller.Domain;
using StoryTeller.Model;
using StoryTeller.Usages;
using StoryTeller.Workspace;

namespace StoryTeller.UserInterface
{
    public class UsageService : IListener<ProjectLoaded>
                                        , IListener<ReloadTestsMessage>
    {
        private ProjectContext _context;
        private UsageGraph _usages;
        private static readonly object _lock = new object();

        public UsageService()
        {
            
        }

        public UsageService(ProjectContext context)
        {
            _context = context;
            _usages = new UsageGraph(context.Library, new ConsoleUsageGraphListener());
        }

        public void Handle(ProjectLoaded message)
        {
            AsyncRebuild();
        }

        public void RebuildUsages()
        {
            Rebuild();
        }

        private void AsyncRebuild()
        {
            Task.Factory.StartNew(Rebuild);
        }

        private void Rebuild()
        {
            lock(_lock)
            {
                _usages.Rebuild(_context.Hierarchy);
            }
        }

        public IEnumerable<Test> FindUsages(IFixtureNode fixtureNode)
        {
            if (fixtureNode == null)
            {
                return new List<Test>();
            }
            lock (_usages)
            {
                if (_usages.AllFixtures().Count() == 0)
                {
                    Rebuild();
                }
                return fixtureNode.FindUsages(_usages);
            }
        }
        
        public void Handle(ReloadTestsMessage message)
        {
            AsyncRebuild();
        }

    }
}