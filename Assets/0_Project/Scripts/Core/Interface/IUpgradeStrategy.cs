
public interface IUpgradeStrategy
{
    void Apply(PlayerData player, UpgradeData upgrade);
}