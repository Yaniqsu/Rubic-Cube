using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YNQ
{
    public struct PoolSettings
    {
        public HideFlags hideFlags;

        public PoolSettings(HideFlags hideFlags)
        {
            this.hideFlags = hideFlags;
        }
        
        public static readonly  PoolSettings Default = new PoolSettings(HideFlags.HideInHierarchy);
    }
    
    public class ObjectPool<T> where T : Component
    {
        private List<T> _pooledObjectsList;
        private T _pooledObject;
        private int _maxSize;
        private Transform _context;
        private Action<T> _onNewObjectMade;
        private PoolSettings _settings;
        
        public List<T> PooledObjects => _pooledObjectsList;

        public Action<T> OnNewObjectMade
        {
            get => _onNewObjectMade;
            set => _onNewObjectMade = value;
        }

        public ObjectPool(T pooledObject, int startSize, int maxSize, Transform context, PoolSettings settings,
            Action<T> onNewObjectMade = null)
        {
            _pooledObjectsList = new List<T>();
            _pooledObject = pooledObject;
            _maxSize = maxSize;
            _context = context;
            _settings = settings;
            _onNewObjectMade = onNewObjectMade;
            _onNewObjectMade += obj =>
            {
                obj.gameObject.hideFlags = _settings.hideFlags;
            };

            for (int i = 0; i < startSize; i++)
            {
                AddObjectToPool();
            }
        }

        public ObjectPool(T pooledObject, int startSize, int maxSize, Transform context,
            Action<T> onNewObjectMade = null) 
            : this(pooledObject, startSize, maxSize, context, PoolSettings.Default, onNewObjectMade) {}

        private T AddObjectToPool()
        {
            if (_pooledObjectsList.Count == _maxSize) return null;

            var newObject = Object.Instantiate(_pooledObject, _context);

            newObject.gameObject.SetActive(false);

            _pooledObjectsList.Add(newObject);

            _onNewObjectMade?.Invoke(newObject);

            return newObject;
        }

        public T GetObject(Func<T, bool> predicate)
        {
            foreach (var obj in _pooledObjectsList.Where(predicate))
            {
                return obj;
            }
            
            return AddObjectToPool();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public T GetFreeObject()
            => GetObject(obj => obj != null && !obj.gameObject.activeSelf);

        public void Reset()
        {
            _pooledObjectsList.ForEach(obj => obj.gameObject.SetActive(false));
        }

        public void MakeOperationOnObjects(Action<T> action)
        {
            _pooledObjectsList.ForEach(_obj => action(_obj));
        }

        public void ClearNullObjects()
        {
            var objectsToRemove = new List<T>();

            foreach (var obj in _pooledObjectsList)
            {
                if(!obj)
                    objectsToRemove.Add(obj);
            }
            
            foreach (var obj in objectsToRemove)
            {
                _pooledObjectsList.Remove(obj);
            }
        }
    }
}