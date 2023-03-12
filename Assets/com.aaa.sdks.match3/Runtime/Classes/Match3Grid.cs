using System;
using System.Linq;
using AAA.SDKs.Match3.Runtime.Detection;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime
{
    public abstract class Match3Grid<T> : ITileProvider<T>
        where T : ITypeProvider
    {
        private readonly T[,] _grid;
        private readonly Vector2Int _size;
        public event Action OnGridChanged;

        protected Match3Grid(Vector2Int size)
        {
            _grid = new T[size.x, size.y];
            _size = size;
        }

        public void SetTileAt(Vector2Int position, T tile)
            => _grid[position.x, position.y] = tile;

        public void RemoveTileAt(Vector2Int position)
            => _grid[position.x, position.y] = default;

        public Vector2Int GetSize() => _size;
        public T GetTileAt(Vector2Int position) => _grid[position.x, position.y];
        public T[,] GetGrid() => _grid;

        public virtual void InvokeOnGridChanged()
            => OnGridChanged?.Invoke();
    }

    public interface ITileProvider<T>
        where T : ITypeProvider
    {
        public event Action OnGridChanged;
        public Vector2Int GetSize();
        public void SetTileAt(Vector2Int position, T tile);
        void RemoveTileAt(Vector2Int position);
        public T GetTileAt(Vector2Int position);
        public T[,] GetGrid();
        public void InvokeOnGridChanged();
    }
}