using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class DragEventPropagator : EventPropagatorBase<IDragHandler>, IDragHandler
    {
        public void OnDrag(PointerEventData eventData) => Propagate(eventData);

        public DragEventPropagator()
            : base(ExecuteEvents.dragHandler)
        {
        }
    }
}