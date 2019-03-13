using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>继承该接口以支持对象池</summary>
public interface IPool {

    /// <summary>重置</summary>
    void ResetObject();

}

/// <summary>对象池异常</summary>
public class ObjectPoolException : ArgumentException {
    public ObjectPoolException(string message, Exception innerException) : base(message, innerException) { }
    public ObjectPoolException(string message) : base(message) { }
    public ObjectPoolException() : base() { }
}

public static class ObjectPool {

    private static Dictionary<string, Stack<GameObject>> GameObjectsInPool;

    static ObjectPool() {
        GameObjectsInPool = new Dictionary<string, Stack<GameObject>>();
    }

    /// <summary>根据指定Key向对象池添加游戏物体 </summary>
    public static void StoreGameObject(string key, GameObject _GameObject) {
        if (_GameObject == null) {
            throw new ObjectPoolException("The gameObject couldn't be null reference");
        }
        if (!GameObjectsInPool.ContainsKey(key)) {
            GameObjectsInPool.Add(key, new Stack<GameObject>());
        }
        GameObjectsInPool[key].Push(_GameObject);
    }

    public static bool TryGetGameObject(string key, out GameObject _GameObject) {
        Stack<GameObject> gameObjects = null;
        if (!GameObjectsInPool.TryGetValue(key, out gameObjects)) {
            _GameObject = null;
            return false;
        }
        while(gameObjects.Count > 0) {
            _GameObject = gameObjects.Pop();
            if (_GameObject != null) {
                return true;
            }
        }
        GameObjectsInPool.Remove(key);
        _GameObject = null;
        return false;
    }

}

/// <summary>对象池(Type作为泛型参数 相当于ObjectPool的Key)</summary>
public static class ObjectPool<T> where T : class, IPool {

    public static int Count {//当前容量
        get { return ObjectsInPool.Count; }
    }

    private static int MaxCapacity = 10;//默认最大容量

    private static Stack<T> ObjectsInPool;

    /// <summary>可以手动初始化</summary>
    public static void Init(int maxCapacity) {
        MaxCapacity = maxCapacity;
        ObjectsInPool = new Stack<T>();
    }
    public static void Init() {
        Init(2);
    }

    /// <summary>ctor</summary>
    static ObjectPool() {
        Init();
    }

    /// <summary>向对象池中添加游戏对象</summary>
    public static void StoreObject(T _Object) {
        if (ObjectsInPool.Count >= MaxCapacity) {
            MaxCapacity *= 2;//自动扩容
        }
        if (_Object == null) {
            throw new ObjectPoolException("ObjectPool couldn't add a null reference");
        }
        if (ObjectsInPool.Contains(_Object)) {
            throw new ObjectPoolException("The object has existed in ObjectPool");
        }
        ObjectsInPool.Push(_Object);
    }

    public static void StoreGameObject(GameObject _GameObject) {
        if (_GameObject == null) {
            throw new ObjectPoolException("The gameObject couldn't be null reference");
        }
        T poolObject = _GameObject.GetComponent<T>();
        if (poolObject == null) {
            throw new ObjectPoolException("The gameObject isn't attached a IPool");
        }
        StoreObject(poolObject);
        _GameObject.SetActive(false);
        _GameObject.transform.SetParent(App.Instance.GameObjectPoolRoot);
    }

    /// <summary>从对象池中取出游戏对象</summary>
    public static bool TryGetObject(out T _Object) {
        if (ObjectsInPool.Count == 0) {
            _Object = null;
            return false;
        }
        T sourceObject = ObjectsInPool.Pop();
        if (sourceObject != null) {
            sourceObject.ResetObject();
            _Object = sourceObject;
            return true;
        }
        else {
            return TryGetObject(out _Object);
        }
    }

    public static bool TryGetGameObject(out GameObject _GameObject) {
        if (!typeof(T).IsSubclassOf(typeof(MonoBehaviour))) {
            throw new ObjectPoolException(string.Format("{0} couldn't be attached to gameObject", typeof(T).Name));
        }
        T _Object = null;
        if (TryGetObject(out _Object)) {
            _GameObject = (_Object as MonoBehaviour).gameObject;
            return true;
        }
        else {
            _GameObject = null;
            return false;
        }

    }

}
