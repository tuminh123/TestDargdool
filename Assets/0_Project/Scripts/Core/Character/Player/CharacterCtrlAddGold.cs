using UnityEngine;
using Zenject;

public class CharacterCtrlAddGold : MonoBehaviour
{
    [InjectOptional]
    private ItemPoolManager itemPoolManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Gold gold))
        {
            SingletonManager.Instance.goldManager.AddGold(1);
            ZenManager.Instance. itemPoolManager.DeSpawn(gold);
            //SingletonManager.Instance.dataManager.DataSave();
        }
    }

}
