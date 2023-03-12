using System;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Swapping
{
    public interface ISwappingSystem
    {
        public event Action OnFailedSwap;
        public event Action OnSuccessfulSwap;
        void TrySwapTiles(Vector2Int position, Vector2Int targetPosition);
    }
}