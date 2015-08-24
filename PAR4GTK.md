# Introduction #

Because of the issues with the old GUI that LordGregGreg provided, Rob (In the SVN as NexisEntertainment@gmail.com) decided to recode all the GUI elements in PAR with a new system using the widely used GTK graphics library.

This means that this:

![http://flex.nexisonline.net/static/screens/parwin_001.jpg](http://flex.nexisonline.net/static/screens/parwin_001.jpg)

turns into this:

![http://flex.nexisonline.net/static/screens/pargtk_001.jpg](http://flex.nexisonline.net/static/screens/pargtk_001.jpg)

In addition to being pretty, it'll work on Linux, Mac, AND Windows.

## Roadmap ##
  1. Alpha
    * **Initial conversion** - Conversion of at least 25% of all forms (Including main form) to GTK. (**DONE!**)
      * PubComb - About 50-65% complete.
      * Shadow - DONE
      * Loader - Complete
    * **Complete conversion** - All forms and messageboxes converted to GTK.
  1. Beta
  1. 1.0 and binary release.

## Hands Dirty ##

PAR-GTK is being developed on Ubuntu Jaunty Jackalope (9.04) using the latest mono packages.  MonoDevelop is the primary IDE, but it saves as MSBuild 2003 files.

To download the code, you must have Subversion installed.

```
# Checkout the latest PAR-GTK code in readonly mode.
svn co http://par.googlecode.com/svn/branches/GTK/ par-gtk
```

I suggest one uses MonoDevelop IDE for the built-in form builder, but I have included the generated `*`.cs files so that users without monodevelop can build it.

## Binaries ##

**PLEASE NOTE:**  These binaries are built a few minutes after each commit by Cruise, an automated build server, and only if it detects that the build process was successful.  **They are not guaranteed to be stable.**


  * [PAR-GTK Binaries (Cruised)](http://www.nexisonline.net/PAR/packages/PAR-GTK_Cruised.zip)