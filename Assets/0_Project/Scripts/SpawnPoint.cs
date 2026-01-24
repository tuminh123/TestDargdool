using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Transform[] summonPoint;

    public Vector3 GetSpawnPos() => summonPoint[Random.Range(0, summonPoint.Length)].position;
}
