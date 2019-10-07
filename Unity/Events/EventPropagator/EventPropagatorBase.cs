using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Unity.Runtime;

namespace Views.Components.EventPropagator
{
    public abstract class EventPropagatorBase : MonoBehaviour, IEventSystemHandler
    {
        public static void Propagate<TEvent>(GameObject owner, PointerEventData eventData, ExecuteEvents.EventFunction<TEvent> handler, bool propagatorToAll = false)
            where TEvent : IEventSystemHandler
        {
            var allResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, allResults);
            var frontObjects = new HashSet<GameObject>();
            var filteredResults = allResults
                .SkipWhile(r =>
                {
                    if (r.gameObject == owner) return false;
                    var handlerObject = ExecuteEvents.GetEventHandler<TEvent>(r.gameObject);
                    if (handlerObject == owner) return false;
                    frontObjects.Add(handlerObject);
                    return true;
                })
                .SkipWhile(r => r.gameObject == owner || ExecuteEvents.GetEventHandler<TEvent>(r.gameObject) == owner)
                .SkipWhile(r => frontObjects.Contains(r.gameObject) || frontObjects.Contains(ExecuteEvents.GetEventHandler<TEvent>(r.gameObject)))
                .ToList();
            if (filteredResults.Count == 0)
                filteredResults = allResults;
//            Debug.Log($"{owner.GetPathInScene()} {handler.GetType().GetGenericArguments().Single().Name} " +
//                      $"event propagation list:\n{string.Join("\n", filteredResults.Select(r => $"{r.gameObject.GetPathInScene()} -> {ExecuteEvents.GetEventHandler<TEvent>(r.gameObject)?.GetPathInScene()}"))}"
//                      + $"\n===\nthese are removed:\n{string.Join("\n", allResults.Except(filteredResults).Select(r => $"{r.gameObject.GetPathInScene()} -> {ExecuteEvents.GetEventHandler<TEvent>(r.gameObject)?.GetPathInScene()}"))}"
//            );
            foreach (var result in filteredResults)
            {
                if (result.gameObject == owner)
                    continue;
                var handlerObject = ExecuteEvents.GetEventHandler<TEvent>(result.gameObject);
                if (handlerObject == owner)
                    continue;
                if (handlerObject != null)
                {
//                    Debug.Log(
//                        $"{handler.GetType().GetGenericArguments().Single().Name} event propagated from {owner.GetPathInScene()} to {handlerObject?.GetPathInScene()}");
                    ExecuteEvents.Execute<TEvent>(handlerObject, eventData, handler);
                    if (propagatorToAll == false)
                        break;
                }
            }
        }
    }

    public abstract class EventPropagatorBase<TEvent> : EventPropagatorBase
        where TEvent : IEventSystemHandler
    {
        [SerializeField] private bool _propagatorToAll;

        private readonly ExecuteEvents.EventFunction<TEvent> _handler;

        protected EventPropagatorBase(ExecuteEvents.EventFunction<TEvent> handler)
        {
            _handler = handler;
        }

        protected void Propagate(PointerEventData eventData)
        {
            Propagate(gameObject, eventData, _handler, _propagatorToAll);
        }
    }
}