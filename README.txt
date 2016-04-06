TexttoCSV(Opensource Version) - README 
Date: 4.7.2016
Author: John Xie (ShiGGie)

This software converts .txt=>.csv

### Requirements
* Windows XP/Vista/7/8/10

### Summary
The goal of this project was to convert strange text files that were inconsistant in their
delimiter usage and each column was separated by varying amounts of spaces.

### Input data format
See /datasamples for the expected types of aligned text files.

### Using the Software
Command-line:
	CSVConverterConsole.exe (number of header lines) ("one" or "multi" data format) (/%~inputfilesDirectory~%/) (Optional: delimiter)
	Example: CSVConverterconsole 9 one "datasamples\size9header-one"
	Example: CSVConverterconsole 7 multi,107 "datasamples\size7header-multiline\primarykey107"
        Example: CSVConverterconsole 8 multi,56 "datasamples\size8header-multiline\primarykey56"
	Example: CSVConverterconsole 1 multi,1 "datasamples\size1header-tab-poorlyformatted" "tab"	


This software requires you to specify the 
1) number of lines that make up the header, 
2) whether each row of data is made of multiple lines or one. 
2.5) The primary key column
3) Path to the file 
4) Optional: the delimiter of the text file. (Default: "spaces")
	
### Using multi-lines on GUI and console
	Console:
		Remember to type "multi,#" where # indicates the primary key in the data in terms of column number
		In the data samples, # = 1. (Since all records should have a date)

	
### Folder structure 
/%~inputfiles~% : insert input data files (text format) 
/csvfiles : contains output .csv files 

### Determining headerlines
This requires that you have an editor to open the TEXT FILES.

Example:
""""""""""""""""""""""""""""""""""
			 
																			
																			
		-	Ratio of    Ratio of    Ratio of      Ratio of       Ratio of   
	H	|	Deal        Deal        Deal          Deal           Enterprise 
	E	|	Value to    Value to    Value to      Value to       Value to   
	A	|	Sales       EBIT        EBITDA        Net Income     Sales      
	D	|																	
	E	|																	
	R	|																	
		|
		-	
			---- First Row of Data ----
			---- Second Row of Data ----
""""""""""""""""""""""""""""""""""
		
	This would have a header size of 7.