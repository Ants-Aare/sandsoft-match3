using System;
using AAA.Games.FruitMatch3d.Runtime.Model;
using AAA.Games.MatchGems.Runtime.GemViewProviders;
using AAA.Games.MatchGems.Runtime.Views;
using AAA.SDKs.SequenceBuilder.Runtime;
using AAA.Utility.Extensions;
using DG.Tweening;
using UnityEngine;

namespace AAA.Games.FruitMatch3d.Runtime.Views
{
    public class FruitModelView : MonoBehaviour
    {
        [SerializeField] private Transform visuals;
        public Fruit Fruit;
        private Sequence _visualsIdleSequence;

        // private FruitViewProvider _fruitViewProvider;

        public void Initialize(GemViewProvider gemViewProvider, GemTextureProvider textureProvider)
        {
            // _fruitViewProvider = gemViewProvider;
        }

        public void ResetViewTransform()
        {
            transform.SetParent(null);
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;
            visuals.localPosition = Vector3.zero;
        }

        public void Link(Fruit fruit)
        {
            Fruit = fruit;
            Fruit.OnSwapped += OnSwapped;
            Fruit.OnDestroyed += OnFruitDestroyed;
            Fruit.OnRefilled += OnFruitRefilled;
            Fruit.OnFalling += OnFruitFalling;

            visuals.localRotation = Quaternion.Euler(new Vector3(0, -10, 0));
            _visualsIdleSequence = DOTween.Sequence()
                .Append
                (
                    visuals.DORotate(new Vector3(0, 5, 0), 2)
                        .SetEase(Ease.InOutSine)
                )
                .Append
                (
                    visuals.DORotate(new Vector3(0, -5, 0), 2)
                        .SetEase(Ease.InOutSine)
                )
                .SetLoops(-1, LoopType.Yoyo);

            OnPositionChanged(fruit.GetPosition());
        }


        public void UnLink()
        {
            _visualsIdleSequence?.Kill();

            Fruit.OnSwapped -= OnSwapped;
            Fruit.OnDestroyed -= OnFruitDestroyed;
            Fruit.OnRefilled -= OnFruitRefilled;
            Fruit.OnFalling -= OnFruitFalling;
            Fruit = null;
            
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _visualsIdleSequence?.Kill();
        }

        private void OnPositionChanged(Vector2Int position)
        {
            gameObject.name = position.ToString();
            transform.localPosition = position.ToVector3XZ();
        }

        private void OnSwapped(Vector2Int targetPosition)
        {
            var position = targetPosition.ToVector3XZ();
            SequencePlayer.AddSequenceBuilder(new GemSwapSequenceBuilder(transform, position));
        }

        private void OnFruitDestroyed()
        {
            SequencePlayer.AddSequenceBuilder(new GemDestroySequenceBuilder(transform, UnLink));
        }

        private void OnFruitRefilled()
        {
            SequencePlayer.AddSequenceBuilder(new GemRefillSequenceBuilder(transform));
        }

        private void OnFruitFalling(Vector2Int source, Vector2Int target)
        {
            transform.position = source.ToVector3XZ();
            for (var i = source.y; i >= target.y; i--)
            {
                SequencePlayer.AddSequenceBuilder(new FruitFallingSequenceBuilder(transform, new Vector2Int(source.x, i), source.y - i));
            }
        }
    }

    public class FruitFallingSequenceBuilder : ISequenceBuilder
    {
        private readonly Transform _transform;
        private readonly Vector2Int _target;
        private readonly int _priority;

        public FruitFallingSequenceBuilder(Transform transform, Vector2Int target, int priority)
        {
            _transform = transform;
            _target = target;
            _priority = priority;
        }

        public Tween BuildSequence()
            => _transform.DOMove(_target.ToVector3XZ(), 0.1f)
                .SetEase(Ease.Linear);

        public int GetPriority() => 500 + _priority;
        public bool ShouldAddPauseBetweenPreviousSequence() => false;
    }
}