using System.Collections.Generic;
using AAA.Games.MatchGems.Runtime.GemViewProviders;
using AAA.Games.MatchGems.Runtime.Input;
using AAA.SDKs.Match3.Runtime;
using AAA.SDKs.Match3.Runtime.Detection;
using AAA.SDKs.Match3.Runtime.GridProviders;
using AAA.SDKs.Match3.Runtime.Matching;
using AAA.SDKs.Match3.Runtime.Swapping;
using AAA.SDKs.Match3.Runtime.Refilling;
using AAA.SDKs.SequenceBuilder.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AAA.Games.MatchGems.Runtime
{
    [CreateAssetMenu(menuName = "Create MatchGemsGame", fileName = "MatchGemsGame", order = 0)]
    public class MatchGemsGame : GameModeBase
    {
        [SerializeField] private float minimumDragDistance;
        
        [SerializeField] private GemViewProvider gemViewProvider;
        [SerializeField] private GridProviderBase<Gem> gridProvider;

        [SerializeField] private AssetReferenceGameObject gemGridViewPrefabReference;
        private AsyncOperationHandle<GameObject> _gemGridViewHandle;

        private ISwapsDetector _swapsDetector;
        private IMatchDetector _matchDetector;
        
        private GemInputSystem _gemInputSystem;
        private ISwappingSystem _swappingSystem;
        private IMatchingSystem _matchingSystem;
        private IRefillingSystem<Gem> _refillingSystem;

        private GemFactory _gemFactory;
        private GemGridView _gemGridView;
        private GemGrid _gemGrid;

        public override void StartGame()
        {
            _gemGrid = new GemGrid(gridProvider.GetSize());
            _gemFactory = new GemFactory();
            
            _matchDetector = new MatchGroupDetector<Gem>(_gemGrid);
            _swapsDetector = new SwapsDetector<Gem>(_gemGrid, _matchDetector);

            gemViewProvider.Initialize();
            gridProvider.Initialize(_matchDetector);
            _gemGridViewHandle = gemGridViewPrefabReference.InstantiateAsync();
            _gemGridView = _gemGridViewHandle.WaitForCompletion().GetComponent<GemGridView>();
            
            gridProvider.PopulateGrid(_gemGrid, _gemFactory);

            var swappingConditions = new ISwappingCondition[]
            {
                new OutOfBoundsSwappingCondition<Gem>(_gemGrid),
                new AdjacentSwappingCondition(),
                new SwapDetectionSwappingCondition(_swapsDetector)
            };
            var matchResolvers = new IMatchResolver<Gem>[]
            {
                new DestroyMatchResolver<Gem>()
            };

            _gemInputSystem = new GemInputSystem(minimumDragDistance);
            _swappingSystem = new SwappingSystem<Gem>(_gemGrid, swappingConditions);
            _matchingSystem = new MatchingSystem<Gem>(_gemGrid, _matchDetector, matchResolvers);
            _refillingSystem = new RefillingSystem<Gem>(_gemGrid, _gemFactory, gridProvider);
            
            _gemInputSystem.OnSwapTiles += _swappingSystem.TrySwapTiles;
            
            _swappingSystem.OnSuccessfulSwap += _matchingSystem.ResolveMatches;
            _swappingSystem.OnSuccessfulSwap += _gemGridView.OnSuccessfulSwap;
            _swappingSystem.OnFailedSwap += _gemGridView.OnFailedSwap;

            _matchingSystem.OnMatched += _refillingSystem.RefillGrid;
            _refillingSystem.OnNewTileCreated += _gemGridView.CreateNewView;
                
            _gemGrid.OnGridChanged += SequencePlayer.StartSequencesNextFrame;
            SequencePlayer.AnimationsFinished += _matchingSystem.ResolveMatches;

            _gemGridView.Initialize(_gemGrid, gemViewProvider, _gemInputSystem);
        }

        public override void TearDownGame()
        {
            gemViewProvider.Release();
            if(_gemGridView != null)    
                Destroy(_gemGridView.gameObject);
            Addressables.Release(_gemGridViewHandle);
        }

        [Button]
        public void HighlightMatchingGems()
        {
            var matchGroups = _matchDetector.GetAllMatchGroups();
            var positions = new HashSet<Vector2Int>();
            foreach (var matchGroup in matchGroups)
            {
                positions.UnionWith(matchGroup.Positions);
            }

            _gemGrid.HighlightGems(positions);
        }

        [Button]
        public void HighlightSwappableGems()
        {
            var matchGroup = _swapsDetector.GetRandomPossibleSwap();
            _gemGrid.HighlightGems(matchGroup.Positions);
        }

        [Button]
        public void SwapGems()
        {
            _swappingSystem.TrySwapTiles(new Vector2Int(2, 2), new Vector2Int(2, 3));
        }
    }
}