using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class PointerDownEventPropagator : EventPropagatorBase<IPointerDownHandler>, IPointerDownHandler
    {
        public PointerDownEventPropagator()
            : base(ExecuteEvents.pointerDownHandler)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Propagate(eventData);
        }
    }
}