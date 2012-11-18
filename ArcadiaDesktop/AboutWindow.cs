using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public class AboutWindow : TextOutputWindow
    {
        public AboutWindow(Window owner)
            : base(owner, Title, Content)
        {
        }

        private static string Title
        {
            get
            {
                return "About " + GuiCommon.MainWindowBaseTitle;
            }
        }

        private static string Content
        {
            get
            {
                return "Welcome to " + GuiCommon.MainWindowBaseTitle + "!"
                    + "\n"
                    + "\n        Arcadia is an emulator frontend that manages a wide variety of classic gaming system files.  I design it to be as configurable as the users want and yet still be easy to use right for the first time.  Going forward I hope to build an online community around Arcadia where users can share game ratings, reviews, links, and game saves.  I also hope to incorporate Arcadia into social networking websites like Facebook and Twitter.  All of this is purely for my own recreation, so I hope you guys are patient with me, and as excited as I am!"
                    + "\n        Look around, enjoy, and email any comments to " + Arcadia.Engine.Common.ContactEmail + ".  I really appreciate your feedback."
                    + "\n        For the latest information, check out posts about Arcadia on my blog: davidmcgrath.com/arcadia"
                    + "\n"
                    + "\nThanks,"
                    + "\nDavid McGrath"
                    + "\n\n"
                    + "\n Arcadia.Desktop Version: " + GuiCommon.Version + " (built " + (GuiCommon.BuildTime.HasValue?GuiCommon.BuildTime.ToString():"unknown") + ")"
                    + "\n Arcadia.Engine Version: " + Arcadia.Engine.Common.LibraryVersion + " (built " + (Arcadia.Engine.Common.BuildTime.HasValue?Arcadia.Engine.Common.BuildTime.ToString():"unknown") + ")"
                    ;
            }
        }
    }
}
