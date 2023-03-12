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
            => _swapsDetector.IsPossibleSwap(position, targetPosition);
    }
}