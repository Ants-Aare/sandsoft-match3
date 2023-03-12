using System;
using AAA.SDKs.Match3.Runtime.Detection;

namespace AAA.SDKs.Match3.Runtime.Matching
{
    public class MatchingSystem<T> : IMatchingSystem
        where T : ITypeProvider
    {
        public event Action OnMatched;
        private readonly ITileProvider<T> _tileProvider;
        private readonly IMatchDetector _matchDetector;
        private readonly IMatchResolver<T>[] _matchResolvers;

        public MatchingSystem(ITileProvider<T> tileProvider, IMatchDetector matchDetector, params IMatchResolver<T>[] matchResolvers)
        {
            _tileProvider = tileProvider;
            _matchDetector = matchDetector;
            _matchResolvers = matchResolvers;
        }

        public void ResolveMatches()
        {
            var matchGroups = _matchDetector.GetAllMatchGroups();

            if (matchGroups.Count == 0)
                return;
            
            foreach (var matchResolver in _matchResolvers)
                foreach (var matchGroup in matchGroups)
                    matchResolver.ResolveMatchGroup(matchGroup, _tileProvider);
            
            OnMatched?.Invoke();
        }
    }
}