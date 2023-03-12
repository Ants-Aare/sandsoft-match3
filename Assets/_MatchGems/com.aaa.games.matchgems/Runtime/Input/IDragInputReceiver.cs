using AAA.SDKs.Match3.Runtime;
using UnityEngine.EventSystems;

namespace AAA.Games.MatchGems.Runtime.Input
{
    public interface IDragInputReceiver
    {
        public void ReceiveOnBeginDrag(PointerEventData eventData, IGridPositionProvider gridPositionProvider);

        public void ReceiveOnDrag(PointerEventData eventData, IGridPositionProvider gridPositionProvider);

        public void ReceiveOnEndDrag(PointerEventData eventData, IGridPositionProvider gridPositionProvider);
    }
}