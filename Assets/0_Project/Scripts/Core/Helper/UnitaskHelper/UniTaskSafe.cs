using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public static class UniTaskSafe
{
    public static Action<Exception, string> OnException;

    public static void Forget(
        Func<CancellationToken, UniTask> taskFactory,
        CancellationToken token,
        string context = null)
    {
        Run(taskFactory, token, context).Forget();
    }

    private static async UniTask Run(
        Func<CancellationToken, UniTask> taskFactory,
        CancellationToken token,
        string context)
    {
        try
        {
            await taskFactory(token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            UnityEngine.Debug.LogError($"[UniTaskSafe] {context}\n{ex}");
#endif
            OnException?.Invoke(ex, context);
        }
    }
}
