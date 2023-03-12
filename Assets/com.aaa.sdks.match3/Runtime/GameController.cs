using NaughtyAttributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AAA.SDKs.Match3.Runtime
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] [Expandable] private GameModeBase[] gameModes;
        [SerializeField] private int selectedGameMode;
        private int _activeGameMode;

#if UNITY_EDITOR
        private void OnValidate() => EditorApplication.delayCall += _OnValidate;

        private void _OnValidate()
        {
            selectedGameMode = Mathf.Clamp(selectedGameMode, 0, gameModes.Length - 1);
            
            if (!Application.IsPlaying(this))
                return;

            if (selectedGameMode != _activeGameMode)
            {
                gameModes[_activeGameMode].TearDownGame();
                _activeGameMode = selectedGameMode;
                gameModes[_activeGameMode].StartGame();
            }
        }
#endif

        public void Awake() => _activeGameMode = selectedGameMode;

        public void Start()
            => gameModes[_activeGameMode].StartGame();

        public void Update()
            => gameModes[_activeGameMode].RunUpdateLoop();

        private void OnDestroy()
            => gameModes[_activeGameMode].TearDownGame();
    }
}