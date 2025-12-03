using Core;
using System.Collections;
using UnityEngine;

namespace Assets._0_Data.Scripts.Test
{
    public struct SignalTakeDamage
    {
        public int damaged;
    }
    public class Test3 : GameElement, 
        IReceive<SignalTakeDamage>
    {
        public void Receive(in SignalTakeDamage signal)
        {
            TakeDamage(signal.damaged);
        }

        public void TakeDamage(int damage)
        {

        }
    }
}