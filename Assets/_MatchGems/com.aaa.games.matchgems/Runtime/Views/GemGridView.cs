using AAA.Games.MatchGems.Runtime.GemViewProviders;
using AAA.Games.MatchGems.Runtime.Input;
using AAA.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace AAA.Games.MatchGems.Runtime
{
    public class GemGridView : MonoBehaviour
        , IPositionProvider
    {
        [SerializeField] private float gemSize;
        [SerializeField] private float spacing;
        [SerializeField] private float padding;
        [SerializeField] private RectTransform gemParent;
        [SerializeField] private UnityEvent onSuccessfulSwap;
        [SerializeField] private UnityEvent onFailedSwap;

        private GemGrid _gemGrid;
        private GemViewProvider _gemViewProvider;
        private Vector3 _offset;
        private float _sizeMultiplier;
        private IDragInputReceiver _inputReceiver;

        public void Initialize(GemGrid gemGrid, GemViewProvider gemViewProvider, IDragInputReceiver inputReceiver)
        {
            _gemGrid = gemGrid;
            _gemViewProvider = gemViewProvider;
            _inputReceiver = inputReceiver;
            _sizeMultiplier = (gemParent.rect.width - (padding * 2)) /
                              (_gemGrid.GetSize().x * (100 * gemSize + spacing));
            _offset = (100 * gemSize * _sizeMultiplier + spacing) * 0.5f *
                      (_gemGrid.GetSize().ToVector3XY() - new Vector3(1, 1));

            foreach (var gem in _gemGrid.GetGrid())
            {
                CreateNewView(gem);
            }
        }

        public void CreateNewView(Gem gem)
        {
            var gemView = _gemViewProvider.GetGemView(gem);
            gemView.transform.SetParent(gemParent);
            gemView.transform.localScale = gemSize.ToVector3() * _sizeMultiplier;
            gemView.Link(gem, this, _inputReceiver);
        }

        public Vector3 GetPosition(Vector2Int gridPosition) => gridPosition.ToVector3XY() * (100 * gemSize * _sizeMultiplier + spacing) - _offset;
        public void OnSuccessfulSwap() => onSuccessfulSwap?.Invoke();
        public void OnFailedSwap() => onFailedSwap?.Invoke();
    }
}