using System.Collections;
using UnityEngine;
public enum Faction { none=0,player =1,enemy=2, }
public class HealthBalance : MonoBehaviour
{
    [SerializeField] private float durationAffected;
    [SerializeField] private Faction faction;

    private HealthBase healthBase;
    private Balance balance;
    
    //get
    public HealthBase HealthBase => healthBase;
    public Balance Balance => balance;
    public Faction Faction => faction;
    private void Awake()
    {
        healthBase = transform.root.GetComponent<HealthBase>();
        balance = GetComponent<Balance>();
    }
    private void OnEnable()
    {
        healthBase.OnTakeDamage += OnTakeDamage;
    }
    private void OnDisable()
    {
        healthBase.OnTakeDamage -= OnTakeDamage;
    }
    private void OnTakeDamage()
    {
        StartCoroutine(SetBalanceTrigger());
    }
    private IEnumerator SetBalanceTrigger()
    {
        balance.SetIsTrigger(false);
        yield return new WaitForSeconds(durationAffected);
        balance.SetIsTrigger(true);
    }
   
}
