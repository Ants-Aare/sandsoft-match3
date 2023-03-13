using AAA.SDKs.SequenceBuilder.Runtime;
using DG.Tweening;
using UnityEngine;

namespace AAA.Games.MatchGems.Runtime.Views
{
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
}