using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using StoryTeller.Domain;
using StoryTeller.Model;
using StoryTeller.UserInterface;
using StoryTeller.UserInterface.Examples;
using StoryTeller.UserInterface.Screens;
using StructureMap;

namespace StoryTeller.Testing.UserInterface.Examples
{
    [TestFixture]
    public class FixtureNodeSubjectIntegratedTester
    {
        [Test]
        public void matches_by_fixture_node()
        {
            var fixture1 = new FixtureGraph("fixture1");
            var fixture2 = new FixtureGraph("fixture2");

            var screen1 = new FixtureNodePresenter(new StubFixtureNodeView(), fixture1, new UsageService());
            var screen2 = new FixtureNodePresenter(new StubFixtureNodeView(), fixture2, new UsageService());

            var subject = new FixtureNodeSubject(fixture1);

            subject.Matches(screen1).ShouldBeTrue();
            subject.Matches(screen2).ShouldBeFalse();

            var randomScreen = MockRepository.GenerateMock<IScreen>();
            subject.Matches(randomScreen).ShouldBeFalse();
        }
    }

    public class StubFixtureNodeView : IFixtureNodeView
    {
        public string Description { get; set; }

        public string Caption { get; set; }

        public IEnumerable<Test> Tests { get; set; }

        public void ShowUsageDescription(string usageCaption, string usageDescription)
        {
            Description = usageDescription;
            Caption = usageCaption;
        }

        public void ShowTests(IEnumerable<Test> tests)
        {
            Tests = tests;
        }

        public void FireRefreshRequest()
        {
            if (RefreshRequested != null)
            {
                RefreshRequested(this, EventArgs.Empty);
            }
        }

        public event EventHandler RefreshRequested;
    }

    [TestFixture]
    public class when_creating_the_screen
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            ProjectContext context = new ProjectContext();
            context.Hierarchy = DataMother.GrammarProject().LoadTests();
            context.Library = DataMother.GrammarsProjectRunner().GetLibary();
            UsageService service = new UsageService(context);

            container = new Container(x =>
            {
                x.For<IFixtureNodeView>().Use<StubFixtureNodeView>();
                x.For<IScreen<IFixtureNode>>().Use<FixtureNodePresenter>();
                x.For<UsageService>().Use(service);
            });

            factory = new ScreenFactory(container);

            fixture = context.Library.FixtureFor("Composite");

            subject = new FixtureNodeSubject(fixture);

            thePresenter = subject.CreateScreen(factory).ShouldBeOfType<FixtureNodePresenter>();
            thePresenter.Activate(null);
        }

        #endregion

        private Container container;
        private FixtureGraph fixture;
        private FixtureNodeSubject subject;
        private FixtureNodePresenter thePresenter;
        private ScreenFactory factory;

        [Test]
        public void the_presenter_has_the_fixture_as_the_subject()
        {
            thePresenter.Subject.ShouldBeTheSameAs(fixture);
        }

        [Test]
        public void the_presenter_has_the_fixture_name_as_the_title()
        {
            thePresenter.Title.ShouldBeTheSameAs(fixture.Name);
        }

        [Test]
        public void the_view_is_showing_the_subject()
        {
            ((StubFixtureNodeView) thePresenter.View).Description.ShouldBeTheSameAs(subject.Subject.Label);
        }

        [Test]
        public void the_view_is_showing_the_tests_where_subject_is_used()
        {
            IEnumerable<Test> tests = ((StubFixtureNodeView) thePresenter.View).Tests;
            tests.Select(x => x.Name).ShouldHaveTheSameElementsAs("Composite with Errors", "Simple Composite");
        }
    }
}