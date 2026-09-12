using System.Collections.Generic;

namespace YuetanMingqu
{
    [System.Serializable]
    public class BandNode
    {
        public string id;

        public string band;

        public string leader;

        public string leaderId;

        public string layer;

        public int nodeIndex;

        public int unlockDay;

        public string defaultState;
    }

    [System.Serializable]
    public class BandNodeDatabase
    {
        public List<BandNode> bandNodes;
    }
}

