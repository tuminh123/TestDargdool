using UnityEngine;

public abstract class DataPoolSO<T> : ScriptableObject where T : MonoBehaviour, IObjectPool
{
    private string id;
    [SerializeField] private string nameObj;
    [SerializeField] private T objectData;

    //get
    public T Prefab => objectData;
    public string Name => nameObj;
    public string Id => id;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
            Debug.Log($"Generated new ID for {name}: {id}");
        }
    }
}

