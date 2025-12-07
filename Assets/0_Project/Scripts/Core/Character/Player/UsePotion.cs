using System.Collections;
using UnityEngine;

public class UsePotion : MonoBehaviour
{
    public CharacterCtrl ctrl { get; private set; }
    private void Awake()
    {
        ctrl = GetComponentInParent<CharacterCtrl>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PotionBase potion))
        {
            potion.Use(ctrl.gameObject);
            SingletonManager.Instance.objInGamePoolManager.DeSpawn(potion);
        }
    }
}