using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager<T> : MonoBehaviour where T : MonoBehaviour,IObjectPool
{
    //event
    public event System.Action<T> OnSpawm;
    public event System.Action<T> OnDeSpawm;

    [SerializeField] protected Transform holder;
    [SerializeField] protected ListDataPoolSO<T> data;
    [SerializeField] protected int count = 0;

    protected Dictionary<string, Queue<T>> objDic = new Dictionary<string, Queue<T>>();

    //get
    public int Count => count;
    public Transform Holder => holder;

    //DeSpawn
    public void DeSpawn(T obj)
    {
        if (obj == null) return;

        string nameObj = obj.GetObjectName();
        if (!objDic.ContainsKey(nameObj))
        {
            objDic[nameObj] = new Queue<T>();
        }
        objDic[nameObj].Enqueue(obj);
        obj.gameObject.SetActive(false);

        OnDeSpawm?.Invoke(obj);

        count--;
    }

    //Spawn
    public T Spawn(string name, Vector3 pos, Quaternion rot)
    {
        if (name == null) return null;

        T obj = data.GetPrefabByName(name);
        if (obj == null) return null;

        return Spawn(obj, pos, rot);
    }

    public T Spawn(T obj, Vector3 pos, Quaternion rot)
    {
        if (obj == null) return null;

        T newObj = GetObjectFromPool(obj);

        SetParent(newObj, holder);
        newObj.gameObject.SetActive(true);
        newObj.transform.SetPositionAndRotation(pos, rot);

        count++;

        OnSpawm?.Invoke(obj);

        return newObj;
    }

    private T GetObjectFromPool(T obj)
    {
        string nameObj = obj.GetObjectName();
        if (objDic.ContainsKey(nameObj) && objDic[nameObj].Count > 0)
        {
            return objDic[nameObj].Dequeue();
        }
        T newObj = Instantiate(obj);
        newObj.name = nameObj;
        return newObj;
    }

    public void SetParent(T obj, Transform parent)
    {
        obj.transform.SetParent(parent);
    }
}
