using System.Collections;
using UnityEngine;
using HadesSDK;
using HadesSDK.Ads.Runtime;

public class AdsManager : MonoBehaviour
{
    private void Awake()
    {
        AdManager.Instance.Init();
    }
}