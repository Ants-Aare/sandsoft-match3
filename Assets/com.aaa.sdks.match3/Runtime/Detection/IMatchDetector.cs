using System.Collections.Generic;

namespace AAA.SDKs.Match3.Runtime.Detection
{
    public interface IMatchDetector
    {
        public List<MatchGroup> GetAllMatchGroups();
        public bool HasAnyMatchGroups();

    }
}