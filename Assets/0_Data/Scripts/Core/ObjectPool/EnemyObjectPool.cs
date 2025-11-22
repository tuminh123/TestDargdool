using System.Collections;
using UnityEngine;

public class EnemyObjectPool : ObjectPoolManager<EnemyAI>
{
    public Transform pointSpawm;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Spawn("Enemy", pointSpawm.position, Quaternion.identity);
        }
    }
}