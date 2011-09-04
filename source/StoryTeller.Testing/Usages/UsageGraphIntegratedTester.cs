using System.Collections.Generic;
using NUnit.Framework;
using StoryTeller.Domain;
using StoryTeller.Execution;
using StoryTeller.Usages;
using System.Linq;

namespace StoryTeller.Testing.Usages
{
    [TestFixture]
    public class UsageGraphIntegratedTester
    {
        private ProjectTestRunner runner;
        private UsageGraph usages;

        [SetUp]
        public void SetUp()
        {
            runner = new ProjectTestRunner(DataMother.THE_GRAMMAR_FILE);
            usages = new UsageGraph(runner.GetLibary(), new ConsoleUsageGraphListener());
            usages.Rebuild(runner.Hierarchy);
        }

        [Test]
        public void find_fixture_usage_for_a_workspace()
        {
            var enumerable = usages.FixturesFor("Tables").Select(x => x.Name).ToList();
            enumerable.Sort();
            enumerable.ShouldHaveTheSameElementsAs("DataTable", "Table");
        }


        [Test]
        public void find_fixture_usage_for_a_workspace_including_paragraphs()
        {
            usages.FixturesFor("Paragraphs").Select(x => x.Name).ShouldHaveTheSameElementsAs("Composite");
        }

        [Test]
        public void find_fixture_usage_for_a_workspace_including_embedded_sections()
        {
            usages.FixturesFor("Embedded").Select(x => x.Name).ShouldHaveTheSameElementsAs("Embedded", "Math");
        }

        [Test]
        public void find_test_for_fixture()
        {
            var tests = usages.TestsFor("Composite");
            tests.Select(x => x.Name).ShouldHaveTheSameElementsAs("Composite with Errors", "Simple Composite");
        }

        [Test]
        public void find_no_matching_tests_for_fixture_search()
        {
            var tests = usages.TestsFor("abc");
            Assert.AreEqual(0, tests.Count());
        }

        [Test]
        public void find_test_for_grammar()
        {
            var tests = usages.TestsFor("Composite", "AddAndMultiplyThrow");
            tests.Select(x => x.Name).ShouldHaveTheSameElementsAs("Composite with Errors");
        }

        [Test]
        public void find_multiple_tests_for_grammar()
        {
            var tests = usages.TestsFor("Composite", "AddAndMultiply");
            tests.Select(x => x.Name).ShouldContain("Composite with Errors");
            tests.Select(x => x.Name).ShouldContain("Simple Composite");
        }

        [Test]
        public void does_not_find_test_for_a_grammar_used_inside_paragraph()
        {
            var tests = usages.TestsFor("Math", "StartWith");
            Assert.AreEqual(0, tests.Count());
        }

    }
}