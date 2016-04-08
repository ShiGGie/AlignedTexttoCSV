/**********************************************
 *    Copyright 2016 Johnny Xie
   
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 * ********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using ConvertorClass.Containers;
using ConvertorClass;


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

            //input path
            var input = args[2];
            if (!Directory.Exists(input))
            {
                Console.WriteLine("Specified input directory does not exist.");
                System.Environment.Exit(1);
            }

            //output path
            var output = Directory.GetCurrentDirectory() + "\\csvfiles";
            if (!Directory.Exists(output))
                Directory.CreateDirectory(output);

            //process files in input path
            var filepaths = Directory.GetFiles(input);
            foreach (string path in filepaths)
            {
                ProcessPath(path, args, output);
            }

            Console.WriteLine(String.Format("Successfully converted files to .csv at {0}", output));
        }


        private static void ProcessPath(string path, string[] args, string output)
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



                string delim = "  ";
                if (args.Length > 3)
                {
                    if (args[3] == "tab")
                    {
                        delim = "\t";
                        Console.WriteLine("Using tab as separator..");
                    }
                    else if (args[3] == "spaces")
                    {
                        Console.WriteLine("Using spaces as separator..");
                    }
                    else
                    {
                        delim = args[3];
                        Console.WriteLine("Using custom separator..");
                    }
                }
                else
                    Console.WriteLine("Using spaces as separator..");

                bool skip = true;
                if (args.Length > 4)
                {
                    if (args[4] == "no")
                    {
                        skip = false;
                        Console.WriteLine("Keeping all malformed data.");
                    }
                }

                ProcessFile(path, output, headerLines, dataFormatInt, primaryKey, new string[] { delim }, skip);
            }
            else
                Console.WriteLine("Unknown Error: Files does not exist. Skipping.");
        }

        private static void ProcessFile(string input, string output, int numHeaderLines, int dataFormat, int primaryKey, string[] delim, bool skip)
        {
            TextFileMetadata textFile = new TextFileMetadata(input);
            CSVFileMetadata csvFile = new CSVFileMetadata(output, numHeaderLines, dataFormat, primaryKey, delim);
            Convertor convertor = new Convertor(textFile, csvFile, skip);
            convertor.Run();
        }

    }
}
