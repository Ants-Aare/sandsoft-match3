using System;

namespace AAA.SDKs.Match3.Runtime.Refilling
{
    public interface IRefillingSystem<T>
    {
        public event Action<T> OnNewTileCreated;
        void RefillGrid();
    }
}