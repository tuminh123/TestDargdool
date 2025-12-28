using UnityEngine;

[CreateAssetMenu(menuName = "Progression/Simple Exp Curve")]
public class SimpleExpCurve : ScriptableObject, IExpCurve
{
    [SerializeField] private int baseExp = 100;
    [SerializeField] private float multiplier = 1.25f;
    [SerializeField] private int maxLevel = 100;  

    public int GetExpToNextLevel(int level)
    {
        if (level >= maxLevel) return int.MaxValue;
        return Mathf.RoundToInt(baseExp * Mathf.Pow(multiplier, level - 1));
    }
}
