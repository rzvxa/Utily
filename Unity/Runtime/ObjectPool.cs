using System;
using UnityEngine;
using System.Collections.Generic;

namespace Utils.Unity.Runtime
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private bool _dynamicSize = false;
        [SerializeField] private int _poolSize = 2;
        [SerializeField] private T _prefab = null;
        [SerializeField] private Vector3 _unusedPosition = new Vector3(-1000F, -1000F, -1000F);
        [SerializeField] private bool _deactiveUnused = false;

        private Stack<T> _pool = new Stack<T>();

        protected void Awake()
        {
            if(!_dynamicSize && _poolSize == 0) throw new InvalidOperationException();

            transform.Clear();

            for (int i = 0; i < _poolSize; i++)
                Release(CreateObject());
        }

        protected T Get()
        {
            T obj;

            if (_pool.Count > 0)
                obj = _pool.Pop();
            else
                obj = CreateObject();

            Setup(ref obj);
            return obj;
        }

        protected void Release(T obj)
        {
            Cleanup(ref obj);
            _pool.Push(obj);
        }

        protected virtual void Cleanup(ref T obj)
        {
            obj.transform.position = _unusedPosition;
            if (_deactiveUnused) obj.gameObject.SetActive(false);
        }

        protected virtual void Setup(ref T obj) =>
            obj.gameObject.SetActive(true);

        private T CreateObject() =>
            Instantiate(_prefab, _unusedPosition, Quaternion.identity, transform);

    }
}
