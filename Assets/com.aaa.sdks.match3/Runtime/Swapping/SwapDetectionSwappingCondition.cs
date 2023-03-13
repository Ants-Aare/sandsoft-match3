using AAA.SDKs.Match3.Runtime.Detection;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Swapping
{
    public class SwapDetectionSwappingCondition : ISwappingCondition
    {
        private readonly ISwapsDetector _swapsDetector;

        public SwapDetectionSwappingCondition(ISwapsDetector swapsDetector)
        {
            _swapsDetector = swapsDetector;
        }

        public bool IsSwapPossible(Vector2Int position, Vector2Int targetPosition)
        {
            var willCreateMatch = _swapsDetector.IsPossibleSwap(position, targetPosition);
            // if(!willCreateMatch)
            //     Debug.Log($"Swapping {position} with {targetPosition} Failed, because tiles will not create a match");
            return willCreateMatch;
        }
    }
}