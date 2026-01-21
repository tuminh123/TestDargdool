using UnityEngine;

public class UpgradeCommand : IUpgradeCommand
{
    private UpgradeType type;
    private UpgradeService service;

    public UpgradeCommand(UpgradeType type, UpgradeService service)
    {
        this.type = type;
        this.service = service;
    }

    public bool Execute()
    {
        return service.TryUpgrade(type);
    }
}
