using System.Collections.Generic;
using AAA.Games.FruitMatch3d.Runtime.Model;
using UnityEngine;
using UnityEngine.Events;

namespace AAA.Games.FruitMatch3d.Runtime.Views
{
    public class FruitGridView : MonoBehaviour
    {
        [SerializeField] private UnityEvent onSuccessfulSwap;
        [SerializeField] private UnityEvent onFailedSwap;
        [SerializeField] private FruitModelView[] fruitModelViews;

        private FruitGrid _fruitGrid;
        private List<FruitModelView> _views = new();


        public void Initialize(FruitGrid fruitGrid)
        {
            _fruitGrid = fruitGrid;

            foreach (var fruit in _fruitGrid.GetGrid())
            {
                CreateNewView(fruit);
            }
        }

        public void CreateNewView(Fruit value)
        {
            var typeID = value.GetTypeID();
            var prefab = fruitModelViews[typeID];
            var view = Instantiate(prefab, transform);
            view.Link(value);
            _views.Add(view);
        }

        public void OnSuccessfulSwap() => onSuccessfulSwap?.Invoke();
        public void OnFailedSwap() => onFailedSwap?.Invoke();

        public void RegenerateBoard()
        {
            foreach (var view in _views)
            {
                if(view != null)
                    view.UnLink();
            }
            _views.Clear();
            
            foreach (var fruit in _fruitGrid.GetGrid())
            {
                CreateNewView(fruit);
            }
        }
    }
}