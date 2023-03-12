using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AAA.Games.MatchGems.Runtime.GemViewProviders
{
    [CreateAssetMenu(menuName = "AddressableGemTextureProvider", fileName = "GemTextureProvider", order = 0)]
    public class AddressableGemTextureProvider : GemTextureProvider
    {
        [SerializeField] private AssetReferenceSprite[] sprites;
        private AsyncOperationHandle<Sprite>[] _operationHandles;
        private bool _isInitialised = false;
        
        private void OnEnable() => _isInitialised = false;
        private void OnDisable() => _isInitialised = false;

        public override void Initialize()
        {
            if (_isInitialised)
                return;
            
            _operationHandles = new AsyncOperationHandle<Sprite>[sprites.Length];
            for (var i = 0; i < sprites.Length; i++)
            {
                _operationHandles[i] = sprites[i].LoadAssetAsync();
            }

            _isInitialised = true;
        }

        public override void Release()
        {
            _isInitialised = false;
            for (var i = 0; i < _operationHandles.Length; i++)
            {
                if(_operationHandles[i].IsValid())
                    Addressables.Release(_operationHandles[i]);
            }
        }

        public override Sprite GetGemSprite(Gem gem)
        {
            var value = Math.Clamp(gem.GetTypeID(), 0, sprites.Length);

            return _operationHandles[value].WaitForCompletion();
        }
    }
}