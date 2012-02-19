﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.8.1.0
//      SpecFlow Generator Version:1.8.0.0
//      Runtime Version:4.0.30319.239
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace GeniusCode.XtraReports.Runtime.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.8.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Subreport Inside of a Detail Report Band is passed the correct Datasource")]
    public partial class SubreportInsideOfADetailReportBandIsPassedTheCorrectDatasourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SubreportInsideOfDetailReportBandIsPassedCorrectDatasource.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Subreport Inside of a Detail Report Band is passed the correct Datasource", "In order to easily build reports without writing any code\r\nAs a report designer\r\n" +
                    "I want my sub reports to share datasources automatically, even in detail report " +
                    "bands", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Subreport inside header of detail report band")]
        [NUnit.Framework.CategoryAttribute("mytag")]
        public virtual void SubreportInsideHeaderOfDetailReportBand()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Subreport inside header of detail report band", new string[] {
                        "mytag"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("A parent report exists");
#line 9
 testRunner.And("the parent report has a datasource of three items");
#line 10
 testRunner.And("a subreport exists as a file");
#line 11
 testRunner.And("the parent report has a detail report band with a datamember of dogs");
#line 12
 testRunner.And("the detail report band contains a subreport in its header band");
#line 13
 testRunner.And("the XRSubreport container references the subreport\'s filename");
#line 14
 testRunner.And("the xtrasubreport engine is initialized");
#line 15
 testRunner.When("the report engine runs");
#line 16
 testRunner.Then("the subreport should have the same datasource as the containing group\'s datasourc" +
                    "e collection");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Subreport inside footer of detail report band")]
        public virtual void SubreportInsideFooterOfDetailReportBand()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Subreport inside footer of detail report band", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
 testRunner.Given("A parent report exists");
#line 21
 testRunner.And("the parent report has a datasource of three items");
#line 22
 testRunner.And("a subreport exists as a file");
#line 23
 testRunner.And("the parent report has a detail report band with a datamember of dogs");
#line 24
 testRunner.And("the detail report band contains a subreport in its footer band");
#line 25
 testRunner.And("the XRSubreport container references the subreport\'s filename");
#line 26
 testRunner.And("the xtrasubreport engine is initialized");
#line 27
 testRunner.When("the report engine runs");
#line 28
 testRunner.Then("the subreport should have the same datasource as the containing group\'s datasourc" +
                    "e collection");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Subreport inside detail of detail report band")]
        public virtual void SubreportInsideDetailOfDetailReportBand()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Subreport inside detail of detail report band", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
 testRunner.Given("A parent report exists");
#line 32
 testRunner.And("the parent report has a datasource of three items");
#line 33
 testRunner.And("a subreport exists as a file");
#line 34
 testRunner.And("the parent report has a detail report band with a datamember of dogs");
#line 35
 testRunner.And("the detail report band contains a subreport in its detail band");
#line 36
 testRunner.And("the XRSubreport container references the subreport\'s filename");
#line 37
 testRunner.And("the xtrasubreport engine is initialized");
#line 38
 testRunner.When("the report engine runs");
#line 39
 testRunner.Then("each subreport should have a datasource containing a single item");
#line 40
 testRunner.And("each subreport datasource contains the same datasource as the containing group\'s " +
                    "detail band");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
