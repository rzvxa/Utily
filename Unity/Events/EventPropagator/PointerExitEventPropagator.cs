using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class PointerExitEventPropagator : EventPropagatorBase<IPointerExitHandler>, IPointerExitHandler
    {
        public PointerExitEventPropagator()
            : base(ExecuteEvents.pointerExitHandler)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Propagate(eventData);
        }
    }
}