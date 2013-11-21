using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace QDFeedParser.Tests
{
    /// <summary>
    /// Used for dynamically loading all of the test files out of their directories within the QFeedParser.Tests solution.
    /// </summary>
    public static class TestFileLoader
    {
        public enum TestFileType
        {
            Http,
            FileSys
        } ;

        /* Yeah these directories are hard-coded - that way NUnit can run out of the box
         * without people having to dick around with .config files 
         * See: http://thedailywtf.com/Articles/Soft_Coding.aspx */
        #region Valid test case file path constants
        private const string ValidFileSysAtomTestDirPath = @"..\..\Test Files\Valid\FileSys\Atom\";
        private const string ValidFileSysRssTestDirPath = @"..\..\Test Files\Valid\FileSys\Rss\";
        private const string ValidHttpRssTestFilePath = @"..\..\Test Files\Valid\Http\RSS.xml";
        private const string ValidHttpAtomTestFilePath = @"..\..\Test Files\Valid\Http\Atom.xml";
        #endregion 

        public const string TestFileSearchPattern = @"*.xml";

        #region Valid test case path accessors
        public static string ValidFileSysAtomTestDir{
            get { return Path.GetFullPath(ValidFileSysAtomTestDirPath); }
        }

        public static string ValidFileSysRssTestDir
        {
            get { return Path.GetFullPath(ValidFileSysRssTestDirPath); }
        }

        public static string ValidHttpAtomTestFile
        {
            get { return Path.GetFullPath(ValidHttpAtomTestFilePath); }
        }

        public static string ValidHttpRssTestFile
        {
            get { return Path.GetFullPath(ValidHttpRssTestFilePath); }
        }
        #endregion

        private const string MissingFileSysTestFilePath = @"..\..\Test Files\Missing\FileSys\MissingFiles.xml";
        private const string MissingHttpFilePath = @"..\..\Test Files\Missing\Http\MissingURLs.xml";

        public static string MissingFileSysTestCases
        {
            get { return Path.GetFullPath(MissingFileSysTestFilePath); }
        }

        public static string MissingHttpTestCases
        {
            get { return Path.GetFullPath(MissingHttpFilePath); }
        }

        public static string GetSingleRssTestFilePath(TestFileType fileType)
        {
            return fileType == TestFileType.FileSys ? GetTestFilePathsFromDirectory(ValidFileSysRssTestDir).First().FullName : GetTestCasePathsFromXml(ValidHttpRssTestFile).First().Value;
        }

        public static string GetSingleAtomTestFilePath(TestFileType fileType)
        {
            return fileType == TestFileType.FileSys ? GetTestFilePathsFromDirectory(ValidFileSysAtomTestDir).First().FullName : GetTestCasePathsFromXml(ValidHttpAtomTestFile).First().Value;
        }

        /// <summary>
        /// Returns a list of TestCaseData for all test files.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns>An IList of TestCaseData objects which contain the full path of each file.</returns>
        public static IList<TestCaseData> LoadAllValidTestCases(TestFileType fileType)
        {
            var atomCases = LoadValidAtomTestCases(fileType);
            var rssCases = LoadValidRssTestCases(fileType);
            foreach(var testcase in atomCases)
            {
                rssCases.Add(testcase);
            }
            return rssCases;
        }

        /// <summary>
        /// Returns a list of TestCaseData for Atom test files only.
        /// </summary>
        /// <returns>An IList of TestCaseData objects which contain the full path of each file.</returns>
        public static IList<TestCaseData> LoadValidAtomTestCases(TestFileType fileType)
        {
            if(fileType == TestFileType.FileSys)
                return LoadTestCaseFilesFromDirectory(TestFileLoader.ValidFileSysAtomTestDir);
            return LoadTestCaseFilesFromXml(TestFileLoader.ValidHttpAtomTestFile);
        }

        /// <summary>
        /// Returns a list of TestCaseData for RSS test files only.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns>An IList of TestCaseData objects which contain the full path of each file.</returns>
        public static IList<TestCaseData> LoadValidRssTestCases(TestFileType fileType)
        {
            if (fileType == TestFileType.FileSys)
                return LoadTestCaseFilesFromDirectory(TestFileLoader.ValidFileSysRssTestDir);
            return LoadTestCaseFilesFromXml(TestFileLoader.ValidHttpRssTestFile);
        }

        /// <summary>
        /// Returns a list of TestCaseData for URIs which are known to not exist.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns>An IList of TestCaseData objects which contain the full URI of each test.</returns>
        public static IList<TestCaseData> LoadMissingTestCases(TestFileType fileType)
        {
            if (fileType == TestFileType.FileSys)
                return LoadTestCaseFilesFromXml(TestFileLoader.MissingFileSysTestCases);
            return LoadTestCaseFilesFromXml(TestFileLoader.MissingHttpTestCases);
        }

        /// <summary>
        /// Returns a list of TestCaseData from the specified XML File.
        /// </summary>
        /// <param name="filepath">The FULL file path of the XML file to open.</param>
        /// <returns>An IList of TestCaseData objects which contain the full path of each test URI.</returns>
        private static IList<TestCaseData> LoadTestCaseFilesFromXml(string filepath)
        {
            if(!File.Exists(filepath))
                throw new FileNotFoundException(string.Format("File {0} was not found!", filepath));

            var testcasepaths = GetTestCasePathsFromXml(filepath);

            return testcasepaths.Select(item => (new TestCaseData(item.Value)).SetName(item.Key)).ToList();
        }

        private static IEnumerable<KeyValuePair<string, string>> GetTestCasePathsFromXml(string filepath)
        {
            var doc = new XmlDocument();
            doc.Load(filepath);
            var xmlTestCases = doc.GetElementsByTagName("testcase");

            return xmlTestCases.Cast<XmlNode>().ToDictionary(xmlTestCase => xmlTestCase["name"].InnerText, xmlTestCase => xmlTestCase["uri"].InnerText);
        }

        /// <summary>
        /// Returns a list of TestCaseData from the specified directory.
        /// </summary>
        /// <param name="directory">The FULL path of the directory to open.</param>
        /// <returns>An IList of TestCaseData objects which contain the full path of each file.</returns>
        private static IList<TestCaseData> LoadTestCaseFilesFromDirectory(string directory)
        {
            return GetTestFilePathsFromDirectory(directory).Select(filepath => (new TestCaseData(filepath.FullName)).SetName(filepath.Name)).ToList();
        }

        private static IEnumerable<FileInfo> GetTestFilePathsFromDirectory(string directory)
        {
            var dir = new DirectoryInfo(directory);
            if(dir.Exists){
                return dir.GetFiles(TestFileLoader.TestFileSearchPattern);
            }

            throw new DirectoryNotFoundException(string.Format("Directory {0} was not found!", directory));
        }
    }
}
