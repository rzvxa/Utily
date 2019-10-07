using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class PointerEnterEventPropagator : EventPropagatorBase<IPointerEnterHandler>, IPointerEnterHandler
    {
        public PointerEnterEventPropagator()
            : base(ExecuteEvents.pointerEnterHandler)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Propagate(eventData);
        }
    }
}