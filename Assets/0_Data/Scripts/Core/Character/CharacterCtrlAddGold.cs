using UnityEngine;

public class CharacterCtrlAddGold : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Gold gold))
        {
            SingletonManager.Instance.goldManager.AddGold(1);
            SingletonManager.Instance.objInGamePoolManager.DeSpawn(gold);
            //SingletonManager.Instance.dataManager.DataSave();
        }
    }

}
