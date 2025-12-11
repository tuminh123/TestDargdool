using System.Collections;
using UnityEngine;
using Zenject;

public class UsePotion : MonoBehaviour
{
    [InjectOptional] private ItemPoolManager itemPoolManager;
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
            ZenManager.Instance. itemPoolManager.DeSpawn(potion);
        }
    }
}