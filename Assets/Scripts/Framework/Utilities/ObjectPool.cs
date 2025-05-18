using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Utilities
{
    public class ObjectPool
    {
        private readonly List<GameObject> pool;
        private Factory factory;

        private Action<GameObject> onGetAction; //ȡ�������ʱ�Ļص�
        private Action<GameObject> onReleaseAction; //�ͷŻض����ʱ�Ļص�

        public int PoolCount => pool.Count;

        public ObjectPool(int size, Factory factory)
        {
            pool = new List<GameObject>(size);
            this.factory = factory;

            onGetAction = (obj) => { obj.SetActive(true); }; //��ӳ�ʼί�з���
            onReleaseAction = (obj) => { obj?.SetActive(false); }; //��ӳ�ʼί�з���
        }

        /// <summary>
        /// ����GameObject
        /// ʹ����������
        /// </summary>
        /// <param name="position"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public GameObject GetWorld(Vector3 position, Quaternion quaternion)
        {
            GameObject obj;
            if (pool.Count > 0)
            {
                obj = pool[pool.Count - 1];
                obj.transform.position = position;
                obj.transform.rotation = quaternion;
                pool.Remove(obj);
            }
            else
            {
                obj = factory.Create(position, quaternion);
            }

            onGetAction?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// ����GameObject
        /// ʹ�ñ�������
        /// </summary>
        /// <param name="position"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public GameObject GetLocal(Vector3 position, Quaternion quaternion)
        {
            GameObject obj;
            if (pool.Count > 0)
            {
                obj = pool[pool.Count - 1];
                obj.transform.localPosition = position;
                obj.transform.rotation = quaternion;
                pool.Remove(obj);
            }
            else
            {
                obj = factory.Create(position, quaternion);
                obj.transform.localPosition = position;
            }

            onGetAction?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// ����GameObject��ָ��������
        /// </summary>
        /// <param name="position"></param>
        /// <param name="quaternion"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject GetWorld(Vector3 position, Quaternion quaternion, Transform parent)
        {
            GameObject obj;
            if (pool.Count > 0)
            {
                obj = pool[pool.Count - 1];
                obj.transform.position = position;
                obj.transform.rotation = quaternion;
                pool.Remove(obj);
            }
            else
            {
                obj = factory.Create(position, quaternion, parent);
            }

            onGetAction?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// ����GameObject��ָ��������
        /// ʹ�ñ�������
        /// </summary>
        /// <param name="position"></param>
        /// <param name="quaternion"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject GetLocal(Vector3 position, Quaternion quaternion, Transform parent)
        {
            GameObject obj;
            if (pool.Count > 0)
            {
                obj = pool[pool.Count - 1];
                obj.transform.localPosition = position;
                obj.transform.rotation = quaternion;
                pool.Remove(obj);
            }
            else
            {
                obj = factory.Create(position, quaternion, parent);
                obj.transform.localPosition = position;
            }

            onGetAction?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// ������Obj�ͷŽ��������
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Release(GameObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            onReleaseAction?.Invoke(obj);
            //pool.Push(obj);
            pool.Add(obj);
        }

        /// <summary>
        /// �������ص�������Ϣ
        /// </summary>
        public void Clear()
        {
            pool.Clear();
        }

        public void AddOnGetCallback(Action<GameObject> callback)
        {
            onGetAction += callback;
        }

        public void RemoveOnGetCallback(Action<GameObject> callback)
        {
            onGetAction -= callback;
        }

        public void AddOnReleaseCallback(Action<GameObject> callback)
        {
            onReleaseAction += callback;
        }

        public void RemoveOnGetCallBack(Action<GameObject> callback)
        {
            onReleaseAction -= callback;
        }
    }
}


