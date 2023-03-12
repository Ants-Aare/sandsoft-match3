using UnityEngine;

namespace AAA.SDKs.Match3.Runtime
{
    public interface IGridPositionProvider
    {
        public Vector2Int GetPosition();
        public void SetPosition(Vector2Int value);
    }
}