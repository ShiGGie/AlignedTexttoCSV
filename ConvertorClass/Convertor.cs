using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using ConvertorClass.Containers;

namespace ConvertorClass
{
    public class Convertor
    {
        public readonly TextFileMetadata textFile;
        public readonly CSVFileMetadata csvFile;
        private const int ONELINE = 1;
        private const int MULTILINE = 0;
        private string headerLine = "";
        private readonly bool skip;
        public AlignedWordsToTableProcessor AWTTP;

        public Convertor(TextFileMetadata input, CSVFileMetadata output, bool skipErrors)
        {

            this.textFile = input;
            this.csvFile = output;
            this.skip = skipErrors;
        }

        public void Run()
        {
            AWTTP = new AlignedWordsToTableProcessor(skip);

            Console.WriteLine(String.Format("Processing {0}..", textFile.filePath));
            using (StreamReader readtext = new StreamReader(textFile.filePath))
            {
                long lineIndex = 0; //Which line are we at in the file?
                string line; //input from file
                string newName = String.Format("{0}\\{1}.csv",
                    csvFile.filePath, 
                    createCSVFileName(textFile.filePath));

                using (StreamWriter writetext = new StreamWriter(newName))
                {
                    while ((line = readtext.ReadLine()) != null)
                    {
                        if (line.Trim() != "")
                        {
                            string[] lineSplit = line.Split(csvFile.delim, StringSplitOptions.None);
                            if (lineIndex == 0)
                                AWTTP.ProcessFirstHeaderLine(lineSplit);
                            else if (lineIndex < csvFile.numHeaderLines)
                                AWTTP.ProcessHeaderLines(lineSplit); 
                            else if (AWTTP.dataList.Count == 0) //Some coupling here
                                AWTTP.ProcessFirstDataLine(lineSplit, lineIndex);
                            else
                                AWTTP.ProcessOtherDataLines(lineSplit);

                            if (lineIndex == csvFile.numHeaderLines - 1)
                                WriteHeaderLine(AWTTP, writetext);
                            else if (lineIndex > csvFile.numHeaderLines - 1)
                                WriteDataLine(AWTTP, writetext);

                            lineIndex++;
                        }//End if not null line 
                    } //End while reading the line.
                }//writer
                Console.WriteLine(String.Format("OK"));
            }//reader
        }

        private void WriteHeaderLine(AlignedWordsToTableProcessor AWTTP, StreamWriter writetext)
        {
            //write headerList
            AWTTP.CombineToSASHeaderLine();

            headerLine = string.Join(",", AWTTP.headerList.ToArray());
            writetext.WriteLine(headerLine);


            if (csvFile.dataFormat == MULTILINE)
            {
                if (!(csvFile.primaryKey > 0 && csvFile.primaryKey <= AWTTP.headerList.Count))
                {
                    throw new Exception("Primary key is out of range because it exceeded the number of columns in the Header.");
                }
            }
        }

        private void WriteDataLine(AlignedWordsToTableProcessor AWTTP, StreamWriter writetext)
        {
            //Make sure ONELINE is set or that MULTILINE primary key record is not NULL
            //dataList[csvFile.primaryKey] == NULL : Not a new row of data, instead it follows from the previous.
            //dataList[csvFile.primaryKey] != NULL : A new row of data
            if (csvFile.dataFormat == ONELINE ||
                (csvFile.dataFormat == MULTILINE && AWTTP.dataList[csvFile.primaryKey] != ""))
            {
                AWTTP.PadDataList();
                string newLine = string.Join(",", AWTTP.dataList.ToArray());
                writetext.WriteLine(newLine);
            }
            AWTTP.ClearDataList();
        }

        /// <summary>
        /// Returns a filename
        /// </summary>
        /// <remarks>Keeps alphanumeric characters, to conform with future external software</remarks>
        private string createCSVFileName(string name)
        {
            return Regex.Replace(Path.GetFileNameWithoutExtension(name), @"\W|_", "_");
        }
    }
}
