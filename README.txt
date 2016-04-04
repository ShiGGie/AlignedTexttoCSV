TexttoCSV(Opensource Version) - README 
Date: 4.7.2016
Author: John Xie (ShiGGie)

This software converts .txt=>.csv

### Requirements
* Windows XP/Vista/7/8/10

### Input data format
See /datasamples for the expected types of aligned text files.

### Using the Software
Command-line:
	CSVConverterConsole.exe (number of header lines) ("one" or "multi" data format) (/%~inputfilesDirectory~%/) (Optional: delimiter)
	Example: CSVConverterconsole 9 one "datasamples\size9header-one"
	Example: CSVConverterconsole 7 multi,107 "datasamples\size7header-multiline\primarykey107"
        Example: CSVConverterconsole 8 multi,56 "datasamples\size8header-multiline\primarykey56"
	Example: CSVConverterconsole 1 multi,1 "datasamples\size1header-tab-poorlyformatted" "tab"	

  *See /_helpfiles/DisplayHeader for finding your primary key column
  *See /_helpfiles/Misalignment for dealing with misaligned textfiles.
	
	
	
### Using multi-lines on GUI and console
	Console:
		Remember to type "multi,#" where # indicates the primary key in the data in terms of column number
		In the data samples, # = 1. (Since all records should have a date)
	GUI:
		Select 'multi'
		A new option should appear. Type the Primary key column in that textbox.
	
	
	
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

##License
Unless otherwise specified, this software is under the APACHE License.
   Copyright 2016 Johnny Xie <jx7189@gmail.com>
   
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
