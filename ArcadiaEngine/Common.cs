using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
namespace SomewhatGeeky.Arcadia.Engine
{
    public static class Common
    {
        private static IdGenerator idGenerator = new IdGenerator();

        public static void WriteListOfStringsToXml(XmlWriter writer, List<string> list, string nodeName, string childNodesName)
        {
            if (list == null)
                return;

            writer.WriteStartElement(nodeName);
            foreach (string member in list)
                writer.WriteElementString(childNodesName, member);
            writer.WriteEndElement();
        }

        public static void ReadListOfStringsFromXml(XmlNode node, string childNodesName, List<string> list)
        {
            if (node == null)
                return;

            foreach (XmlNode otherNameNode in node.SelectNodes(childNodesName))
                if (!String.IsNullOrEmpty(otherNameNode.InnerText))
                    list.Add(otherNameNode.InnerText);
        }

        public static IdGenerator IdGenerator
        {
            get
            {
                return idGenerator;
            }
        }

        public static List<string> StringToList(string value)
        {
            List<string> result = new List<string>();

            value = value.Replace(';', ',');

            foreach (string item in value.Split(','))
                result.Add(item.Trim());

            return result;
        }

        public static string ListToString(List<string> items)
        {
            StringBuilder result = new StringBuilder();
            foreach(string item in items)
                result.Append(item + ", ");
            if (result.Length <= 2)
                return "";
            return result.ToString(0,result.Length-2);

        }

        //public static bool CheckStringIsInListOfStrings(string toCheck, string match1, List<string> otherMatches)
        //{
        //    if (toCheck.Equals(match1, StringComparison.InvariantCultureIgnoreCase))
        //        return true;
        //    foreach (string otherMatch in otherMatches)
        //        if (toCheck.Equals(otherMatch, StringComparison.InvariantCultureIgnoreCase))
        //            return true;
        //    return false;
        //}

        //public static bool CheckStringEqualsStringOrListOfStrings(string toCheck, string match1, List<string> otherMatches)
        //{
        //    if (toCheck.Equals(match1, StringComparison.InvariantCultureIgnoreCase))
        //        return true;
        //    foreach (string otherMatch in otherMatches)
        //        if (toCheck.Equals(otherMatch, StringComparison.InvariantCultureIgnoreCase))
        //            return true;
        //    return false;
        //}

        public static string MakeRelativePath(string path)
        {
            if (path.StartsWith(AppDomain.CurrentDomain.BaseDirectory, StringComparison.InvariantCultureIgnoreCase))
            {
                //make relative path
                return "." + System.IO.Path.DirectorySeparatorChar + path.Substring(AppDomain.CurrentDomain.BaseDirectory.Length);
            }
            return path;
        }

        public static string ContactEmail
        {
            get
            {
                return "arcadia@davidmcgrath.com";
            }
        }

        public static Version LibraryVersion
        {
            get
            {
                return InternalLibraryVersion.GetLibraryVersion(); 
            }
        }
    }
}
