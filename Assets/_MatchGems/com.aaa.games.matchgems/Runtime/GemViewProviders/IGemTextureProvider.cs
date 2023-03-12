using UnityEngine;

namespace AAA.Games.MatchGems.Runtime.GemViewProviders
{
    public interface IGemTextureProvider
    {
        Sprite GetGemSprite(Gem gem);
        void Initialize();
        void Release();
    }
}