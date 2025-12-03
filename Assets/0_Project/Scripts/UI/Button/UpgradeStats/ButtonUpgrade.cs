using UnityEngine;

public enum UpgradeType
{
    NONE = 0,
    HEALTH = 1,
    DAMAGEBASE = 2,
}

public class ButtonUpgrade : ButtonBase
{
    [SerializeField] private UpgradeType type;
    public override void Clicked()
    {
        switch (type)
        {
            default:
            case UpgradeType.HEALTH:
                CalculateMaxHealth(10);
                break;
            case UpgradeType.DAMAGEBASE:
                CalculateDamageBase(10);
                break;
        }
    }
    public void CalculateMaxHealth(float add)
    {
        if (SingletonManager.Instance.goldManager.MinusGold(10))
        {
            SingletonManager.Instance.dataManager.Data.maxHp += add;
            SingletonManager.Instance.dataManager.DataSave();
        }
    }
    public void CalculateDamageBase(float add)
    {
        if (SingletonManager.Instance.goldManager.MinusGold(10))
        {
            SingletonManager.Instance.dataManager.Data.damageBase += add;
            SingletonManager.Instance.dataManager.DataSave();
        } 
    }
}
