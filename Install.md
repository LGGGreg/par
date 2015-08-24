## Instructions for Windows ##

To get PAR working with your viewer, watch the tutorial [here](http://www.youtube.com/watch?v=MBNUogQLH0U&feature=channel_page).

  1. Go to the main web site at http://code.google.com/p/par/ (You're already here, lol)
  1. Click the "Download" tab at the top.
  1. Download the file "PAR - Plugins For SL v##.zip"
  1. Unzip the contents somewhere.   You should have two folders, "Plugins", "Shadow Files" and a .bat file
  1. Double click to run the batch file.  (if  you get an error here, you need to install [Microsoft .NET Framework](http://www.microsoft.com/downloads/details.aspx?familyid=10cc340b-f857-4a14-83f5-25634c3bf043&displaylang=en))
  1. A window pops up. Click on the top left "add plugins" button.
  1. Navigate to the folder you extracted named "plugins" and select  everything inside (click drag), then press open.
  1. Press the "start" button .   A few windows (dont close them) should come up.  If you get an error here, please double check you added ONLY the files in the "plugins" folder.
  1. Make a copy of your shortcut to second life, and rename it to "Par - Second life"
  1. Right click on this shortcut, select "properties",
  1. In the window that pops up, edit the "target" field only.  Use your arrow keys to (press right arrow) move to the very end of the text
  1. Copy the next line, and paste it in there at the very end. ```
-loginuri http://localhost:8080/ ```
  1. Press "OK" then double click your new shortcut, you done!

> To run par after the installation, simple double click the batch file, press start, and double click the shortcut

## Linux Installation Instructions ##

  1. Download one of the binary ZIPs and extract it to a directory somewhere.
    * You can also check out the nexis branch on SVN if you wish to try pubcomb's GTK flavor.  <s>Note: PAR-GTK is, at the time of writing, horribly screwed up.</s>
  1. Save the PAR Linux Loader bash script as "linuxloader" in the root of your PAR folder and execute the following in terminal to make it executable: ```
chmod +x ./linuxloader```
  1. Execute linuxloader either by doubleclicking it in your window manager and selecting "Run in Terminal" or by executing  through the terminal itself.
  1. Open your SL install directory in terminal and execute: ```
./secondlife --loginuri http://localhost:8080/```