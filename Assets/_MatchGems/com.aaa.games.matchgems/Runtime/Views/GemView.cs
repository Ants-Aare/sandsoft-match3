using System;
using AAA.Games.MatchGems.Runtime.GemViewProviders;
using AAA.Games.MatchGems.Runtime.Input;
using AAA.SDKs.SequenceBuilder.Runtime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AAA.Games.MatchGems.Runtime.Views
{
    public class GemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Transform icon;
        public Gem Gem;

        private GemTextureProvider _textureProvider;
        private GemViewProvider _gemViewProvider;
        private Vector3 _offset;
        private IPositionProvider _positionProvider;
        private IDragInputReceiver _inputReceiver;

        public void Initialize(GemViewProvider gemViewProvider, GemTextureProvider textureProvider)
        {
            _gemViewProvider = gemViewProvider;
            _textureProvider = textureProvider;
        }

        public void ResetViewTransform()
        {
            transform.SetParent(null);
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;
            icon.localPosition = Vector3.zero;
        }

        public void Link(Gem gem, IPositionProvider positionProvider, IDragInputReceiver inputReceiver)
        {
            _positionProvider = positionProvider;
            _inputReceiver = inputReceiver;

            Gem = gem;
            Gem.OnGemHighlighted += OnGemHighlighted;
            Gem.OnSwapped += OnSwapped;
            Gem.OnGemDestroyed += OnGemDestroyed;
            Gem.OnGemRefilled += OnGemRefilled;

            OnPositionChanged(gem.GetPosition());
            OnTextureProviderChanged();
        }

        public void UnLink()
        {
            Gem.OnGemHighlighted -= OnGemHighlighted;
            Gem.OnSwapped -= OnSwapped;
            Gem.OnGemDestroyed -= OnGemDestroyed;
            Gem.OnGemRefilled -= OnGemRefilled;
            Gem = null;

            _gemViewProvider.ReturnGemView(this);
        }

        private void OnPositionChanged(Vector2Int position)
        {
            gameObject.name = position.ToString();
            transform.localPosition = _positionProvider.GetPosition(position);
        }

        private void OnSwapped(Vector2Int targetPosition)
        {
            var position = _positionProvider.GetPosition(targetPosition);
            SequencePlayer.AddSequenceBuilder(new GemSwapSequenceBuilder(transform, position));
        }

        private void OnGemDestroyed()
        {
            SequencePlayer.AddSequenceBuilder(new GemDestroySequenceBuilder(transform, UnLink));
        }

        private void OnGemRefilled()
        {
            SequencePlayer.AddSequenceBuilder(new GemRefillSequenceBuilder(transform));
        }

        private void OnGemHighlighted()
        {
            transform.DOPunchScale(transform.localScale * 1.1f, 0.2f);
        }

        private void OnTextureProviderChanged()
        {
            if (image == null || Gem == null || _textureProvider == null)
            {
                Debug.Log("Image, gem or texture provider is null. image will not be set");
                return;
            }

            var sprite = _textureProvider.GetGemSprite(Gem);
            image.sprite = sprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _inputReceiver.ReceiveOnBeginDrag(eventData, Gem);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var targetPosition = transform.InverseTransformPoint(eventData.position);
            icon.localPosition = Vector3.Lerp(Vector3.zero, targetPosition, 0.3f);
            _inputReceiver.ReceiveOnDrag(eventData, Gem);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _inputReceiver.ReceiveOnEndDrag(eventData, Gem);
            icon.DOLocalMove(Vector3.zero, 0.1f);
        }
    }

    public readonly struct GemSwapSequenceBuilder : ISequenceBuilder
    {
        private readonly Transform _transform;
        private readonly Vector3 _targetPosition;

        public GemSwapSequenceBuilder(Transform transform, Vector3 targetPosition)
        {
            _transform = transform;
            _targetPosition = targetPosition;
        }

        public Tween BuildSequence()
            => _transform.DOLocalMove(_targetPosition, 0.2f)
                .SetEase(Ease.OutBack);

        public int GetPriority() => 100;
    }

    public class GemRefillSequenceBuilder : ISequenceBuilder
    {
        private readonly Transform _transform;

        public GemRefillSequenceBuilder(Transform transform)
        {
            _transform = transform;
            _transform.localScale = Vector3.zero;
        }

        public Tween BuildSequence()
            => _transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack);
        public int GetPriority() => 300;
    }

    public readonly struct GemDestroySequenceBuilder : ISequenceBuilder
    {
        private readonly Transform _transform;
        private readonly Action _onComplete;

        public GemDestroySequenceBuilder(Transform transform, Action onComplete)
        {
            _transform = transform;
            _onComplete = onComplete;
        }

        public Tween BuildSequence()
            => _transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(_onComplete.Invoke);

        public int GetPriority() => 200;
    }
}