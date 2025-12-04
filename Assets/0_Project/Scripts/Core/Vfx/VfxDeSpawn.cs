using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class VfxDeSpawn : MonoBehaviour
{
    [SerializeField] private float durationTime;
    private float time;
    private VfxBase vfxBase;

    private void Awake()
    {
        vfxBase = GetComponentInParent<VfxBase>();
        time = durationTime;
    }

    private void Update()
    {
        time-=Time.deltaTime;
        if(time <= 0)
        {
            VfxDeSpawnHandle();

            time = durationTime;
        }
    }

    private void VfxDeSpawnHandle()
    {
        SingletonManager.Instance.vfxPoolManager.DeSpawn(vfxBase);
        Transform holder = SingletonManager.Instance.vfxPoolManager.Holder;
        SingletonManager.Instance.vfxPoolManager.SetParent(vfxBase, holder);
    }
}