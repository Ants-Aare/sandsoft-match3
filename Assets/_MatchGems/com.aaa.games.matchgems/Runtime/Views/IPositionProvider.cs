using UnityEngine;

namespace AAA.Games.MatchGems.Runtime
{
    public interface IPositionProvider
    {
        public Vector3 GetPosition(Vector2Int gridPosition);
    }
}