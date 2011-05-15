using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;
using HtmlTags;
using NUnit.Framework;
using StoryTeller.Domain;
using StoryTeller.Engine;
using StoryTeller.Model;
using StoryTeller.UserInterface.Editing;
using StoryTeller.UserInterface.Editing.Scripts;
using StoryTeller.Workspace;

namespace StoryTeller.Testing.UserInterface.Editing
{
    [TestFixture]
    public class TestEditorBuilderTester
    {

        private const string TEST_EDITOR_FILENAME = "testEditor.js";
        private const string JQUERY_METADATA_FILENAME = "jquery.metadata.js";

        [Test]
        public void Jquery_metadata_file_is_before_testeditor_file()
        {
            TestEditorBuilder builder = new TestEditorBuilder();
            IProject project = DataMother.GrammarProject();
            Test test = project.LoadTests()
                .GetAllTests().FirstOrDefault(x => x.Name == "Sentences");
            FixtureLibrary library = new LibraryBuilder(new NulloFixtureObserver(), new CompositeFilter<Type>()).Library;
            HtmlDocument document = builder.BuildTestEditor(test, library);

            IEnumerable<JavascriptFile> files = Embeds.GetFiles();
            string contentOfTheFirstFile = GetFileContents(files, JQUERY_METADATA_FILENAME);
            string contentOfTheSecondFile = GetFileContents(files, TEST_EDITOR_FILENAME);

            Assert.Less(document.ToString().IndexOf(contentOfTheFirstFile),
                document.ToString().IndexOf(contentOfTheSecondFile));
        }

        private static string GetFileContents(IEnumerable<JavascriptFile> files, string fileName)
        {
            return files.Where(file => file.FileName == fileName).Select(file => file.Contents()).FirstOrDefault();
        }
    }
}