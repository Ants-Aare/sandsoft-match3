using AAA.Games.FruitMatch3d.Runtime.Model;
using AAA.Games.FruitMatch3d.Runtime.Views;
using AAA.SDKs.Match3.Runtime;
using AAA.SDKs.Match3.Runtime.Detection;
using AAA.SDKs.Match3.Runtime.GridProviders;
using AAA.SDKs.Match3.Runtime.Matching;
using AAA.SDKs.Match3.Runtime.Swapping;
using AAA.SDKs.Match3.Runtime.Refilling;
using AAA.SDKs.SequenceBuilder.Runtime;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AAA.Games.FruitMatch3d.Runtime
{
    [CreateAssetMenu(menuName = "Create FruitMatchGameMode", fileName = "FruitMatchGameMode", order = 0)]
    public class FruitMatchGameMode : GameModeBase
    {
        [SerializeField] private float minimumDragDistance;
        [SerializeField] private GridProviderBase<Fruit> gridProvider;

        [SerializeField] private AssetReferenceGameObject fruitGridViewPrefabReference;
        private AsyncOperationHandle<GameObject> _fruitGridViewHandle;

        private ISwapsDetector _swapsDetector;
        private IMatchDetector _matchDetector;

        private FruitMatch3dInputSystem _fruitInputSystem;
        private ISwappingSystem _swappingSystem;
        private IMatchingSystem _matchingSystem;
        private IRefillingSystem<Fruit> _refillingSystem;

        private FruitFactory _fruitFactory;
        private FruitGridView _fruitGridView;
        private FruitGrid _fruitGrid;

        public override void StartGame()
        {
            DOTween.SetTweensCapacity(200, 100);
            _fruitGrid = new FruitGrid(gridProvider.GetSize());
            _fruitFactory = new FruitFactory();

            _matchDetector = new MatchGroupDetector<Fruit>(_fruitGrid);
            _swapsDetector = new SwapsDetector<Fruit>(_fruitGrid, _matchDetector);

            gridProvider.Initialize(_matchDetector);
            _fruitGridViewHandle = fruitGridViewPrefabReference.InstantiateAsync();
            _fruitGridView = _fruitGridViewHandle.WaitForCompletion().GetComponent<FruitGridView>();

            gridProvider.PopulateGrid(_fruitGrid, _fruitFactory);

            var swappingConditions = new ISwappingCondition[]
            {
                new OutOfBoundsSwappingCondition<Fruit>(_fruitGrid),
                new AdjacentSwappingCondition(),
                // new SwapDetectionSwappingCondition(_swapsDetector)
            };
            var matchResolvers = new IMatchResolver<Fruit>[]
            {
                new DestroyMatchResolver<Fruit>()
            };

            _fruitInputSystem = new FruitMatch3dInputSystem(minimumDragDistance);
            _swappingSystem = new SwappingSystem<Fruit>(_fruitGrid, swappingConditions);
            _matchingSystem = new MatchingSystem<Fruit>(_fruitGrid, _matchDetector, matchResolvers);
            _refillingSystem = new FallingSystem<Fruit>(_fruitGrid, _fruitFactory, gridProvider);

            _fruitInputSystem.OnSwapTiles += _swappingSystem.TrySwapTiles;

            _swappingSystem.OnSuccessfulSwap += _matchingSystem.ResolveMatches;
            _swappingSystem.OnSuccessfulSwap += _fruitGridView.OnSuccessfulSwap;
            _swappingSystem.OnFailedSwap += _fruitGridView.OnFailedSwap;

            _matchingSystem.OnMatched += _refillingSystem.RefillGrid;
            _refillingSystem.OnNewTileCreated += _fruitGridView.CreateNewView;

            _fruitGrid.OnGridChanged += SequencePlayer.StartSequencesNextFrame;
            
            SequencePlayer.AnimationsFinished += _matchingSystem.ResolveMatches;

            _fruitGridView.Initialize(_fruitGrid);
        }

        public override void RunUpdateLoop()
        {
            _fruitInputSystem?.RunUpdateLoop();
        }

        public override void TearDownGame()
        {
            if (_fruitGridView != null)
                Destroy(_fruitGridView.gameObject);
            Addressables.Release(_fruitGridViewHandle);
        }

        [Button]
        public void SwapFruits()
        {
            _swappingSystem.TrySwapTiles(new Vector2Int(2, 2), new Vector2Int(2, 3));
        }
        [Button]
        public void RegenerateBoard()
        {
            _fruitGridView.RegenerateBoard();
        }
    }
}