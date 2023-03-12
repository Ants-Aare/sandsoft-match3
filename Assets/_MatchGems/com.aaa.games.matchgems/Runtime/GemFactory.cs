using AAA.SDKs.Match3.Runtime;

namespace AAA.Games.MatchGems.Runtime
{
    public class GemFactory : ITileFactory<Gem>
    {
        public Gem GetTile() => new Gem();
    }
}