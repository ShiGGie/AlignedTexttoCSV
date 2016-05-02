using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClass
{

    public class StubTableData
    {
        public List<String> greatestHeaderList_1 = new List<String> { "Acquirer's", "Acquiror", "Acquiror", "SIC", "Acquiror SIC Codes" };
        public List<String> greatestHeaderList_2 = new List<String> { "Acquirer's", "Acquiror", "Acquiror", "Acquiror", "Acquiror SIC Codes" };
        public List<String> greatestHeaderList_3 = new List<String> { "Acquirer's", "Acquiror", "Acquiror", "Acquiror", "Acquiror SIC Codes" };
        public List<String> greatestHeaderList_4 = new List<String> { "Acquirer's", "Acquiror", "Acquiror", "Acquiror", "Acquiror SIC Codes" };
        public List<String> headerLine_1 = new List<String> { "Acquirer's", "Acquiror", "Acquiror", "SIC", "Acquiror SIC Codes" };
        public List<String> headerLine_2 = new List<String> { "Acquirer's Indemnification", "Acquiror CUSIP", "Acquiror Accts Payable", "SIC Acquiror", "Acquiror SIC Codes" };
        public List<String> headerLine_3 = new List<String> { "Acquirer's Indemnification Cap (USD)", "Acquiror CUSIP", "Acquiror Accts Payable LTM", "SIC Acquiror Codes", "Acquiror SIC Codes" };
        public List<String> headerLine_4 = new List<String> { "Acquirer's Indemnification Cap (USD)", "Acquiror CUSIP", "Acquiror Accts Payable LTM (host mil)", "SIC Acquiror Codes", "Acquiror SIC Codes" };
        public List<int> lineCount_1 = new List<int> { 18, 30, 48, 58, 86 };
        public List<int> lineCount_2 = new List<int> { 18, 30, 48, 58, 86 };
        public List<int> lineCount_3 = new List<int> { 18, 30, 48, 58, 86 };
        public List<int> lineCount_4 = new List<int> { 18, 30, 48, 58, 86 };
        public List<String> dataline_5 = new List<String> { "", "87192H", "", "7376", "7376/7372/7379/7374" };
        public List<String> dataline_6 = new List<String> { "", "87192H", "", "7376 7372", "7376/7372/7379/7374" };
        public List<String> dataline_7 = new List<String> { "", "87192H", "", "7376 7372 7379", "7376/7372/7379/7374" };
        public List<String> dataline_8 = new List<String> { "", "87192H", "", "7376 7372 7379 7374", "7376/7372/7379/7374" };
        public List<String> dataline_9 = new List<String> { "", "72022A", "", "6799", "6799" };
        public List<String> dataline_10 = new List<String> { "", "67135K", "", "5182", "5182/5181/5921" };
        public List<String> dataline_11 = new List<String> { "", "67135K", "", "5182 5181", "5182/5181/5921" };
        public List<String> dataline_12 = new List<String> { "", "67135K", "", "5182 5181 5921", "5182/5181/5921" };
        public List<String> dataline_13 = new List<String> { "", "08588T", "", "2834", "2834/2833/2836" };

        public string rawText =
"Acquirer's        Acquiror    Acquiror           SIC      Acquiror SIC Codes            \n" +
"Indemnification    CUSIP      Accts Payable    Acquiror                                    \n" +
"Cap (USD)                      LTM              Codes                                   \n" +
"                              (host mil)                                                \n" +
"                  87192H                       7376       7376/7372/7379/7374           \n" +
"                                               7372                                     \n" +
"                                               7379\n" +
"                                               7374\n" +
"                  72022A                       6799       6799                          \n" +
"                  67135K                       5182       5182/5181/5921                \n" +
"                                               5181\n" +
"                                               5921\n" +
"                  08588T                       2834       2834/2833/2836                \n";

        public StubTableData() { }
    }
}
