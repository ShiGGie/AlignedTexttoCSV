﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Text;

namespace ConvertorClass
{
    public class AlignedWordsToTableProcessor
    {
        private const int ONELINE = 1;
        private const int MULTILINE = 0;
        //private int debug = 0;

        private readonly bool skip;
        public List<String> headerList { get; private set; }
        public List<String> greatestHeaderWordList { get; private set; }
        public List<String> dataList { get; private set; }
        public List<String> tempDataList { get; private set; }
        public List<int> lineCountList { get; private set; }

        /// <summary>
        /// Processes string[] that has been split according to a delimiter 
        /// which padded/filled the spaces in the aligned text file.
        /// </summary>
        public AlignedWordsToTableProcessor(bool skipErrors)
        {
            this.headerList = new List<String>();
            this.greatestHeaderWordList = new List<String>();
            this.dataList = new List<String>();
            this.tempDataList = new List<String>();
            this.lineCountList = new List<int>();
            this.skip = skipErrors;
        }

        /// <summary>
        /// Process header lines following the first header line
        /// </summary>
        /// <remarks>
        /// Fills lineCount (word lengths) List
        /// Fills HeaderList (each of the header words)
        /// Fills Greatest Word (using word length) List
        /// </remarks>
        public void ProcessFirstHeaderLine(string[] lineSplit)
        {

            int totalNullCount = 0;
            int totalCharCount = 0;

            //1 Count number of nulls. aka two spaces with nothing in them
            //2 Add non nulls to a List<String>

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

                    totalNullCount = 2;
                    totalCharCount += 2;

                    arrayIndex++;
                }
            }

            //Leftover spaces
            lineCountList[arrayIndex - 1] = (int)lineCountList[arrayIndex - 1] + totalNullCount - 4;

            //Perform Deep copy
            foreach (string e in headerList)
            {
                greatestHeaderWordList.Add(e);
            }

            //debug++;
            //DebugToFile("headerLine_" + debug, headerList);
            //DebugToFile("lineCount_" + debug, lineCountList);
            //DebugToFile("greatestHeaderList_" + debug, greatestHeaderWordList);
        }

        /// <summary>
        /// Process header lines following the first header line
        /// </summary>
        /// 
        /// <remarks>
        /// Accepts string[] that has been split from an aligned text file.
        /// </remarks>
        public void ProcessHeaderLines(string[] lineSplit)
        {

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
                    //No need to re adjust for the first cell
                    //This portion attempts to align data cells to the largest header word.
                    // Example: 
                            //          Acquiror
                            //      Accts Payable
                    //data:         xxxxx
                    // It has no effect if words are left aligned. Example:
                            //          Acquiror
                            //          Accts Payable
                    //Data:             xxxxx

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
                                lineCountList[i - 1] = (int)lineCountList[i - 1] - (difference) / 2; //subtract space from previous word to gain space
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

            //debug++;
            //DebugToFile("headerLine_"+debug, headerList);
            //DebugToFile("lineCount_" + debug, lineCountList);
            //DebugToFile("greatestHeaderList_" + debug, greatestHeaderWordList);
        }

        /// <summary>
        /// Process data lines. Must search according to primary key to combine data rows.
        /// </summary>
        /// <remarks>
        /// Returns string of data row or null if the line being processed has no primary key.
        /// </remarks>
        public string ProcessDataLine(string[] lineSplit, long lineIndex, int primaryKey, int dataFormat)
        {
            // Use lineCountList to know how much each cell is suppose to be in length
            //Split by two spaces

            int totalCharCount = 0;
            int iMax = -1;
            int i = 0;
            string return_data = "";

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
                            tempDataList.Add("");
                        i++;

                    }

                    if (i > iMax)
                        iMax = i;
                    tempDataList.Add(s.Trim().Replace(',', ' ').Replace('"', ' '));

                    totalCharCount += 2;
                }
            }


            //leftovers
            while (i < lineCountList.Count - 1)
            {
                tempDataList.Add("");
                i++;
            }

            if (tempDataList.Count != headerList.Count)
            {
                if (!skip)
                {
                    Console.WriteLine(String.Format("ERROR: Line {0} Misaligned Column. See \\helpfiles to manually fix.", lineIndex));
                    while (tempDataList.Count < headerList.Count)
                    {
                        tempDataList.Add("");
                    }
                    while (tempDataList.Count > headerList.Count)
                    {
                        tempDataList.RemoveAt(tempDataList.Count - 1);
                    }

                }
                else
                    Console.WriteLine(String.Format("Skipping Line {0} Misaligned Column.", lineIndex));
            }

            if (checkPrimaryKey(tempDataList, primaryKey, dataFormat))
            {
                if (dataList.Count > 0)
                {
                    return_data = createDataLine();
                    dataList.Clear();
                }
                dataList = deepCopy(tempDataList);
                tempDataList.Clear();
            }
            else
            {
                AppendDataLines();
                tempDataList.Clear(); 
            }

            //debug++;
            //DebugToFile("dataline_" + debug, dataList);

            return return_data;
        }

        //private void DebugToFile(string filename, List<string> l)
        //{
        //    string filename2 = filename + ".txt";
        //    using (StreamWriter sw = new StreamWriter(filename2))
        //    {
        //        sw.Write("public List<String> " + filename + " = new List<String>{");
        //        for (int i = 0; i < l.Count; i++)
        //        {
        //            sw.Write("\""+l[i]+"\"");
        //            if (i != l.Count - 1)
        //                sw.Write(",");
        //        }
        //        sw.Write("};");
        //    }
        //}

        //private void DebugToFile(string filename, List<int> l)
        //{
        //    string filename2 = filename + ".txt";
        //    using (StreamWriter sw = new StreamWriter(filename2))
        //    {
        //        sw.Write("public List<int> " + filename + " = new List<int>{");
        //        for (int i = 0; i < l.Count; i++)
        //        {
        //            sw.Write(l[i]);
        //            if (i != l.Count - 1)
        //                sw.Write(",");
        //        }
        //        sw.Write("};");
        //    }
        //}

        /// <summary>
        /// Process data lines following the first data line
        /// </summary>
        /// <remarks>
        /// Takes tempData and Datalist and merges them.
        /// </remarks>
        public void AppendDataLines()
        {
            if (tempDataList.Count > dataList.Count)
                return;

            for (int i = 0; i < tempDataList.Count; i++)
            {
                if (tempDataList[i] != "")
                {
                    dataList[i] = string.Format("{0} {1}",
                        dataList[i], tempDataList[i]);
                }
            }
        }

        private List<String> deepCopy(List<String> a)
        {
            if (a == null)
                return null;

            List<String> b = new List<String>();
            foreach (string element in a)
            {
                b.Add(element);
            }

            return b;
        }
        /// <summary>
        /// Combines a multi-lined header.
        /// </summary>
        /// <remarks>
        /// For SAS format, header records must be unique when truncated to 32 chars.
        /// </remarks>
        /// <todo>: Should be extracted out into an IContainer </todo>
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

        /// <summary>
        /// Add empty data columns. Also removes extra data columns.
        /// </summary>
        /// <remarks>
        /// Required when there is a column of data and no data after that.
        /// This is somewhat of a failsafe, since ProcessFirstDataline already does this.
        /// 
        /// *This could be unexpected behavior however.
        /// </remarks>
        private void PadAndFormatDataList()
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

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void ClearDataList()
        {
            dataList = new List<String>();
        }

        public void ClearTempList()
        {
            tempDataList = new List<String>();
        }

        private bool checkPrimaryKey(List<string> list, int primaryKey, int dataFormat)
        {
            //Make sure ONELINE is set or that MULTILINE primary key record is not NULL
            //dataList[csvFile.primaryKey] == NULL : Not a new row of data, instead it follows from the previous.
            //dataList[csvFile.primaryKey] != NULL : A new row of data
            return dataFormat == ONELINE ||
                (dataFormat == MULTILINE && list[primaryKey] != "");

        }

        public string createDataLine()
        {
            PadAndFormatDataList();
            return string.Join(",", dataList.ToArray());
        }


    }
}
