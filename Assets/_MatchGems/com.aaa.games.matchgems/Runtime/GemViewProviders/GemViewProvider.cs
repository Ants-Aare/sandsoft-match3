using AAA.Games.MatchGems.Runtime.Views;
using UnityEngine;

namespace AAA.Games.MatchGems.Runtime.GemViewProviders
{
    public abstract class GemViewProvider : ScriptableObject, IGemViewProvider
    {
        public abstract void Initialize();
        public abstract void Release();
        public abstract GemView GetGemView(Gem gem);
        public abstract void ReturnGemView(GemView view);
    }
}