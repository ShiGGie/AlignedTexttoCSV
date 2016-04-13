using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Text;

namespace ConvertorClass
{
    public class AlignedWordsToTableProcessor
    {
        private readonly bool skip;
        public List<String> headerList { get; private set; }
        public List<String> greatestHeaderWordList { get; private set; }
        public List<String> dataList { get; private set; }
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
            this.lineCountList = new List<int>();
            this.skip = skipErrors;
        }


        public void ProcessFirstHeaderLine(string[] lineSplit)
        {

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

            //Perform Deep copy
            foreach (string e in headerList)
            {
                greatestHeaderWordList.Add(e);
            }
        }

        /// <summary>
        /// Process header lines following the first header line
        /// </summary>
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

        public void ProcessFirstDataLine(string[] lineSplit, long lineIndex)
        {
            // Use lineCountList to know how much each cell is suppose to be in length
            //Split by two spaces

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

        /// <summary>
        /// Process data lines following the first data line
        /// </summary>
        /// <remarks>
        /// Accepts string[] that has been split from an aligned text file.
        /// </remarks>
        public void ProcessOtherDataLines(string[] lineSplit)
        {
            // Use lineCountList to know how much each cell is suppose to be in length
            //Split by two spaces
            // Multi only
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

        public void ClearDataList()
        {
            dataList = new List<String>();
        }
    }
}
