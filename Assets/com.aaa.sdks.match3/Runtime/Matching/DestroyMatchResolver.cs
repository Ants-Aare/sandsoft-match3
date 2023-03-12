using AAA.SDKs.Match3.Runtime.Detection;

namespace AAA.SDKs.Match3.Runtime.Matching
{
    public class DestroyMatchResolver<T> : IMatchResolver<T>
        where T : ITypeProvider, IDestroyable
    {
        public void ResolveMatchGroup(MatchGroup matchGroup, ITileProvider<T> tileProvider)
        {
            foreach (var position in matchGroup.Positions)
            {
                var tile = tileProvider.GetTileAt(position);
                tile.Destroy();
                tileProvider.RemoveTileAt(position);
                tileProvider.InvokeOnGridChanged();
            }
        }
    }
}