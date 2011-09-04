using System;
using System.Collections.Generic;
using StoryTeller.Domain;
using StoryTeller.Execution;
using StoryTeller.Model;

namespace StoryTeller.UserInterface.Examples
{
    public interface IFixtureNodeView
    {
        void ShowUsage(IFixtureNode usage);
        void ShowTests(IEnumerable<Test> tests);

        event EventHandler RefreshRequested;
    }
}