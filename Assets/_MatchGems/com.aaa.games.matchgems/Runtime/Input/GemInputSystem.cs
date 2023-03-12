using System;
using AAA.SDKs.Match3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AAA.Games.MatchGems.Runtime.Input
{
    public class GemInputSystem : IDragInputReceiver, ISwapInputProvider
    {
        public event Action<Vector2Int, Vector2Int> OnSwapTiles;
        
        private readonly float _minimumDragDistance;
        private readonly float _endDragDistance;
        private bool _hasTileSelected;
        private int _selectingPointerId;
        private Vector2Int _selectedTilePosition;

        public GemInputSystem(float minimumDragDistance, float endDragDistance = 300f)
        {
            _minimumDragDistance = minimumDragDistance;
            _endDragDistance = endDragDistance;
        }
        
        public void ReceiveOnBeginDrag(PointerEventData eventData, IGridPositionProvider gridPositionProvider)
        {
            if (_hasTileSelected)
                return;
            
            _selectingPointerId = eventData.pointerId;
            _hasTileSelected = true;
            _selectedTilePosition = gridPositionProvider.GetPosition();
        }

        public void ReceiveOnDrag(PointerEventData eventData, IGridPositionProvider gridPositionProvider)
        {
            if (eventData.pointerId != _selectingPointerId)
                return;
            
            var dragDirection = eventData.position - eventData.pressPosition;

            if(_hasTileSelected && dragDirection.sqrMagnitude > _endDragDistance * _endDragDistance)
                EndDrag(eventData);
        }

        public void ReceiveOnEndDrag(PointerEventData eventData, IGridPositionProvider gridPositionProvider)
        {
            if (eventData.pointerId != _selectingPointerId)
                return;

            if(_hasTileSelected)
                EndDrag(eventData);
        }

        private void EndDrag(PointerEventData eventData)
        {
            _hasTileSelected = false;

            var dragDirection = eventData.position - eventData.pressPosition;

            if (dragDirection.sqrMagnitude < _minimumDragDistance * _minimumDragDistance)
                return;

            var targetTilePosition = GetTargetTilePosition(dragDirection);

            OnSwapTiles?.Invoke(_selectedTilePosition, targetTilePosition);
        }

        private Vector2Int GetTargetTilePosition(Vector2 dragDirection)
        {
            var direction = ToCardinalDirection(dragDirection);
            var targetTilePosition = _selectedTilePosition + direction;
            return targetTilePosition;
        }

        public Vector2Int ToCardinalDirection(Vector2 value)
        {
            value = value.normalized;
            var dotProductUp = Vector2.Dot(Vector2.up, value);

            if (dotProductUp >= 0.707f)
                return Vector2Int.up;
            if (dotProductUp <= -0.707f)
                return Vector2Int.down;

            var dotProductRight = Vector2.Dot(Vector2.right, value);
            
            if (dotProductRight >= 0.707f)
                return Vector2Int.right;
            if (dotProductRight <= -0.707f)
                return Vector2Int.left;

            return Vector2Int.up;
        }
    }
}