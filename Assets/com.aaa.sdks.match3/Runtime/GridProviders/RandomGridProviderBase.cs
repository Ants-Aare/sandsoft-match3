using AAA.SDKs.Match3.Runtime;
using AAA.SDKs.Match3.Runtime.Detection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AAA.SDKs.Match3.Runtime.GridProviders
{
    public class RandomGridProviderBase<T> : GridProviderBase<T>
        where T : ITypeProvider, IGridPositionProvider
    {
        [SerializeField] private Vector2Int size;
        [SerializeField] private int flavors;
        IMatchDetector _matchDetector;

        public override void Initialize(IMatchDetector matchDetector)
        {
            _matchDetector = matchDetector;
        }

        public override Vector2Int GetSize() => size;
        
        public override void PopulateGrid(ITileProvider<T> tileProvider, ITileFactory<T> tileFactory)
        {
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    var position = new Vector2Int(x, y);
                    var tile = tileFactory.GetTile();
                    tile.SetTypeID(Random.Range(0, flavors));
                    tile.SetPosition(position);
                    tileProvider.SetTileAt(position, tile);
                }
            }

            for (var i = 0; i < 10; i++)
            {
                var matchGroups = _matchDetector.GetAllMatchGroups();

                if (matchGroups.Count == 0)
                    break;
                
                foreach (var matchGroup in matchGroups)
                {
                    foreach (var position in matchGroup.Positions)
                    {
                        var tile = tileProvider.GetTileAt(position);
                        tile.SetTypeID(Random.Range(0, flavors));
                    }
                }
            }
        }

        public override T GetRefillTile(ITileFactory<T> tileFactory)
        {
            var tile = tileFactory.GetTile();
            tile.SetTypeID(Random.Range(0, flavors));
            return tile;
        }
    }
}