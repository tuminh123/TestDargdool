using UnityEngine;

public class DontDestroyerOnLoadComponent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
