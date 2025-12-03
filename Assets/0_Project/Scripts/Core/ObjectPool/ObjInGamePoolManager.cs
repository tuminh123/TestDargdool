
using System.Collections;
using UnityEngine;

public class ObjInGamePoolManager : ObjectPoolManager<ObjInGameBase>
{
    [Header("Box spawn")]
    [SerializeField] Transform[] points;
    [SerializeField] private float durationSpawn = 30;
    private float time;
    private void Start()
    {
        time = durationSpawn;
    }
    private void Update()
    {
        time -= Time.deltaTime;
        if (SingletonManager.Instance.gameManager.CurrentState != GameState.PLAY) return;
        if(time <= 0)
        {
            BoxSpawn();
            time = durationSpawn;
        }
    }
    private void BoxSpawn()
    {
        int index = Random.Range(0, points.Length);
        Vector2 pos = points[index].position;
        Box box = Spawn(StringConst.BOX, pos, Quaternion.identity) as Box;
        box.boxHealth.InitHealth();
    }

}
