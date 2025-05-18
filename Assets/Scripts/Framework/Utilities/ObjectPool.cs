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

        private Action<GameObject> onGetAction; //取出对象池时的回调
        private Action<GameObject> onReleaseAction; //释放回对象池时的回调

        public int PoolCount => pool.Count;

        public ObjectPool(int size, Factory factory)
        {
            pool = new List<GameObject>(size);
            this.factory = factory;

            onGetAction = (obj) => { obj.SetActive(true); }; //添加初始委托方法
            onReleaseAction = (obj) => { obj?.SetActive(false); }; //添加初始委托方法
        }

        /// <summary>
        /// 创建GameObject
        /// 使用世界坐标
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
        /// 创建GameObject
        /// 使用本地坐标
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
        /// 创建GameObject，指定父对象
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
        /// 创建GameObject，指定父对象
        /// 使用本地坐标
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
        /// 将销毁Obj释放进对象池中
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
        /// 清除对象池的所有信息
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


