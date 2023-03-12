using System;
using System.Linq;
using AAA.SDKs.Match3.Runtime;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.Swapping
{
    public class SwappingSystem<T> : ISwappingSystem
        where T : ISwappable, ITypeProvider
    {
        private readonly ISwappingCondition[] _swappingConditions;
        private readonly ITileProvider<T> _tileProvider;
        public event Action OnSuccessfulSwap;
        public event Action OnFailedSwap;

        public SwappingSystem(ITileProvider<T> tileProvider, params ISwappingCondition[] swappingConditions)
        {
            _swappingConditions = swappingConditions;
            _tileProvider = tileProvider;
        }

        public void TrySwapTiles(Vector2Int position, Vector2Int targetPosition)
        {
            if (_swappingConditions.Any(swappingCondition => !swappingCondition.IsSwapPossible(position, targetPosition)))
            {
                OnFailedSwap?.Invoke();
                return;
            }

            _tileProvider.GetTileAt(position).SwapToPosition(targetPosition);
            _tileProvider.GetTileAt(targetPosition).SwapToPosition(position);
            
            var typeProviders = _tileProvider.GetGrid();
            (typeProviders[position.x, position.y], typeProviders[targetPosition.x, targetPosition.y]) =
                (typeProviders[targetPosition.x, targetPosition.y], typeProviders[position.x, position.y]);

            _tileProvider.InvokeOnGridChanged();
            OnSuccessfulSwap?.Invoke();
        }
    }
}