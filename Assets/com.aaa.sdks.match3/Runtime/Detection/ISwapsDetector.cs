using System.Collections.Generic;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Detection
{
    public interface ISwapsDetector
    {
        public List<MatchGroup> GetAllPossibleSwaps();
        public MatchGroup GetRandomPossibleSwap();
        public int GetPossibleSwapAmount();
        public bool HasAnyPossibleSwapsAtPosition(Vector2Int position);
        public bool HasAnyPossibleSwapsInGrid();
        public bool IsPossibleSwap(Vector2Int position, Vector2Int targetPosition);
    }
}