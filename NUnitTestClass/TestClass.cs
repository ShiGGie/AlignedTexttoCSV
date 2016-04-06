using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Collections;
using CSVConverterConsole;
using CSVConverterConsole.Containers;

namespace TestClass
{

    /// <summary>
    /// TestFixture defines a class of tests
    /// </summary>
    [TestFixture(), Category("Convertor Class"), Description("NUnit tests CSVConverter")]
    public class Test
    {
        public static string input = "datasamples\\";
        public static TextFileMetadata textFile;
        public static CSVFileMetadata csvFile;
        public static Convertor convertor;
        public static ModelData modelData;

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


            textFile = new TextFileMetadata("datasamples\\size7header-multiline\\primarykey107\\short-MA_var2y00to01.txt");
            csvFile = new CSVFileMetadata(output, numHeaderLines, dataFormat, primaryKey, delim);
            convertor = new Convertor(textFile, csvFile);
            modelData = new ModelData();
            
            
            convertor.Run();
        }
        [Test]
        public void greatestHeaderWordList()
        {    
            Assert.IsNotEmpty(convertor.greatestHeaderWordList);
            CollectionAssert.AreEqual(convertor.greatestHeaderWordList, modelData.greatestHeaderWordList);
        }

        [Test]
        public void headerList()
        {
            Assert.IsNotEmpty(convertor.headerList);
            CollectionAssert.AreEqual(convertor.headerList, modelData.headerList);
        }

        [Test]
        public void fileHeader()
        {
            //String Asserts
        }

        [Test]
        public void fileData()
        {
            //String Asserts
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            
        }
    }
}
