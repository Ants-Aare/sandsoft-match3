using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Refilling
{
    public interface IFallable
    {
        public void FallToPosition(Vector2Int position);
    }
}