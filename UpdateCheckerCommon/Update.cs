using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.UpdateChecker.Common
{
    public class Update
    {
        private string projectName;
        private Version version;
        private bool? stable;
        private string description;
        private string informationUrl;

        public Update(string projectName, Version version, bool? stable, string description, string informationUrl)
        {
            this.projectName = projectName;
            this.version = version;
            this.stable = stable;
            this.description = description;
            this.informationUrl = informationUrl;
        }

        public static Update ReadFromXml(XmlNode updateNode)
        {
            if (updateNode == null)
                return null;

            string projectName = (updateNode.Attributes["ProjectName"] != null) ? (updateNode.Attributes["ProjectName"].Value) : (null);
            Version version = (updateNode.Attributes["ProjectVersion"] != null) ? (new System.Version(updateNode.Attributes["ProjectVersion"].Value)) : (null);
            bool? stable = (updateNode.Attributes["Stable"] != null) ? ((bool?)(System.Convert.ToBoolean(updateNode.Attributes["Stable"].Value))) : (null);
            string description = (updateNode.Attributes["Description"] != null) ? (updateNode.Attributes["Description"].Value) : (null);
            string informationUrl = (updateNode.Attributes["InformationUrl"] != null) ? (updateNode.Attributes["InformationUrl"].Value) : (null);

            //if(description == null)
            //    description = "Error: description not included with the update information";

            //if(version == null)
            //    description = "\n\nError: version not included with the update information";

            //if(url == null)
            //    description = "\n\nError: Download URL not included with the update information";

            return new Update(projectName, version, stable, description, informationUrl);
        }

        public void WriteToXml(XmlWriter writer, string nodeName)
        {
            writer.WriteStartElement(nodeName);

            if (projectName != null)
                writer.WriteAttributeString("ProjectName", projectName);
            if (version != null)
                writer.WriteAttributeString("ProjectVersion", version.ToString());
            if (Stable != null)
                writer.WriteAttributeString("Stable", stable.ToString());
            if (description != null)
                writer.WriteAttributeString("Description", description);
            if (informationUrl != null)
                writer.WriteAttributeString("InformationUrl", informationUrl);

            writer.WriteEndElement();
        }
        public void WriteToXml(XmlWriter writer)
        {
            WriteToXml(writer, DefaultXmlNodeName);
        }
        public string DefaultXmlNodeName
        {
            get
            {
                return "Update";
            }
        }

        public string ProjectName
        {
            get
            {
                return projectName;
            }
        }

        public Version Version
        {
            get
            {
                return version;
            }
        }

        public bool? Stable
        {
            get
            {
                return stable;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public string InformationUrl
        {
            get
            {
                return informationUrl;
            }
        }


    }
}
