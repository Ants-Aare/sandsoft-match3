using System;

namespace AAA.SDKs.Match3.Runtime.Matching
{
    public interface IMatchingSystem
    {
        public event Action OnMatched;
        public void ResolveMatches();
    }
}