using AAA.Games.FruitMatch3d.Runtime.Model;
using AAA.SDKs.Match3.Runtime;

namespace AAA.Games.FruitMatch3d.Runtime
{
    public class FruitFactory : ITileFactory<Fruit>
    {
        public Fruit GetTile() => new Fruit();
    }
}