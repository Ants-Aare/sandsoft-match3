using AAA.SDKs.Match3.Runtime;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Swapping
{
    public class OutOfBoundsSwappingCondition<T> : ISwappingCondition
        where T : ITypeProvider
    {
        private readonly ITileProvider<T> _tileProvider;

        public OutOfBoundsSwappingCondition(ITileProvider<T> tileProvider)
        {
            _tileProvider = tileProvider;
        }


        public bool IsSwapPossible(Vector2Int position, Vector2Int targetPosition)
        {
            var size = _tileProvider.GetSize();
            
            if (position.x < 0 || position.x >= size.x)
                return false;
            if (position.y < 0 || position.y >= size.y)
                return false;
            
            if (targetPosition.x < 0 || targetPosition.x >= size.x)
                return false;
            if (targetPosition.y < 0 || targetPosition.y >= size.y)
                return false;

            return true;
        }
    }
}