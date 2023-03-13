using System;
using AAA.Games.MatchGems.Runtime.Input;
using AAA.Utility.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AAA.Games.FruitMatch3d.Runtime
{
    public class FruitMatch3dInputSystem : ISwapInputProvider
    {
        private readonly float _minimumDragDistance;
        private Plane _plane;
        private static Camera _camera;
        public event Action<Vector2Int, Vector2Int> OnSwapTiles;
        public Vector2Int _selectedTile;
        private Vector3 _startPosition;

        public FruitMatch3dInputSystem(float minimumDragDistance)
        {
            _minimumDragDistance = minimumDragDistance;
            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;
        }
        public void RunUpdateLoop()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var viewPortPosition = ScreenUtility.ScreenToViewPort(Input.mousePosition);
                _startPosition = GetPointOnTileGridPlane(viewPortPosition);
                _selectedTile = _startPosition.ToVector2XZ().ToVector2Int();
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                var viewPortPosition = ScreenUtility.ScreenToViewPort(Input.mousePosition);
                var endPosition = GetPointOnTileGridPlane(viewPortPosition);
                var delta = (endPosition - _startPosition).ToVector2XZ();
                if (delta.sqrMagnitude < _minimumDragDistance * _minimumDragDistance)
                    return;

                var targetTilePosition = GetTargetTilePosition(delta);

                OnSwapTiles?.Invoke(_selectedTile, targetTilePosition);
            }
        }
        
        public Vector3 GetPointOnTileGridPlane(Vector3 viewPortPosition)
        {
            var ray = _camera.ViewportPointToRay(viewPortPosition);
            _plane.Raycast(ray, out var enter);
            var hitPoint = ray.GetPoint(enter);
            Debug.DrawRay(hitPoint, Vector3.up, Color.red, 1f);
            return hitPoint + new Vector3(0.5f, 0, 0.5f);
        }
        
        private Vector2Int GetTargetTilePosition(Vector2 dragDirection)
        {
            var direction = ToCardinalDirection(dragDirection);
            var targetTilePosition = _selectedTile + direction;
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