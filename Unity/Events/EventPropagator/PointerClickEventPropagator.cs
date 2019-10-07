using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class PointerClickEventPropagator : EventPropagatorBase<IPointerClickHandler>, IPointerClickHandler
    {
        public PointerClickEventPropagator()
            : base(ExecuteEvents.pointerClickHandler)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Propagate(eventData);
        }
    }
}