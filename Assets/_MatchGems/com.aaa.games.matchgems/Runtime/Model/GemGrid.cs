using System.Collections.Generic;
using AAA.SDKs.Match3.Runtime;
using UnityEngine;
// ReSharper disable CoVariantArrayConversion

namespace AAA.Games.MatchGems.Runtime
{
    public class GemGrid : Match3Grid<Gem>
    {
        public GemGrid(Vector2Int size) : base(size) { }
        
        public void HighlightGems(IEnumerable<Vector2Int> positions)
        {
            foreach (var position in positions)
            {
                GetGrid()[position.x, position.y].Highlight();
            }
        }
    }
}