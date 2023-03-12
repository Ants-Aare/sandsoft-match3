using System;
using UnityEngine;

namespace AAA.Games.MatchGems.Runtime.Input
{
    public interface ISwapInputProvider
    {
        public event Action<Vector2Int, Vector2Int> OnSwapTiles;
    }
}