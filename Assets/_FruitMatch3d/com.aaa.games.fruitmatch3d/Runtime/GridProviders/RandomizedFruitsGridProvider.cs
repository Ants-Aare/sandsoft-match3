using AAA.Games.FruitMatch3d.Runtime.Model;
using AAA.SDKs.Match3.Runtime.GridProviders;
using UnityEngine;

namespace AAA.Games.FruitMatch3d.Runtime
{
    [CreateAssetMenu(menuName = "Create RandomizedFruitsGridProvider", fileName = "RandomizedFruitsGridProvider", order = 0)]
    public class RandomizedFruitsGridProvider : RandomGridProviderBase<Fruit>
    {
        
    }
}