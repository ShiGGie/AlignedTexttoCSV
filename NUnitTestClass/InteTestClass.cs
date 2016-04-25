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

namespace TestClass
{

    /// <summary>
    /// TestFixture defines a class of tests
    /// </summary>
    [TestFixture(), Category("Integration"), Description("NUnit tests CSVConverter")]
    public class Test
    {
        public static TextFileMetadata shortTextFile;
        public static CSVFileMetadata shortCsvFile;
        public static Convertor shortConvertor;
        public static ModelData shortModelData;

        public static TextFileMetadata testTextFile;
        public static CSVFileMetadata testCsvFile;
        public static Convertor testConvertor;
        public static ModelData testModelData;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            //output path
            var output = Directory.GetCurrentDirectory() + "\\csvfiles";
            if (!Directory.Exists(output))
                Directory.CreateDirectory(output);

            int numHeaderLines = 7;
            int dataFormat = 0;
            int primaryKey = 107;
            string[] delim = new string[] { "  " };

            shortTextFile = new TextFileMetadata("..\\..\\..\\datasamples\\size7header-multiline\\primarykey107\\short-MA_var2y00to01.txt");
            shortCsvFile = new CSVFileMetadata(output, numHeaderLines, dataFormat, primaryKey, delim);
            shortConvertor = new Convertor(shortTextFile, shortCsvFile, true);
            shortModelData = new ModelData();


            shortConvertor.Run();
        }
        [Test, Category("Header")]
        public void greatestHeaderWordList()
        {
            Assert.IsNotEmpty(shortConvertor.AWTTP.greatestHeaderWordList);
            CollectionAssert.AreEqual(shortConvertor.AWTTP.greatestHeaderWordList, shortModelData.greatestHeaderWordList);
        }

        [Test, Category("Header")]
        public void headerList()
        {
            Assert.IsNotEmpty(shortConvertor.AWTTP.headerList);
            CollectionAssert.AreEqual(shortConvertor.AWTTP.headerList, shortModelData.headerList);
        }

        [Test, Category("Data")]
        public void fileHeader()
        {
            //String Asserts
            Assert.Fail();
        }

        [Test, Category("Data")]
        public void fileData()
        {
            //String Asserts
            Assert.Fail();
        }


        [TestFixtureTearDown]
        public void FixtureTearDown()
        {

        }
    }
}
