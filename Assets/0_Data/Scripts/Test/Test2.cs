using Core;
using System.Collections;
using UnityEngine;

namespace Assets._0_Data.Scripts.Test
{
    public struct SignalTest
    {
        public int Damage;
    }
    public struct SignalRun
    {

    }


    public class Test2 : GameElement,
        IReceive<SignalTest>,
        IReceive<SignalRun>
    {
        public int _hp = 100;
        public void Receive(in SignalTest signal)
        {
            Damage(signal.Damage);
        }
        public void Damage(int damage)
        {
            _hp -= damage;
            Global.Send(new SignalTakeDamage() { damaged = damage });
        }
        public void Run()
        {
            Debug.Log("run");
        }

        public void Receive(in SignalRun signal)
        {
            Run();
        }
    }
}