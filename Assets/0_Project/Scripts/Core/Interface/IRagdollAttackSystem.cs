using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

public interface IRagdollAttackSystem
{
    public event System.Action OnAttackEnd;
    public UniTask ExecuteAttack(CancellationToken token, Vector2 dir, IPostAction postBase, string nameAction);
}