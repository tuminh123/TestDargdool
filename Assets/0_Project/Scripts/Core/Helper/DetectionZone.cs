using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private List<EnemyAI> enemiesInside = new List<EnemyAI>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();
        if (enemy == null) return;
        if (!enemiesInside.Contains(enemy)) enemiesInside.Add(enemy);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyAI enemy = collision.GetComponentInParent<EnemyAI>();
       if(enemy == null) return;
        enemiesInside.Remove(enemy);
    }
    public EnemyAI GetFirstEnemy()
    {
        if (enemiesInside.Count > 0)
            return enemiesInside[0];
        return null;
    }

    public EnemyAI GetNearestEnemy(Transform origin)
    {
        EnemyAI nearest = null;
        float minDist = Mathf.Infinity;

        foreach (EnemyAI enemy in enemiesInside)
        {
            if (enemy == null) continue;
            float dist = Vector3.Distance(origin.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
