using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VfxDeSpawn : MonoBehaviour
{
    [SerializeField] private float durationTime;
    private float time;
    private VfxBase vfxBase;

    private void Awake()
    {
        vfxBase = GetComponentInParent<VfxBase>();
    }

    private void Update()
    {
        time-=Time.deltaTime;
        if(time <= 0)
        {
            SingletonManager.Instance.vfxPoolManager.DeSpawn(vfxBase);
            time = durationTime;
        }
    }
}