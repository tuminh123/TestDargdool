using HadesSDK.Ads.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    private static bool _initialized;

   /* private void Awake()
    {
        if (_initialized)
        {
            Destroy(gameObject);
            SceneLoadData.NextScene = null;
            return;
        }

        _initialized = true;
        DontDestroyOnLoad(gameObject);
    }
*/
    private void Start()
    {
        InitSystems();

        SceneLoadData.NextScene = StringConst.MAINMENUSCENE;
        SceneManager.LoadScene(StringConst.LOADINGSCENE);
    }

    void InitSystems()
    {
        //Invoke(nameof(Show), 1);
        // AudioManager.Init();
        // SaveSystem.Init();
        // AdsManager.Init();
    }

    private void Show()
    {
        if (AdManager.Instance.IsAoaReady())
            AdManager.Instance.ShowAoa();
    }
}
