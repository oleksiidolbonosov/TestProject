using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Stack<T> _pool = new Stack<T>();

        public ObjectPool(T prefab, int initialSize = 10)
        {
            _prefab = prefab;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = Object.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                _pool.Push(obj);
            }
        }

        public T Get()
        {
            while (_pool.Count > 0)
            {
                var obj = _pool.Pop();
                if (obj != null)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            var newObj = Object.Instantiate(_prefab);
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        public void Return(T obj)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                _pool.Push(obj);
            }
        }

        public void Clear()
        {
            foreach (var obj in _pool)
            {
                if (obj != null)
                {
                    Object.Destroy(obj.gameObject);
                }
            }
            _pool.Clear();
        }
    }
}
