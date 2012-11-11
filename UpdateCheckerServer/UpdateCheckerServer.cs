using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using SomewhatGeeky.UpdateChecker.Common;

namespace SomewhatGeeky.UpdateChecker.Server
{
    public class UpdateCheckerServer
    {
        private List<Update> allUpdates;
        public UpdateCheckerServer()
        {
            allUpdates = new List<Update>();
        }

        public void AddAvailableUpdate(Update update)
        {
            allUpdates.Add(update);
        }
        public void AddAvailableUpdate(string projectName, Version version, bool? stable, string description, string informationUrl)
        {
            AddAvailableUpdate(new Update(projectName, version, stable, description, informationUrl));
        }

        #region update selecting
        private List<Update> getByProject(string projectName)
        {
            List<Update> result = new List<Update>();
            foreach (Update update in allUpdates)
                if (update.ProjectName.Equals(projectName))
                    result.Add(update);
            return result;
        }

        public List<Update> getAvailableUpdates(string projectName, bool? stableOnly, Version versionToBeat)
        {
            List<Update> result = new List<Update>();
            foreach (Update update in getByProject(projectName))
                if (Common.Common.XIsLaterThanY(update.Version, versionToBeat) && ((stableOnly != true) || (update.Stable == true)) )
                    result.Add(update);
            return result;
        }
        #endregion

        public void processRequest(System.Web.HttpRequest request, System.Web.HttpResponse response)
        {
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(request.InputStream);
            //XmlNode rootNode = xmlDoc.ChildNodes[xmlDoc.ChildNodes.Count - 1];

            string projectName = request.QueryString["ProjectName"];
            Version projectVersion = (!String.IsNullOrEmpty(request.QueryString["ProjectVersion"])) ? (new Version(request.QueryString["ProjectVersion"])) : (null);
            bool? stableUpdatesOnly = (!String.IsNullOrEmpty(request.QueryString["StableUpdatesOnly"])) ? ((bool?)(System.Convert.ToBoolean(request.QueryString["StableUpdatesOnly"]))) : (null);

            if (string.IsNullOrEmpty(projectName))
                throw new Exception("ProjectName is not set.");
            if (projectVersion == null)
                throw new Exception("Version not set.");

            List<Update> availableUpdates = getAvailableUpdates(projectName, stableUpdatesOnly, projectVersion);

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = Encoding.UTF8;
            writerSettings.OmitXmlDeclaration = false;

            XmlWriter writer = XmlWriter.Create(response.Output, writerSettings);
            writer.WriteStartDocument();
            writer.WriteStartElement("UpdateChecker");
            foreach (Update update in availableUpdates)
                update.WriteToXml(writer);
            writer.WriteEndDocument();
            writer.Close();
        }

        
    }
}
