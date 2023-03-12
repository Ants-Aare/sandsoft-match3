// using System;
// using AAA.SDKs.Match3.Runtime.GridProviders;
// using UnityEngine;
//
// namespace AAA.SDKs.Match3.Runtime.Refilling
// {
//     public class FallingSystem<T> : IRefillingSystem<T>
//         where T : ITypeProvider, IGridPositionProvider, IFallable
//     {
//         public event Action<T> OnNewTileCreated;
//
//         private readonly ITileProvider<T> _tileProvider;
//         private readonly ITileFactory<T> _tileFactory;
//         private readonly IRefillTileProvider<T> _refillTileProvider;
//
//         public RefillingSystem(ITileProvider<T> tileProvider, ITileFactory<T> tileFactory,
//             IRefillTileProvider<T> refillTileProvider)
//         {
//             _tileProvider = tileProvider;
//             _tileFactory = tileFactory;
//             _refillTileProvider = refillTileProvider;
//         }
//
//
//         public void RefillGrid()
//         {
//             var size = _tileProvider.GetSize();
//             var grid = _tileProvider.GetGrid();
//             for (var x = 0; x < size.x; x++)
//             {
//                 bool canFall = false;
//                 for (var y = 0; y < size.y; y++)
//                 {
//                     // if(canFall && )
//                     if (grid[x, y] == null)
//                     {
//                         var position = new Vector2Int(x, y);
//                         var tile = _refillTileProvider.GetRefillTile(_tileFactory);
//                         tile.SetPosition(position);
//                         _tileProvider.SetTileAt(position, tile);
//                         OnNewTileCreated?.Invoke(tile);
//                         // tile.FallToPosition();
//                     }
//                 }
//             }
//         }
//     }
// }