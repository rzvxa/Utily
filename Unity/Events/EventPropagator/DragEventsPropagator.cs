using UnityEngine;
using UnityEngine.EventSystems;

namespace Views.Components.EventPropagator
{
    public class DragEventsPropagator : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField]
        private bool _propagatorToAll;

        public void OnBeginDrag(PointerEventData eventData) => Propagate(eventData, EventTriggerType.BeginDrag);
        public void OnDrag(PointerEventData eventData) => Propagate(eventData, EventTriggerType.Drag);
        public void OnEndDrag(PointerEventData eventData) => Propagate(eventData, EventTriggerType.EndDrag);
        public void OnDrop(PointerEventData eventData) => Propagate(eventData, EventTriggerType.Drop);

        private void Propagate(PointerEventData eventData, EventTriggerType type)
        {
            switch (type)
            {
                case EventTriggerType.BeginDrag:
                    EventPropagatorBase.Propagate(gameObject, eventData, ExecuteEvents.beginDragHandler, _propagatorToAll);
                    break;
                case EventTriggerType.Drag:
                    EventPropagatorBase.Propagate(gameObject, eventData, ExecuteEvents.dragHandler, _propagatorToAll);
                    break;
                case EventTriggerType.EndDrag:
                    EventPropagatorBase.Propagate(gameObject, eventData, ExecuteEvents.endDragHandler, _propagatorToAll);
                    break;
                case EventTriggerType.Drop:
                    EventPropagatorBase.Propagate(gameObject, eventData, ExecuteEvents.dropHandler, _propagatorToAll);
                    break;

            }
            
        }
    }
}