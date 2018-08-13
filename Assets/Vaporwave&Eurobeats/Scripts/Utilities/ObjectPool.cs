using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtilities;

namespace Scripts.Utilities {
    public class ObjectPool<T> : MonoBehaviour where T : Component {
        public T Prefab;
        public byte PrewarmAmount;

        private readonly List<T> pooledObjects = new List<T>();
        private readonly List<T> usedObjects = new List<T>();
        private readonly List<T> temporaryObjects = new List<T>();

        private void Start() {
            for (byte i = 0; i < PrewarmAmount; i++) {
                CreateNew();
            }
        }

        private void CreateNew() {
            var obj = Instantiate(Prefab);
            obj.transform.parent = transform;
            obj.gameObject.SetActive(false);
            pooledObjects.Add(obj);
        }

        public T Get() {
            if (pooledObjects.IsEmpty()) {
                return GetTemporary();
            }

            var obj = pooledObjects.First();
            obj.gameObject.SetActive(true);
            pooledObjects.Remove(obj);
            usedObjects.Add(obj);
            return obj;
        }

        public void Return(T obj) {
            if (obj == null) {
                return;
            }

            if (temporaryObjects.Contains(obj)) {
                temporaryObjects.Remove(obj);
                Destroy(obj.gameObject);
                return;
            }

            if (!usedObjects.Contains(obj)) {
                return;
            }

            usedObjects.Remove(obj);
            obj.gameObject.SetActive(false);
            obj.transform.parent = transform;
            obj.transform.position = Vector3.zero;
            pooledObjects.Add(obj);
        }

        private T GetTemporary() {
            var obj = Instantiate(Prefab);
            obj.transform.parent = transform;
            temporaryObjects.Add(obj);
            return obj;
        }
    }
}