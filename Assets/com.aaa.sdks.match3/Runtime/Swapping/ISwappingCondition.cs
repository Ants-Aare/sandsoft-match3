using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Swapping
{
    public interface ISwappingCondition
    {
        bool IsSwapPossible(Vector2Int position, Vector2Int targetPosition);
    }
}