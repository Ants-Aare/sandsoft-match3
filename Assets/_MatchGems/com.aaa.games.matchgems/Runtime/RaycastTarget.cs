//Stolen from com.aaa.utility.ui, but I didn't want to import the whole thing

using UnityEngine;
using UnityEngine.UI;

namespace AAA.Games.MatchGems.Runtime
{
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class RaycastTarget : Graphic
    {
        public override void SetMaterialDirty() { }

        public override void SetVerticesDirty() { }

        protected override void OnPopulateMesh(VertexHelper vh) => vh.Clear();
    }
}