using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Zenject;

public class VfxDeSpawn : MonoBehaviour
{
    [SerializeField] private VfxPoolManager vfxPoolManager;
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
            

            time = durationTime;
        }
    }

}