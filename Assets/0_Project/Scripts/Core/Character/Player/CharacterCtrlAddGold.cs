using Core;
using UnityEngine;
using Zenject;

public class CharacterCtrlAddGold : MonoBehaviour
{
    [InjectOptional]
    private ItemPoolManager itemPoolManager;
    private int count = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Gold gold))
        {
            int goldCount = 1;
            if (SingletonManager.Instance == null || SingletonManager.Instance.goldManager == null) return;
            SingletonManager.Instance.goldManager.AddGold(goldCount);

            Global.Send(new SignalGoldReceived { receivedGoldCount = goldCount });

            if (ZenManager.Instance == null || ZenManager.Instance.itemPoolManager == null) return;
            ZenManager.Instance. itemPoolManager.DeSpawn(gold);
            count++;
            //Debug.Log(goldCount);
            //SingletonManager.Instance.dataManager.DataSave();
        }
    }

}
