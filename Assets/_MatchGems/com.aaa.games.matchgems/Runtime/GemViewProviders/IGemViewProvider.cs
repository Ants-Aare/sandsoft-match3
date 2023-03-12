using AAA.Games.MatchGems.Runtime.Views;

namespace AAA.Games.MatchGems.Runtime.GemViewProviders
{
    public interface IGemViewProvider
    {
        void Initialize();
        void Release();
        GemView GetGemView(Gem gem);
        void ReturnGemView(GemView view);
    }
}