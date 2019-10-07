using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class DropHandlerEventPropagator : EventPropagatorBase<IDropHandler>, IDropHandler
    {
        public DropHandlerEventPropagator()
            : base(ExecuteEvents.dropHandler)
        {
        }

        public void OnDrop(PointerEventData eventData)
        {
            Propagate(eventData);
        }
    }
}