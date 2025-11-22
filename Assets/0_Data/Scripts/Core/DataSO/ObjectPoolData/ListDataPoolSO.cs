using System.Collections.Generic;
using UnityEngine;

public abstract class ListDataPoolSO<T> : ScriptableObject where T : MonoBehaviour,IObjectPool
{
    [SerializeField] private List<DataPoolSO<T>> objList = new List<DataPoolSO<T>>();

    public T GetPrefabByName(string name)
    {
        foreach (var objInList in objList)
        {
            if (objInList.Prefab == null) continue;
            if (objInList.Prefab.GetObjectName() == name) return objInList.Prefab;
        }
        return null;
    }
    public T GetRandomPrefab()
    {
        int index = Random.Range(0, objList.Count);
        return objList[index].Prefab;
    }
}
