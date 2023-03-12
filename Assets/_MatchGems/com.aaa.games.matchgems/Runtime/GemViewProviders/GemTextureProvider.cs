using UnityEngine;

namespace AAA.Games.MatchGems.Runtime.GemViewProviders
{
    public abstract class GemTextureProvider : ScriptableObject
        , IGemTextureProvider
    {
        public abstract Sprite GetGemSprite(Gem gem);

        public virtual void Initialize() { }
        public virtual void Release() { }
    }
}