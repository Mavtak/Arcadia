using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Xml;
using SomewhatGeeky.UpdateChecker.Common;

namespace SomewhatGeeky.UpdateChecker.Client
{
    public delegate void UpdatesFoundEventHandler(object sender, UpdatesFoundEventArgs eventArgs);
    public delegate void UpdateCheckFailedEventHandler(object sender, EventArgs eventArgs);

    public class UpdaterClient
    {
        private List<string> checkUrls;
        private string projectName;
        private Version version;
        private bool? stable;

        public event UpdatesFoundEventHandler UpdateFound;
        public event UpdateCheckFailedEventHandler UpdateCheckFailed;

        public UpdaterClient(ICollection<string>checkUrls, string projectName, Version version, bool? stable)
        {
            this.checkUrls = new List<string>(checkUrls);
            this.projectName = projectName;
            this.version = version;
            this.stable = stable;
        }

        public UpdaterClient(string[] checkUrls, string projectName, Version version, bool? stable)
            : this(new List<string>(checkUrls), projectName, version, stable)
        {
        }

        public List<Update> CheckForUpdates()
        {
            List<Update> result = new List<Update>();

            bool succeed = false;
            int urlIndex = 0;

            while (!succeed && urlIndex < checkUrls.Count)
            {

                try
                {
                    XmlDocument xmlDocument = new XmlDocument();

                    xmlDocument.Load(checkUrls[urlIndex] + "?ProjectName=" + projectName + "&ProjectVersion=" + version + "&StableUpdatesOnly=" + stable);
                    XmlNode rootNode = xmlDocument.ChildNodes[xmlDocument.ChildNodes.Count - 1];

                    foreach (XmlNode updateNode in rootNode.SelectNodes("Update"))
                    {
                        result.Add(Update.ReadFromXml(updateNode));
                    }

                    succeed = true;
                }
                catch (WebException) { }
                catch (XmlException) { }

                urlIndex++;
                //result.Add(new Update("Arcadia Desktop", new Version("1.2.3.4"), true, "lol!  This is a description", "http://davidmcgrath.com/lol"));
            }

            if (!succeed)
            {
                UpdateCheckFailed(this, new EventArgs());
            }

            return result;
        }

        private void checkForUpdatesAsyncHelper()
        {
            List<Update> result = CheckForUpdates();
            if(result != null && result.Count>0)
                UpdateFound(this, new UpdatesFoundEventArgs(result));
        }

        public void CheckForUpdatesAsync()
        {
            Thread thread = new Thread(new ThreadStart(checkForUpdatesAsyncHelper));
            thread.Start();

        }
    }
}
