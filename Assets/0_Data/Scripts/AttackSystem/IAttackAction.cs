using System.Collections;

public interface IAttackAction
{
    // Returns IEnumerator to be started as coroutine.
    IEnumerator Execute(AttackContext ctx, AttackProfile profile);
}
