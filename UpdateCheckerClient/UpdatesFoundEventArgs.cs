using System;
using System.Collections.Generic;

using SomewhatGeeky.UpdateChecker.Common;

namespace SomewhatGeeky.UpdateChecker.Client
{
    public class UpdatesFoundEventArgs : EventArgs
    {
        List<Update> updates;

        public UpdatesFoundEventArgs(List<Update> updates)
        {
            this.updates = updates;
        }

        public List<Update> Updates
        {
            get
            {
                return new List<Update>(updates);
            }
        }

    }
}
