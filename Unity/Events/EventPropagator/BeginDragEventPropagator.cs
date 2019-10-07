using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class BeginDragEventPropagator : EventPropagatorBase<IBeginDragHandler>, IBeginDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData) => Propagate(eventData);

        public BeginDragEventPropagator()
            : base(ExecuteEvents.beginDragHandler)
        {
        }
    }
}