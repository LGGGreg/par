#/bin/bash
#
# Manual Mono loader due to the graphical loader failing on Linux
#  By Fred Rookstown (SLName to prevent stalkan)
#
# To remove a plugin, remove a --load= switch.
# You can add a plugin by adding a similar line.
#
# I may end up writing a working loader for Mono.
#
# MIT License here.
#
# $Id$

# ***CHANGE THIS TO POINT TO WHEREVER YOU INSTALLED PAR.***
#  NO ENDING SLASH.
PAR_INSTALL=/home/nexis/Desktop/PAR

# Change this if you need to.
PAR_PORT=8080

PAR_PLUGINS=""

### Plugin Config:
#  Comment out a plugin if you don't want it. (Prepend a # to that line)

# PubComb (Don't use the other variant of this DLL on Linux.  Just trust me.)'
#
#   Plugin Container system to put other plugins into the same gui.
#   Do not load any plugin that PubComb already has, else you get duplicates.
#   Currently Includes:
#	ClientDetection.dll,IMLocator.dll,Lyra.dll,Handicap.dll,Penny.dll,DisableCaps.dll,LeetSpeak.dll,Cinderella.dll,ProfileFun.dll. GetHigh.dll, SpamProtection.dll, SitAntwhere.dll,FileProtection.dll, RadarChat.dll, ProTextion.dll, AwesomeSauce.dll,Coin.dll, SelectionBeams.dll, Retreat.dll
PAR_PLUGINS=" ${PAR_PLUGINS} --load=${PAR_INSTALL}/Plugins/PubComb-NoGUI.dll"  

# Useful stuff.
PAR_PLUGINS=" ${PAR_PLUGINS} --load=${PAR_INSTALL}/Plugins/useful.dll"  

# NeilSay
#PAR_PLUGINS=" ${PAR_PLUGINS} --load=${PAR_INSTALL}/Plugins/NeilSay.dll"  

# Shadow (GUI APPEARS to work on Ubuntu, need others to confirm.)
#PAR_PLUGINS=" ${PAR_PLUGINS} --load=${PAR_INSTALL}/Plugins/Shadow.dll"  

cd ShadowFiles
mono GridProxyApp.exe --proxy-login-port=${PAR_PORT} ${PAR_PLUGINS}

# For debugging any exceptions that occur.
sleep 10
