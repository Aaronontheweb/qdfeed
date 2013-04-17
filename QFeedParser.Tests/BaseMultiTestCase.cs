using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace QDFeedParser.Tests
{
    public class BaseMultiTestCase
    {
        public BaseMultiTestCase(IEnumerable<TestCaseData> testcases)
        {
            this.TestCaseList = new List<TestCaseData>(testcases);
        }

        //Our list of test cases
        protected List<TestCaseData> TestCaseList;

        public IEnumerable TestCases
        {
            get
            {
                foreach (var testcase in this.TestCaseList)
                {
                    yield return testcase;
                }
            }
        }
    }
}
