using AAA.Games.MatchGems.Runtime.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace AAA.Games.MatchGems.Runtime.GemViewProviders
{
    [CreateAssetMenu(menuName = "Create GemViewProvider", fileName = "GemViewProvider", order = 0)]
    class AddressableGemViewProvider : GemViewProvider
    {
        [FormerlySerializedAs("gemViewPrefab")] [SerializeField]
        private AssetReferenceGameObject gemViewReference;

        [SerializeField] private GemTextureProvider gemTextureProvider;

        private GemView _gemViewPrefab;
        private ObjectPool<GemView> _pool;
        private AsyncOperationHandle<GameObject> _operationHandle;

        public override void Initialize()
        {
            if(gemTextureProvider != null)
                gemTextureProvider.Initialize();
            
            _operationHandle = gemViewReference.LoadAssetAsync();
            _operationHandle.WaitForCompletion()
                .TryGetComponent(out _gemViewPrefab);

            _pool = new ObjectPool<GemView>(CreateGemView, OnGemViewGet, OnGemViewReleased, OnGemViewDestroy);
        }

        public override void Release()
        {
            _pool.Dispose();
            gemTextureProvider.Release();
            _gemViewPrefab = null;
            if(_operationHandle.IsValid())
                Addressables.Release(_operationHandle);
        }

        public override GemView GetGemView(Gem gem) => _pool.Get();

        public override void ReturnGemView(GemView view) =>_pool.Release(view);
        
        private void OnGemViewDestroy(GemView view)
        {
            if(view != null)
                Destroy(view.gameObject);
        }

        private void OnGemViewReleased(GemView view)
        {
            view.ResetViewTransform();
            view.gameObject.name = "Pooled";
        }

        private void OnGemViewGet(GemView view)
        {
        }

        private GemView CreateGemView()
        {
            var gemView = Instantiate(_gemViewPrefab);
            gemView.Initialize(this, gemTextureProvider);
            return gemView;
        }
    }
}