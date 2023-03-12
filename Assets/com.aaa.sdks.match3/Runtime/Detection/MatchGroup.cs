using System.Collections.Generic;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Detection
{
    public class MatchGroup
    {
        public readonly HashSet<Vector2Int> Positions;
        public readonly int Type;

        public MatchGroup(int type)
        {
            Positions = new HashSet<Vector2Int>();
            Type = type;
        }

        public void Add(Vector2Int position) => Positions.Add(position);
    }
}