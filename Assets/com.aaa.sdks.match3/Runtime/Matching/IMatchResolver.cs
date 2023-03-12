using AAA.SDKs.Match3.Runtime.Detection;

namespace AAA.SDKs.Match3.Runtime.Matching
{
    public interface IMatchResolver<T>
        where T : ITypeProvider
    {
        public void ResolveMatchGroup(MatchGroup matchGroup, ITileProvider<T> tileProvider);
    }
}