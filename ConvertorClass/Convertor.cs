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
        private readonly bool skip;
        public List<String> headerList { get; private set; }
        public List<String> greatestHeaderWordList { get; private set; }

        public Convertor(TextFileMetadata input, CSVFileMetadata output, bool skipErrors)
        {
            this.textFile = input;
            this.csvFile = output;
            this.headerList = new List<String>();
            this.greatestHeaderWordList = new List<String>();
            this.skip = skipErrors;
        }

        public void Run()
        {
            Console.WriteLine(String.Format("Processing {0}..", textFile.filePath));
            //read line
            using (StreamReader readtext = new StreamReader(textFile.filePath))
            {
                var newName = Regex.Replace(Path.GetFileNameWithoutExtension(textFile.filePath), @"\W|_", "_");

                string newLine = ""; //csvFile.filePath line
                string headerLine = ""; //csvFile.filePath headerline
                long lineIndex = 0;
                string line;

                string newName2 = String.Format("{0}\\{1}.csv", csvFile.filePath, newName);
                using (StreamWriter writetext = new StreamWriter(newName2))
                {
                    List<String> dataList = new List<String>();
                    List<int> lineCountList = new List<int>();


                    if (headerLine != "")
                        writetext.WriteLine(headerLine);
                    if (newLine != "")
                        writetext.WriteLine(newLine);

                    while ((line = readtext.ReadLine()) != null)
                    {
                        #region process each line
                        if (line.Trim() != "")
                        {
                            if (lineIndex == 0)
                            {
                                ProcessFirstLine(line, lineCountList);
                            }
                            else if (lineIndex < csvFile.numHeaderLines)
                            {
                                ProcessHeaderLines(line, lineCountList);
                            }
                            else if (dataList.Count == 0)
                            {
                                ProcessFirstDataLine(line, lineCountList, dataList, lineIndex);
                            }
                            else
                            {
                                ProcessOtherDataLines(line, lineCountList, dataList);
                            }

                            #region postprocess
                            if (lineIndex == 0)
                            {
                                //Perform Deep copy
                                foreach (string e in headerList)
                                {
                                    greatestHeaderWordList.Add(e);
                                }
                                lineIndex++;
                            }

                            else if (lineIndex < csvFile.numHeaderLines - 1)
                            {
                                lineIndex++;
                            }
                            else if (lineIndex == csvFile.numHeaderLines - 1)
                            {
                                WriteHeaderLine(headerLine, writetext);
                                lineIndex++;
                            }
                            else if (lineIndex > csvFile.numHeaderLines - 1)
                            {
                                //write comma separated line.
                                if (csvFile.dataFormat == 1 ||
                                    (csvFile.dataFormat == 0 && ((string)dataList[csvFile.primaryKey]) != ""))
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
                                        dataList[i] = String.Format("\"{0}\"", (string)dataList[i]);
                                    }

                                    if (dataList.Count != headerList.Count)
                                        throw new Exception("Data error: Datalist != HeaderList count");

                                    newLine = string.Join(",", dataList.ToArray());
                                    writetext.WriteLine(newLine);

                                }

                                dataList = new List<String>();
                                lineIndex++;
                            }
                            #endregion


                        }//End if not null line 
                        #endregion
                    } //End while reading the line.

                }//writer

                Console.WriteLine(String.Format("OK"));

            }//reader
        }

        private void ProcessFirstLine(string line, List<int> lineCountList)
        {
            //Split by two spaces
            string[] lineSplit = line.Split(csvFile.delim, StringSplitOptions.None);

            int totalNullCount = 0;
            int totalCharCount = 0;

            //1 Count number of nulls. aka two spaces with nothing in them
            //2 Add non nulls to an List<String>

            int arrayIndex = 0;
            foreach (string s in lineSplit)
            {
                if (s.Trim() == "")
                {
                    totalNullCount += 2;
                    totalCharCount += 2;
                }
                else
                {
                    headerList.Add(s.Trim().Replace(',', ' ').Replace('"', ' '));
                    totalCharCount += s.Length;

                    if (arrayIndex - 1 >= 0)
                    {
                        lineCountList[arrayIndex - 1] = (int)lineCountList[arrayIndex - 1] + totalNullCount;
                        lineCountList.Add(totalCharCount);
                    }
                    else
                        lineCountList.Add(totalCharCount + totalNullCount);

                    //Always a "csvFile.delimiter" missing from calculations when splitting.\. in between split thing.
                    totalNullCount = 2;
                    totalCharCount += 2;

                    arrayIndex++;
                }
            }

            //Leftover spaces
            lineCountList[arrayIndex - 1] = (int)lineCountList[arrayIndex - 1] + totalNullCount - 4;
        }

        private void ProcessHeaderLines(string line, List<int> lineCountList)
        {
            string[] lineSplit = line.Split(csvFile.delim, StringSplitOptions.None);
            int totalCharCount = 0;

            foreach (string s in lineSplit)
            {
                if (s.Trim() == "")
                {
                    totalCharCount += 2;
                }
                else
                {

                    totalCharCount += s.Length;
                    int i = 0;
                    while (i < lineCountList.Count && totalCharCount > (int)lineCountList[i])
                    {
                        i++;
                    }

                    //possible bug - should never get to here
                    if (i >= lineCountList.Count)
                    {
                        Console.WriteLine("Bug has occurred. Please check textFile.input files. Continuing process..");
                        Console.ReadLine();
                        i = lineCountList.Count - 1;
                    }


                    //Adjusting alignment
                    //Check if left aligned

                    if (i > 0)
                    {
                        string str = s.Trim();
                        int compare2 = totalCharCount - s.Length;
                        int compare1 = (int)lineCountList[i - 1];
                        if ((compare1 - compare2) > 1)
                        {
                            int difference = (str.Length - ((string)greatestHeaderWordList[i]).Length) + 1;
                            if (difference > 1)
                            {
                                lineCountList[i - 1] = (int)lineCountList[i - 1] - (difference) / 2;
                                //lineCountList[i] = (int)lineCountList[i] + difference/2; Because there's plenty                of space already.
                                greatestHeaderWordList[i] = str;
                            }
                        }
                    }

                    headerList[i] = string.Format("{0} {1}",
                        (string)headerList[i], s.Trim().Replace(',', ' ').Replace('"', ' '));

                    totalCharCount += 2;

                }
            }
        }

        private void ProcessFirstDataLine(string line, List<int> lineCountList, List<String> dataList, long lineIndex)
        {
            // Use lineCountList to know how much each cell is suppose to be in length
            //Split by two spaces
            string[] lineSplit = line.Split(csvFile.delim, StringSplitOptions.None);
            int totalCharCount = 0;
            int iMax = -1;
            int i = 0;

            foreach (string s in lineSplit)
            {
                if (s.Trim() == "")
                {
                    totalCharCount += 2;
                }
                else
                {
                    totalCharCount += s.Length;
                    i = 0;
                    while (i < lineCountList.Count && totalCharCount > (int)lineCountList[i])
                    {
                        if (i > iMax)
                            dataList.Add("");
                        i++;

                    }

                    if (i > iMax)
                        iMax = i;
                    dataList.Add(s.Trim().Replace(',', ' ').Replace('"', ' '));

                    totalCharCount += 2;
                }
            }


            //leftovers
            while (i < lineCountList.Count - 1)
            {
                dataList.Add("");
                i++;
            }

            if (dataList.Count != headerList.Count)
            {
                if (!skip)
                {
                    Console.WriteLine(String.Format("ERROR: Line {0} Misaligned Column. See \\helpfiles to manually fix.", lineIndex));
                    while (dataList.Count < headerList.Count)
                    {
                        dataList.Add("");
                    }
                    while (dataList.Count > headerList.Count)
                    {
                        dataList.RemoveAt(dataList.Count - 1);
                    }

                }
                else
                    Console.WriteLine(String.Format("Skipping Line {0} Misaligned Column.", lineIndex));
            }
        }

        private void ProcessOtherDataLines(string line, List<int> lineCountList, List<String> dataList)
        {
            // Use lineCountList to know how much each cell is suppose to be in length
            //Split by two spaces
            // Multi only
            string[] lineSplit = line.Split(csvFile.delim, StringSplitOptions.None);
            int totalCharCount = 0;

            foreach (string s in lineSplit)
            {
                if (s.Trim() == "")
                {
                    totalCharCount += 2;
                }
                else
                {

                    totalCharCount += s.Length;
                    int i = 0;
                    while (totalCharCount > (int)lineCountList[i])
                    {
                        i++;
                    }

                    //possible bug - should never get to here
                    if (i >= lineCountList.Count)
                    {
                        Console.WriteLine("Bug has occurred. Please check textFile.input files. Continuing process..");
                        Console.ReadLine();
                        i = lineCountList.Count - 1;
                    }


                    dataList[i] = string.Format("{0} {1}",
                    (string)dataList[i], s.Trim().Replace(',', ' ').Replace('"', ' '));

                    totalCharCount += 2;
                }
            }
        }

        private void WriteHeaderLine(String headerLine, StreamWriter writetext)
        {
            //For SAS format, must eliminate duplicates in header as well as
            // truncate to 32 character.
            //int truncate = 0;
            var map = new Dictionary<string, int>();
            for (int s = 0; s < headerList.Count; s++)
            {

                string str = (string)headerList[s];
                string substring;
                //int tSize = truncate / 10;
                //string substring1 = str.Substring(0, 21);
                //string substring2 = str.Substring(str.Length - (10), (10));
                //headerList[s] = String.Format("{0}_{1}", substring1, substring2);
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

            //write headerList

            headerLine = string.Join(",", headerList.ToArray());
            writetext.WriteLine(headerLine);


            if (csvFile.dataFormat == 0)
            {

                if (!(csvFile.primaryKey > 0 && csvFile.primaryKey <= headerList.Count))
                {
                    throw new Exception("Primary key is out of range");
                }
            }
        }

    }

}
