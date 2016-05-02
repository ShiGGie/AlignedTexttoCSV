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
    public class UnitTestClass
    {
        
        /// <summary>
        /// Defines a set of Tests for AWTTP.
        ///     The Stub raw text begins with data and ends with data. (No empty lines before data)
        /// </summary>
        [TestFixture(), Category("Unit"), Description("Tests AlignedWordsToTableProcessor")]
        public class AWTTPUnitTests
        {
            public static StubTableData STD = new StubTableData();
            public static AlignedWordsToTableProcessor AWTTP = new AlignedWordsToTableProcessor(true);
            public static string[] textFile = STD.rawText.Split(new Char[] { '\n' });
            private const int PRIMARYKEY = 1;
            private const int HEADERLINES = 4;
            private const int MULTILINE = 0;
            private const string DELIMITER = "  "; // *TWO* spaces.

            //[TestFixtureSetUp]
            //public void FixtureSetUp()
            //{
            //    STD = new StubTableData();
            //    AWTTP = new AlignedWordsToTableProcessor(true);
            //    textFile = STD.rawText.Split(new Char[] { '\n' });
            //}

            [Test, Category("Header")]
            public void ProcessHeaderLine1()
            {
                AWTTP.ProcessFirstHeaderLine(textFile[0].Split(new string[] { "  " }, StringSplitOptions.None));
                Assert.AreEqual(STD.headerLine_1, AWTTP.headerList);
                Assert.AreEqual(STD.lineCount_1, AWTTP.lineCountList);
                Assert.AreEqual(STD.greatestHeaderList_1, AWTTP.greatestHeaderWordList);
            }

            [Test, Category("Header")]
            public void ProcessHeaderLine2()
            {
                AWTTP.ProcessHeaderLines(textFile[1].Split(new string[] { "  " }, StringSplitOptions.None));
                Assert.AreEqual(STD.headerLine_2, AWTTP.headerList);
                //Assert.AreEqual(STD.lineCount_2, AWTTP.lineCountList);
               // Assert.AreEqual(STD.greatestHeaderList_2, AWTTP.greatestHeaderWordList);
            }

            [Test, Category("Header")]
            public void ProcessHeaderLine3()
            {
                AWTTP.ProcessHeaderLines(textFile[2].Split(new string[] { "  " }, StringSplitOptions.None));
                Assert.AreEqual(STD.headerLine_3, AWTTP.headerList);
               // Assert.AreEqual(STD.lineCount_3, AWTTP.lineCountList);
              //  Assert.AreEqual(STD.greatestHeaderList_3, AWTTP.greatestHeaderWordList);
            }


            [Test, Category("Header")]
            public void ProcessHeaderLine4()
            {
                AWTTP.ProcessHeaderLines(textFile[3].Split(new string[] { "  " }, StringSplitOptions.None));
                Assert.AreEqual(STD.headerLine_4, AWTTP.headerList);
               // Assert.AreEqual(STD.lineCount_4, AWTTP.lineCountList);
               // Assert.AreEqual(STD.greatestHeaderList_4, AWTTP.greatestHeaderWordList);
            }

            [Test, Category("Data")]
            public void ProcessDataLine5()
            {
                AWTTP.ProcessDataLine(textFile[4].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_5, AWTTP.dataList );
            }

            [Test, Category("Data")]
            public void ProcessDataLine6789101112()
            {
                AWTTP.ProcessDataLine(textFile[5].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_6, AWTTP.dataList );
                AWTTP.ProcessDataLine(textFile[6].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_7, AWTTP.dataList);
                AWTTP.ProcessDataLine(textFile[7].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_8, AWTTP.dataList);
                AWTTP.ProcessDataLine(textFile[8].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_9, AWTTP.dataList);
                AWTTP.ProcessDataLine(textFile[9].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_10, AWTTP.dataList);
                AWTTP.ProcessDataLine(textFile[10].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_11, AWTTP.dataList);
                AWTTP.ProcessDataLine(textFile[11].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_12, AWTTP.dataList);
            }

            [Test, Category("Data")]
            public void ProcessDataLine13()
            {
                AWTTP.ProcessDataLine(textFile[12].Split(new string[] { "  " }, StringSplitOptions.None), 0, PRIMARYKEY, MULTILINE);
                Assert.AreEqual(STD.dataline_13, AWTTP.dataList );
            }


            [Test, Category("Helper")]
            public void CombineToSASHeaderLine()
            {
                Assert.True(true);
            }


            [Test, Category("Helper")]
            public void PadDataList()
            {
                Assert.True(true);
            }

            [TestFixtureTearDown]
            public void FixtureTearDown()
            {

            }
        }
    }
}
