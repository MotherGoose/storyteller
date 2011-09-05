using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using StoryTeller.Domain;
using StoryTeller.UserInterface;

namespace StoryTeller.Testing.UserInterface
{
    [TestFixture]
    public class UsageServiceTester
    {
        #region Setup/Teardown

        [TestFixtureSetUp]
        public void SetUp()
        {
            _context = new ProjectContext();
            _context.Hierarchy = DataMother.GrammarProject().LoadTests();
            _context.Library = DataMother.GrammarsProjectRunner().GetLibary();
            _service = new UsageService(_context);
        }

        #endregion

        private UsageService _service;
        private ProjectContext _context;

        [Test]
        public void finding_a_usage_of_a_fixture()
        {
            IEnumerable<Test> tests = _service.FindUsages(_context.Library.FixtureFor("Composite"));
            tests.Select(x => x.Name).ShouldHaveTheSameElementsAs("Composite with Errors", "Simple Composite");
        }

        [Test]
        public void finding_a_usage_of_a_grammar()
        {
            IEnumerable<Test> tests =
                _service.FindUsages(_context.Library.FixtureFor("Composite").GrammarFor("AddAndMultiplyThrow"));
            tests.Select(x => x.Name).ShouldHaveTheSameElementsAs("Composite with Errors");
        }

        [Test]
        public void finding_a_usage_of_a_unsupported_object_returns_empty_list()
        {
            IEnumerable<Test> tests = _service.FindUsages(null);
            Assert.AreEqual(0, tests.Count());
        }

        [Test]
        public void finding_a_usage_of_a_unused_grammar_returns_empty_list()
        {
            IEnumerable<Test> tests =
                _service.FindUsages(_context.Library.FixtureFor("DecoratedGrammar").GrammarFor("Go"));
            Assert.AreEqual(0, tests.Count());
        }
        
    }
}