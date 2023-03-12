using System.Collections.Generic;
using AAA.Utility.Extensions;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Detection
{
    public class SwapsDetector<T> :ISwapsDetector
        where T : ITypeProvider
    {
        private IMatchDetector _matchDetector;
        private static readonly Vector2Int[] DirectionVectors = { new(0, 1), new(1, 0) };

        private ITileProvider<T> _tileProvider;

        public SwapsDetector(ITileProvider<T> tileProvider, IMatchDetector matchDetector)
        {
            _tileProvider = tileProvider;
            _matchDetector = matchDetector;
        }
        
        public bool HasAnyPossibleSwapsInGrid()
        {
            if (_matchDetector.HasAnyMatchGroups())
                return true;

            var size = _tileProvider.GetSize();
            for (var x = 0; x < size.x - 1; x++)
            {
                for (var y = 0; y < size.y - 1; y++)
                {
                    if (HasAnyPossibleSwapsAtPosition(new Vector2Int(x, y)))
                        return true;
                }
            }

            return false;
        }
    
        public int GetPossibleSwapAmount()
        {
            if (_matchDetector.HasAnyMatchGroups())
                return -1;

            var value = 0;
            var size = _tileProvider.GetSize();
            for (var x = 0; x < size.x - 1; x++)
            {
                for (var y = 0; y < size.y - 1; y++)
                {
                    if (HasAnyPossibleSwapsAtPosition(new Vector2Int(x, y)))
                        value++;
                }
            }

            return value;
        }

        public List<MatchGroup> GetAllPossibleSwaps()
        {
            var matchGroups = new List<MatchGroup>();
            var size = _tileProvider.GetSize();
            for (var x = 0; x < size.x - 1; x++)
            {
                for (var y = 0; y < size.y - 1; y++)
                {
                    GetPossibleSwapsAtPosition(new Vector2Int(x, y), matchGroups);
                }
            }
            return matchGroups;
        }
        public MatchGroup GetRandomPossibleSwap()
        {
            var matchGroups = GetAllPossibleSwaps();
            var random = Random.Range(0, matchGroups.Count);
            matchGroups.TryGetClamped(random, out var maxGroup);
            return maxGroup;
        }
        public bool IsPossibleSwap(Vector2Int position, Vector2Int targetPosition)
        {
            var grid = _tileProvider.GetGrid();
            
            (grid[position.x, position.y], grid[targetPosition.x, targetPosition.y]) = (
                grid[targetPosition.x, targetPosition.y], grid[position.x, position.y]);

            var hasMatchGroups = _matchDetector.HasAnyMatchGroups();
            (grid[position.x, position.y], grid[targetPosition.x, targetPosition.y]) = (
                grid[targetPosition.x, targetPosition.y], grid[position.x, position.y]);

            return hasMatchGroups;
        }

        public bool HasAnyPossibleSwapsAtPosition(Vector2Int position)
        {
            var currentType = _tileProvider.GetTileAt(position).GetTypeID();
            if (currentType < 0)
                return false;

            foreach (var directionVector in DirectionVectors)
            {
                var targetPosition = directionVector + position;
                var otherType = _tileProvider.GetTileAt(targetPosition).GetTypeID();

                if (otherType < 0)
                    continue;
                if (currentType == otherType)
                    continue;

                if (IsPossibleSwap(position, targetPosition))
                    return true;
            }

            return false;
        }

        void GetPossibleSwapsAtPosition(Vector2Int position, List<MatchGroup> matchGroups)
        {
            var currentType = _tileProvider.GetTileAt(position).GetTypeID();
            if (currentType < 0)
                return;

            foreach (var directionVector in DirectionVectors)
            {
                var targetPosition = directionVector + position;
                var targetType = _tileProvider.GetTileAt(targetPosition).GetTypeID();

                if (targetType < 0)
                    continue;
                if (currentType == targetType)
                    continue;

                GetMatchGroupFromPossibleSwap(position, targetPosition, matchGroups);
            }
        }

        void GetMatchGroupFromPossibleSwap(Vector2Int position, Vector2Int targetPosition, List<MatchGroup> groups)
        {
            var grid = _tileProvider.GetGrid();
            (grid[position.x, position.y], grid[targetPosition.x, targetPosition.y]) = (
                grid[targetPosition.x, targetPosition.y], grid[position.x, position.y]);

            var matchGroups = _matchDetector.GetAllMatchGroups();
            
            (grid[position.x, position.y], grid[targetPosition.x, targetPosition.y]) = (
                grid[targetPosition.x, targetPosition.y], grid[position.x, position.y]);

            foreach (var matchGroup in matchGroups)
            {
                if (matchGroup.Positions.Contains(position))
                {
                    matchGroup.Positions.Remove(position);
                    matchGroup.Positions.Add(targetPosition);
                }
                else if (matchGroup.Positions.Contains(targetPosition))
                {
                    matchGroup.Positions.Remove(targetPosition);
                    matchGroup.Positions.Add(position);
                }
            }

            groups.AddRange(matchGroups);
        }
    }
}