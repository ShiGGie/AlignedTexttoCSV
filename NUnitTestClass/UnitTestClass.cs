using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Collections;
using ConvertorClass.Containers;
using ConvertorClass;
using System.Collections;


namespace TestClass
{
    class UnitTestClass
    {

        /// <summary>
        /// TestFixture defines a class of tests
        /// </summary>
        [TestFixture(), Category("Unit"), Description("Tests AlignedWordsToTableProcessor")]
        public class Test
        {
            public static TextFileMetadata testTextFile;
            public static CSVFileMetadata testCsvFile;
            public static AlignedWordsToTableProcessor testAWTTP;
            public static ModelData testModelData;

            [Test]
            public void init()
            {
                //output path
                testAWTTP = new AlignedWordsToTableProcessor(true);
                testModelData = new ModelData();
            }

            [Test, Category("Header")]
            public void ProcessFirstHeaderLine()
            {
                string[] lineSplit = 
                    ProcessFirstHeaderLine(lineSplit);
                stub.headerList = headerList;
                stub.greatestHeaderWordList =
                stub.lineCountList = 
            }

            [Test, Category("Header")]
            public void ProcessHeaderLines2()
            {
                string[] lineSplit = 
                ProcessHeaderLines(lineSplit);
                stub.headerList = headerList;
                stub.greatestHeaderWordList =
                stub.lineCountList = 
            }

            [Test, Category("Header")]
            public void headerList()
            {
                Assert.IsNotEmpty(testAWTTP.headerList);
                CollectionAssert.AreEqual(testAWTTP.headerList, testModelData.headerList);
            }


            [Test, Category("WordLengths")]
            public void greatestHeaderWordList()
            {
                Assert.IsNotEmpty(testAWTTP.greatestHeaderWordList);
                CollectionAssert.AreEqual(testAWTTP.greatestHeaderWordList, testModelData.greatestHeaderWordList);
            }

            [Test, Category("Data")]
            public void ProcessFirstDataLine()
            {
                ProcessFirstDataLine(string[] lineSplit, long lineIndex);
                dataList = ;
            }

            [Test, Category("Data")]
            public void ProcessOtherDataLines(string[] lineSplit)
            {
                ProcessFirstDataLine(string[] lineSplit, long lineIndex);
                dataList = ;
            }

            [Test, Category("Data")]
            public void CompleteDataCheck()
            {
                string[] lineSplit = 
                    ProcessHeaderLines(lineSplit);
                stub.headerList = headerList;
                stub.greatestHeaderWordList =
                stub.lineCountList = 
            }

            [Test, Category("Helper")]
            public void CombineToSASHeaderLine()
            {
                //For SAS format, must eliminate duplicates in header as well as
                // truncate to 32 character.

                var map = new Dictionary<string, int>();
                for (int s = 0; s < headerList.Count; s++)
                {

                    string str = (string)headerList[s];
                    string substring;

                    if (str.Length > 29)
                        substring = str.Substring(0, 30);
                    else
                        substring = str.Substring(0, str.Length);

                    if (map.ContainsKey(substring))
                    {
                        map[substring]++;
                        headerList[s] = String.Format("\"{1}{0}\"", str,
                            map[substring]);

                    }
                    else
                    {
                        map.Add(substring, 1);
                        headerList[s] = String.Format("\"{0}\"", str);
                    }
                }
            }


            [Test, Category("Helper")]
            public void PadDataList()
            {
                while (dataList.Count < headerList.Count)
                {
                    dataList.Add("");
                }
                while (dataList.Count > headerList.Count)
                {
                    dataList.RemoveAt(dataList.Count - 1);
                }

                for (int i = 0; i < dataList.Count; i++)
                {
                    dataList[i] = String.Format("\"{0}\"", dataList[i]);
                }

                if (dataList.Count != headerList.Count)
                    throw new Exception("Data error: Datalist != HeaderList count");
            }

            [Test, Category("Helper")]
            public void ClearDataList()
            {
                dataList = new List<String>();
            }


            [TestFixtureTearDown]
            public void FixtureTearDown()
            {

            }
        }
    }
}
