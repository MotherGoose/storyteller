using NUnit.Framework;
using Rhino.Mocks;
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

            var screen1 = new FixtureNodePresenter(null, fixture1, new UsageService());
            var screen2 = new FixtureNodePresenter(null, fixture2, new UsageService());

            var subject = new FixtureNodeSubject(fixture1);

            subject.Matches(screen1).ShouldBeTrue();
            subject.Matches(screen2).ShouldBeFalse();

            var randomScreen = MockRepository.GenerateMock<IScreen>();
            subject.Matches(randomScreen).ShouldBeFalse();
        }
    }

    [TestFixture]
    public class when_creating_the_screen
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            container = new Container(x =>
            {
                x.For<IFixtureNodeView>().Use<FixtureNodeView>();
                x.For<IScreen<IFixtureNode>>().Use<FixtureNodePresenter>();
            });

            factory = new ScreenFactory(container);

            fixture = new FixtureGraph("fixture1");

            subject = new FixtureNodeSubject(fixture);

            thePresenter = subject.CreateScreen(factory).ShouldBeOfType<FixtureNodePresenter>();
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
    }
}