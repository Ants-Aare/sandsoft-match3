using AAA.SDKs.Match3.Runtime;
using UnityEngine;

namespace AAA.Games.FruitMatch3d.Runtime.Model
{
    public class FruitGrid : Match3Grid<Fruit>
    {
        public FruitGrid(Vector2Int size) : base(size) { }
        
    }
}