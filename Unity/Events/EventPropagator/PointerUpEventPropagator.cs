using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class PointerUpEventPropagator : EventPropagatorBase<IPointerUpHandler>, IPointerUpHandler
    {
        public PointerUpEventPropagator()
            : base(ExecuteEvents.pointerUpHandler)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Propagate(eventData);
        }
    }
}