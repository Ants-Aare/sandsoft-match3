using UnityEngine;

namespace AAA.SDKs.Match3.Runtime
{
    public abstract class GameModeBase : ScriptableObject
    {
        public abstract void StartGame();
        public abstract void TearDownGame();

        public virtual void RunUpdateLoop()
        {
        }
    }
}