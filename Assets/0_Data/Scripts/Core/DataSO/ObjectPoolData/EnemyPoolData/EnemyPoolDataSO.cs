using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Pool Data", menuName = "Data SO/Object Pool/Enemy/ Pool Data")]
public class EnemyPoolDataSO : DataPoolSO<EnemyAI>
{
    [SerializeField] private EnemyStat enemyStat;

    public EnemyStat EnemyStat=>enemyStat;
}
[System.Serializable]
public class EnemyStat
{
    [SerializeField] private float maxHP;
    [SerializeField] private float damageBase;
    [SerializeField] private float speed;
    [SerializeField] private float attackDuration;

    //get
    public float AttackDuration =>attackDuration;
    public float MaxHP => maxHP;
    public float DamageBase => damageBase;
    public float Speed => speed;
}