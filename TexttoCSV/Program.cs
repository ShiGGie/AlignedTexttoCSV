using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVConverterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Program requires at least 3 arguments.");
                System.Environment.Exit(1);
            }

            var input = args[2];
            var output = Directory.GetCurrentDirectory() + "\\csvfiles";

            if (!Directory.Exists(output))
                Directory.CreateDirectory(output);

            var filepaths = Directory.GetFiles(input);
            foreach (string path in filepaths)
            {
                if (File.Exists(path))
                {
                    // Args
                    int headerLines;
                    if (!int.TryParse(args[0], out headerLines))
                    {
                        Console.WriteLine("Invalid Number of header lines, Argument 1.");
                        System.Environment.Exit(1);
                    };

                    if (headerLines > 50)
                    {
                        Console.WriteLine("You will be processing a header with more than 50 lines (Press ENTER)");
                        Console.ReadLine();
                    }

                    int dataFormatInt = 1;
                    int primaryKey = -1;
                    //dataformat
                    string dataFormat = args[1].ToLower();
                    if (dataFormat == "one")
                    {
                        dataFormatInt = 1;
                    }
                    else
                    {
                        var dataFormatSplit = dataFormat.Split(',');

                        if (dataFormatSplit.Length == 2)
                        {
                            if (dataFormatSplit[0] != "multi")
                            {
                                Console.WriteLine(String.Format("Invalid Command: {0}", dataFormat));
                                Console.WriteLine("Invalid Multi-line data format specified. The format is: multi,#");
                                System.Environment.Exit(1);
                            }
                            if (!int.TryParse(dataFormatSplit[1], out primaryKey))
                            {
                                Console.WriteLine("Please insert a valid primary key column number(>1)");
                                System.Environment.Exit(1);
                            }

                            dataFormatInt = 0;
                        }
                        else
                        {
                            Console.WriteLine(String.Format("Invalid Command: {0}", dataFormat));
                            Console.WriteLine("Invalid Multi-line data format specified. The format is: multi,#");
                            System.Environment.Exit(1);
                        }
                    }

                    //input path
                    if (!Directory.Exists(input))
                    {
                        Console.WriteLine("Specified input directory does not exist.");
                        System.Environment.Exit(1);
                    }


                    string delim = "  ";
                    if (args.Length > 3)
                    {
                        if (args[3] == "tab")
                        {
                            delim = "\t";
                            Console.WriteLine("Using tab as separator..");
                        }
                        else
                        {
                            delim = args[3];
                            Console.WriteLine("Using custom separator..");
                        }
                    }
                    else
                        Console.WriteLine("Using spaces as separator..");

                    ProcessFile(path, output, headerLines, dataFormatInt, primaryKey, new string[] { delim });
                }
            }

            Console.WriteLine(String.Format("Successfully converted files to .csv at {0}", output));
        }

        public static void ProcessFile(string input, string output, int numHeaderLines, int dataFormat, int primaryKey, string[] delim)
        {
            Console.WriteLine(String.Format("Processing {0}..", input));
            //read line
            using (StreamReader readtext = new StreamReader(input))
            {
                var newName = Regex.Replace(Path.GetFileNameWithoutExtension(input), @"\W|_", "_");

                string fileNumString = "";
                int fileNumber = 0;
                string newLine = ""; //output line
                string headerLine = ""; //output headerline
                bool completed = false;
                long lineIndex = 0;
                long writtenBytes = 0;
                string line;

                ArrayList headerList = new ArrayList();
                ArrayList greatestHeaderWordList = new ArrayList();

                while (!completed)
                {

                    fileNumber++;
                    if (fileNumber > 1)
                        fileNumString = String.Format("_{0}", fileNumber);

                    string newName2 = String.Format("{0}\\{1}{2}.csv", output, newName, fileNumString);
                    using (StreamWriter writetext = new StreamWriter(newName2))
                    {
                        ArrayList dataList = new ArrayList();
                        ArrayList lineCountList = new ArrayList();

                        writtenBytes = 0;

                        if (headerLine != "")
                            writetext.WriteLine(headerLine);
                        if (newLine != "")
                            writetext.WriteLine(newLine);

                        while ((line = readtext.ReadLine()) != null)
                        {
                            //lineLengthList.Add(line.Length);

                            if (line.Trim() != "")
                            {
                                //Found first line of header.
                                #region First Line
                                if (lineIndex == 0)
                                {
                                    //Split by two spaces
                                    string[] lineSplit = line.Split(delim, StringSplitOptions.None);

                                    int totalNullCount = 0;
                                    int totalCharCount = 0;

                                    //1 Count number of nulls. aka two spaces with nothing in them
                                    //2 Add non nulls to an arraylist

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
                                            headerList.Add(s.Trim().Replace(',', ' ').Replace('"', ' ')); //Might be off by a char. Nope nvm, counting char happens down.
                                            totalCharCount += s.Length;

                                            //int addCount2 = totalNullCount % 2 == 1 ? totalNullCount / 2 + 1 : totalNullCount / 2;
                                            //int addCount1 = totalNullCount / 2;
                                            //int addCount2 = 0;
                                            //int addCount1 = totalNullCount;

                                            if (arrayIndex - 1 >= 0)
                                            {
                                                lineCountList[arrayIndex - 1] = (int)lineCountList[arrayIndex - 1] + totalNullCount;
                                                lineCountList.Add(totalCharCount);// + addCount2);
                                            }
                                            else
                                                lineCountList.Add(totalCharCount + totalNullCount);



                                            //Always a "delimiter" missing from calculations when splitting.\. in between split thing.
                                            //totalNullCount += 2;
                                            totalNullCount = 2;
                                            totalCharCount += 2;

                                            arrayIndex++;
                                        }
                                    }

                                    //Leftover spaces
                                    lineCountList[arrayIndex - 1] = (int)lineCountList[arrayIndex - 1] + totalNullCount - 4;

                                }
                                #endregion
                                else if (lineIndex < numHeaderLines)
                                #region Other header lines
                                {
                                    //Split by two spaces
                                    string[] lineSplit = line.Split(delim, StringSplitOptions.None);
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
                                                Console.WriteLine("Bug has occurred. Please check input files. Continuing process..");
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
                                #endregion
                                else if (dataList.Count == 0)
                                #region Data Lines
                                { // Use lineCountList to know how much each cell is suppose to be in length
                                    //Split by two spaces
                                    string[] lineSplit = line.Split(delim, StringSplitOptions.None);
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
                                        Console.WriteLine(String.Format("ERROR: Line {0} Misaligned Column. See \\helpfiles to manually fix. Continuing.. ", lineIndex));

                                        while (dataList.Count < headerList.Count)
                                        {
                                            dataList.Add("");
                                        }
                                        while (dataList.Count > headerList.Count)
                                        {
                                            dataList.RemoveAt(dataList.Count - 1);
                                        }
                                    }
                                }
                                #endregion
                                else
                                #region Other data Lines
                                { // Use lineCountList to know how much each cell is suppose to be in length
                                    //Split by two spaces
                                    // Multi only
                                    string[] lineSplit = line.Split(delim, StringSplitOptions.None);
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
                                                Console.WriteLine("Bug has occurred. Please check input files. Continuing process..");
                                                Console.ReadLine();
                                                i = lineCountList.Count - 1;
                                            }


                                            dataList[i] = string.Format("{0} {1}",
                                            (string)dataList[i], s.Trim().Replace(',', ' ').Replace('"', ' '));

                                            totalCharCount += 2;
                                        }
                                    }
                                }
                                #endregion

                                #region postprocess
                                if (lineIndex < numHeaderLines - 1)
                                {
                                    if (lineIndex == 0)
                                    {
                                        //Perform Deep copy
                                        foreach (string e in headerList)
                                        {
                                            greatestHeaderWordList.Add(e);
                                        }
                                    }

                                    lineIndex++;
                                }
                                else if (lineIndex == numHeaderLines - 1)
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


                                    if (dataFormat == 0)
                                    {

                                        if (!(primaryKey > 0 && primaryKey <= headerList.Count))
                                        {
                                            throw new Exception("Primary key is out of range");
                                        }
                                    }

                                    lineIndex++;
                                }
                                else if (lineIndex > numHeaderLines - 1)
                                {
                                    //write comma seperated line.
                                    if (dataFormat == 1 ||
                                        (dataFormat == 0 && ((string)dataList[primaryKey]) != ""))
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
                                        writtenBytes += newLine.Length;

                                        if (writtenBytes < 100000000)
                                            writetext.WriteLine(newLine);
                                        else
                                            break;
                                    }

                                    dataList = new ArrayList();
                                    lineIndex++;
                                }
                                #endregion


                            }//End if not null line 

                        } //End while reading the line.

                        if (writtenBytes < 100000000)
                            completed = true;
                    }//while not completed
                }//streamwriter

                Console.WriteLine(String.Format("OK"));
            }//streamreader
        }

    }
}
