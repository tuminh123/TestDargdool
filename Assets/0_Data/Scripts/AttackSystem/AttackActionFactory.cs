public static class AttackActionFactory
{
    public static IAttackAction Create(AttackProfile profile)
    {
        if (profile == null) return null;

        bool isRight = profile.side != AttackProfile.Side.Left;
        switch (profile.category)
        {
            case AttackProfile.Category.Arm: return new ArmAttackAction(isRight);
            case AttackProfile.Category.Elbow: return new ArmAttackAction(isRight); // reuse Arm with different angles via profile
            case AttackProfile.Category.Leg: return new LegAttackAction(isRight);
            case AttackProfile.Category.Pillow: return new PillowAttackAction(isRight);
            default: return new ArmAttackAction(isRight);
        }
    }
}