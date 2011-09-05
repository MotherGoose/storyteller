using System;
using System.Collections.Generic;
using StoryTeller.Domain;
using StoryTeller.Execution;
using StoryTeller.Model;

namespace StoryTeller.UserInterface.Examples
{
    public interface IFixtureNodeView
    {
        void ShowUsageDescription(string usageCaption, string usageDescription);
        void ShowTests(IEnumerable<Test> tests);

        event EventHandler RefreshRequested;
    }
}