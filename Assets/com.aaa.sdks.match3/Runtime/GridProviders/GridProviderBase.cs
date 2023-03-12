using AAA.SDKs.Match3.Runtime;
using AAA.SDKs.Match3.Runtime.Detection;
using UnityEngine;

namespace AAA.SDKs.Match3.Runtime.GridProviders
{
    public abstract class GridProviderBase<T> : ScriptableObject, IItemGridProvider<T>, IRefillTileProvider<T>
        where T : ITypeProvider
    {
        public abstract void Initialize(IMatchDetector matchDetector);
        public abstract Vector2Int GetSize();
        public abstract void PopulateGrid(ITileProvider<T> tileProvider, ITileFactory<T> tileFactory);
        public abstract T GetRefillTile(ITileFactory<T> tileFactory);
    }

    public interface IRefillTileProvider<T>
    {
        public T GetRefillTile(ITileFactory<T> tileFactory);
    }
}