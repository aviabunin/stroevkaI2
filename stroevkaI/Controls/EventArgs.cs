using System;

namespace stroevkaI.Controls
{
    public class PsgSelectedEventArgs : EventArgs
    {
        public string PsgName { get; }

        public PsgSelectedEventArgs(string psgName)
        {
            PsgName = psgName;
        }
    }

    public class PchSelectedEventArgs : EventArgs
    {
        public int PchId { get; }
        public string PchName { get; }

        public PchSelectedEventArgs(int pchId, string pchName)
        {
            PchId = pchId;
            PchName = pchName;
        }
    }
}
