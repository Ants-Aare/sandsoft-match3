using System;
using AAA.SDKs.SequenceBuilder.Runtime;
using DG.Tweening;
using UnityEngine;

namespace AAA.Games.MatchGems.Runtime.Views
{
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