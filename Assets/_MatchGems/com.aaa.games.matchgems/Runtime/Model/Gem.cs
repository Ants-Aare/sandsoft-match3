using System;
using AAA.SDKs.Match3.Runtime;
using AAA.SDKs.Match3.Runtime.Refilling;
using AAA.SDKs.Match3.Runtime.Matching;
using UnityEngine;

namespace AAA.Games.MatchGems.Runtime
{
    public class Gem : ITypeProvider, ISwappable, IDestroyable, IGridPositionProvider, IRefillable
    {
        private int _gemType;
        private Vector2Int _gridPosition;

        public event Action<Vector2Int> OnSwapped;
        public event Action OnGemDestroyed;
        public event Action OnGemHighlighted;
        public event Action OnGemRefilled;

        public void Highlight() => OnGemHighlighted?.Invoke();

        public int GetTypeID() => _gemType;
        public void SetTypeID(int value)
        {
            _gemType = value;
        }

        public void SwapToPosition(Vector2Int position)
        {
            _gridPosition = position;
            OnSwapped?.Invoke(position);
        }

        public void Destroy()=> OnGemDestroyed?.Invoke();

        public Vector2Int GetPosition()=> _gridPosition;
        public void SetPosition(Vector2Int value) => _gridPosition = value;
        public void Refill() => OnGemRefilled?.Invoke();
    }
}