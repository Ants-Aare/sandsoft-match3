using UnityEngine;

namespace AAA.SDKs.Match3.Runtime
{
    public interface IItemGridProvider<T> where T : ITypeProvider
    {
        public Vector2Int GetSize();
        public void PopulateGrid(ITileProvider<T> tileProvider, ITileFactory<T> tileFactory);
    }
}