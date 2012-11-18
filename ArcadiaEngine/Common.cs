using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
namespace SomewhatGeeky.Arcadia.Engine
{
    public static class Common
    {
        public static void WriteListOfStringsToXml(XmlWriter writer, IEnumerable<string> list, string nodeName, string childNodesName)
        {
            if (list == null)
            {
                return;
            }

            writer.WriteStartElement(nodeName);
            foreach (var member in list)
            {
                writer.WriteElementString(childNodesName, member);
            }

            writer.WriteEndElement();
        }

        public static void ReadListOfStringsFromXml(XmlNode node, string childNodesName, ICollection<string> list)
        {
            if (node == null)
            {
                return;
            }

            foreach (XmlNode otherNameNode in node.SelectNodes(childNodesName))
            {
                if (!String.IsNullOrEmpty(otherNameNode.InnerText))
                {
                    list.Add(otherNameNode.InnerText);
                }
            }
        }

        public static IEnumerable<string> StringToList(string value)
        {
            value = value.Replace(';', ',');

            foreach (string item in value.Split(','))
            {
                yield return item.Trim();
            }
        }

        public static string ListToString(IEnumerable<string> items)
        {
            var result = new StringBuilder();
            foreach (string item in items)
            {
                result.Append(item + ", ");
            }

            if (result.Length <= 2)
            {
                return "";
            }
            
            //TODO: use StringBuilder.Remove()
            return result.ToString(0,result.Length-2);

        }

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

        public static DateTime? BuildTime
        {
            get
            {
                return InternalLibraryVersion.LastBuildTime;
            }
        }
    }
}
