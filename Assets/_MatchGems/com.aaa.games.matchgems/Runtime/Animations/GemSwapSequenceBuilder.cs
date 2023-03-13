using AAA.SDKs.SequenceBuilder.Runtime;
using DG.Tweening;
using UnityEngine;

namespace AAA.Games.MatchGems.Runtime.Views
{
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
}