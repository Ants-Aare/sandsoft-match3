using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Swapping
{
    public class AdjacentSwappingCondition : ISwappingCondition
    {
        public bool IsSwapPossible(Vector2Int position, Vector2Int targetPosition)
        {
            var positionDelta = targetPosition - position;
            
            return Mathf.Abs(positionDelta.x) + Mathf.Abs(positionDelta.y) == 1;
        }
    }
}