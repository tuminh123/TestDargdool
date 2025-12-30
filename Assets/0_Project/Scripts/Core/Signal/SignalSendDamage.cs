using System.Collections;
using UnityEngine;
public enum Character
{
    None = 0,
    Player = 1,
    Enemy = 2,
}
public struct SignalSendDamage
{
    //public Character character;
    public float damaged;
}