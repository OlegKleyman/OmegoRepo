#region Designer generated code

using TechTalk.SpecFlow;

#pragma warning disable

namespace Oleg.Kleyman.Utorrent.Core.Tests.Integration
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.1.84")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Utorrent")]
    public partial class UtorrentFeature
    {
        private static TechTalk.SpecFlow.ITestRunner testRunner;

#line 1 "Utorrent.feature"
#line hidden

        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            var featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"),
                                                                "Utorrent",
                                                                "As a developer I need to be able to\r\nget information from the utorrent web API",
                                                                ProgrammingLanguage.CSharp, ((string[]) (null)));
            testRunner.OnFeatureStart(featureInfo);
        }

        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }

        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }

        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }

        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }

        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get utorrent key")]
        public virtual void GetUtorrentKey()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get utorrent key", ((string[]) (null)));
#line 5
            this.ScenarioSetup(scenarioInfo);
#line 6
            testRunner.When("I call the method GetKey", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 7
            testRunner.Then("It should result in returning the key to use for this utorrent session", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get a torrent")]
        public virtual void GetATorrent()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get a torrent", ((string[]) (null)));
#line 9
            this.ScenarioSetup(scenarioInfo);
#line 10
            testRunner.Given("I have attained an API key", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)),
                             "Given ");
#line 11
            testRunner.When("I call the method GetTorrentFile with a hash of \"FB4F76083F21CC6AA6A2E2EB210D126C" +
                            "3CC090DC\"", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 12
            testRunner.Then("It should return a torrent with build number \"27498\"", ((string) (null)),
                            ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 13
            testRunner.And("the torrent should have a hash value of \"FB4F76083F21CC6AA6A2E2EB210D126C3CC090DC" +
                           "\"", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "And ");
#line 14
            testRunner.And("the torrent should have a count of \"2\" files", ((string) (null)),
                           ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            var table1 = new TechTalk.SpecFlow.Table(new string[]
                {
                    "Name"
                });
            table1.AddRow(new string[]
                {
                    "daa-alvh-1080p.mkv"
                });
            table1.AddRow(new string[]
                {
                    "daa-alvh-1080p.nfo"
                });
#line 15
            testRunner.And("the file names should be", ((string) (null)), table1, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}

#pragma warning restore

#endregion