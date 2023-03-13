using System;
using AAA.SDKs.Match3.Runtime;
using AAA.SDKs.Match3.Runtime.Matching;
using AAA.SDKs.Match3.Runtime.Refilling;
using UnityEngine;

namespace AAA.Games.FruitMatch3d.Runtime.Model
{
    public class Fruit : ITypeProvider, ISwappable, IDestroyable, IGridPositionProvider, IRefillable, IFallable
    {
        private int _fruitType;
        private Vector2Int _position;

        public event Action<Vector2Int> OnSwapped;
        public event Action<Vector2Int, Vector2Int> OnFalling;
        public event Action OnDestroyed;
        public event Action OnRefilled;

        public int GetTypeID() => _fruitType;
        public void SetTypeID(int value) => _fruitType = value;

        public void SwapToPosition(Vector2Int position)
        {
            _position = position;
            OnSwapped?.Invoke(position);
        }

        public void Destroy() => OnDestroyed?.Invoke();

        public Vector2Int GetPosition() => _position;
        public void SetPosition(Vector2Int value) => _position = value;
        public void Refill() => OnRefilled?.Invoke();

        public void FallToPosition(Vector2Int source, Vector2Int target)
        {
            _position = target;
            OnFalling?.Invoke(source, target);
        }
    }
}