using UnityEngine;

public enum Faction { none = 0,player = 1,enemy = 2}
public interface IDamageFaction
{
    public void SetFaction(Faction faction);
}
