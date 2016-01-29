using NUnit.Framework;
using System.Collections;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System;

namespace NUnit3Tests
{
    [TestFixture]
    public class TestClass
    {
        private static IEnumerable TestData
        {
            get { return GetTestData(); }
        }

        private static string GetTestDataFolder(string testDataFolder)
        {

            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = string.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 3));
            return Path.Combine(projectPath, testDataFolder);
        }

        private static IEnumerable GetTestData()
        {
            var doc = XDocument.Load(GetTestDataFolder("TestData") + @"\TestData.xml");
            return
                from data in doc.Descendants("data")
                let firstNumber = data.Element("firstNumber").Value
                let secondNumber = data.Element("secondNumber").Value
                let result = data.Element("result").Value
                select new object[] { firstNumber, secondNumber, result };
        }

        [Test, TestCaseSource("TestData")]
        public void AdditionTest(string firstNumber, string secondNumber, string result)
        {
            string exeSourceFile = @"C:\Windows\System32\calc.exe";
            Application application = Application.Launch(exeSourceFile);
            Window window = application.GetWindow("Calculator");
            window.WaitWhileBusy();
            window.Get<Button>(SearchCriteria.ByText(firstNumber)).Click();
            window.Get<Button>(SearchCriteria.ByAutomationId("93")).Click();
            window.Get<Button>(SearchCriteria.ByText(secondNumber)).Click();
            window.Get<Button>(SearchCriteria.ByAutomationId("121")).Click();
            window.WaitWhileBusy();
            SearchCriteria searchCriteria = SearchCriteria.ByAutomationId("158");
            Label display = window.Get<Label>(searchCriteria);
            Assert.AreEqual(display.Text, result, "Bla bla message");
            application.Close();
        }
    }
}
