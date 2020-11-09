# File-Mover
The problem I was eperiencing.  I had a extremely labor intensive process that was errored prone and wasted a large amount of time upwards of 20-40 times a day.

Issue:
* Had to move printer files that was being saved in one directory to another diretory that had to be created every time based on string in another file.
* I would moved pdf and excel file in one directory to a finish folder.
* Several files were not moved or not all files were moved.


Solution:
* Watch for changes in directory
* When a pdf is moved to the folder.
   A.) Parse file name and create a (2) variables: 1 for creating print file directory and another for moving the excel document to final distination
   B.) Create printer files directory structure based on parsing of file name that was moved.
* Copy printer file from origin to final destination that was created above
* Delete original printer files
* Move the excel file from origin to final destination
