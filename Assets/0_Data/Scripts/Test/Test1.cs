using Core;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._0_Data.Scripts.Test
{
    public class Test1 : MonoBehaviour
    {

        [SerializeField] private Button _test;
        [SerializeField] private int damage;
        void Start()
        {
            _test.onClick.AddListener(()=>
            {
                Global.Send(new SignalTakeDamage() { damaged = damage });
                Global.Send(new SignalRun());
            });
        }
    }
}