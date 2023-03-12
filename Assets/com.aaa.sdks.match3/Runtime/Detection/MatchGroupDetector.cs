using System.Collections.Generic;
using AAA.Utility.Extensions;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Detection
{
    public class MatchGroupDetector<T> : IMatchDetector
        where T : ITypeProvider
    {
        private ITileProvider<T> _tileProvider;

        public MatchGroupDetector(ITileProvider<T> types)
        {
            _tileProvider = types;
        }

        public List<MatchGroup> GetAllMatchGroups()
        {
            var verticalGroups = GetMatchGroups(true);
            var horizontalGroups = GetMatchGroups(false);

            List<MatchGroup> matchGroups;
            if (horizontalGroups.Count == 0)
                matchGroups = verticalGroups;
            else if (verticalGroups.Count == 0)
                matchGroups = horizontalGroups;
            else
                matchGroups = CombineBidirectionalGroups(horizontalGroups, verticalGroups);

            return matchGroups;
        }

        public bool HasAnyMatchGroups()
        {
            if (HasMatchGroupsInDirection(true))
                return true;
            if (HasMatchGroupsInDirection(false))
                return true;
            return false;
        }

        List<MatchGroup> GetMatchGroups(bool isVertical)
        {
            var matchGroups = new List<MatchGroup>();
            var tileGridSize = _tileProvider.GetSize();
            tileGridSize = isVertical ? tileGridSize : tileGridSize.SwapAxis();
            for (var x = 0; x < tileGridSize.x; x++)
            {
                //Todo: maybe object pool these...
                var currentMatchGroup = new MatchGroup(-1);
                for (var y = 0; y < tileGridSize.y; y++)
                {
                    var currentPosition = isVertical ? new Vector2Int(x, y) : new Vector2Int(y, x);
                    var tile = _tileProvider.GetTileAt(currentPosition);
                    var type = tile != null
                        ? tile.GetTypeID()
                        : -1;

                    if (type < 0)
                    {
                        if (IsMatchGroupValid(currentMatchGroup))
                            matchGroups.Add(currentMatchGroup);
                        currentMatchGroup = new MatchGroup(-1);
                        continue;
                    }

                    if (type != currentMatchGroup.Type)
                    {
                        if (IsMatchGroupValid(currentMatchGroup))
                            matchGroups.Add(currentMatchGroup);

                        currentMatchGroup = new MatchGroup(type);
                        currentMatchGroup.Add(currentPosition);
                    }
                    else
                    {
                        currentMatchGroup.Add(currentPosition);
                    }
                }

                if (IsMatchGroupValid(currentMatchGroup))
                    matchGroups.Add(currentMatchGroup);
            }

            return matchGroups;
        }

        static List<MatchGroup> CombineBidirectionalGroups(List<MatchGroup> horizontalGroups,
            List<MatchGroup> verticalGroups)
        {
            for (int i = horizontalGroups.Count - 1; i >= 0; i--)
            {
                for (int j = verticalGroups.Count - 1; j >= 0; j--)
                {
                    if (verticalGroups[j].Positions.Overlaps(horizontalGroups[i].Positions))
                    {
                        verticalGroups[j].Positions.UnionWith(horizontalGroups[i].Positions);
                        horizontalGroups.RemoveAt(i);
                        break;
                    }
                }
            }

            verticalGroups.AddRange(horizontalGroups);

            return verticalGroups;
        }

        bool HasMatchGroupsInDirection(bool isVertical)
        {
            var tileGridSize = _tileProvider.GetSize();
            tileGridSize = isVertical ? tileGridSize : tileGridSize.SwapAxis();

            for (var x = 0; x < tileGridSize.x; x++)
            {
                var currentMatchGroup = new MatchGroup(-1);
                for (var y = 0; y < tileGridSize.y; y++)
                {
                    var currentPosition = isVertical ? new Vector2Int(x, y) : new Vector2Int(y, x);
                    var type = _tileProvider.GetTileAt(currentPosition).GetTypeID();

                    if (type < 0)
                    {
                        if (IsMatchGroupValid(currentMatchGroup))
                            return true;
                        currentMatchGroup = new MatchGroup(-1);
                        continue;
                    }

                    if (type == currentMatchGroup.Type)
                    {
                        currentMatchGroup.Add(currentPosition);
                    }
                    else
                    {
                        currentMatchGroup = new MatchGroup(type);
                        currentMatchGroup.Add(currentPosition);
                    }

                    if (IsMatchGroupValid(currentMatchGroup))
                        return true;
                }

                if (IsMatchGroupValid(currentMatchGroup))
                    return true;
            }

            return false;
        }

        private bool IsMatchGroupValid(MatchGroup currentMatchGroup)
            => currentMatchGroup.Positions.Count >= 3; // maybe serialize this value?
    }
}