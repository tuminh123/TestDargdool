using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoadData
{
    public static string NextScene;
}

public class SceneLoader : MonoBehaviour
{

    public void LoadHomeScene()
    {
        LoadScene(StringConst.MAINMENUSCENE);
    }

    public void LoadGamePlayScene()
    {
        LoadScene(StringConst.GAMEPLAYSCENE);
    }
    public void LoadUpgradeScene()
    {
        LoadScene(StringConst.UPGRADESCENE);
    }

    public void LoadScene(string sceneName)
    {
        SceneLoadData.NextScene = sceneName;
        SceneManager.LoadScene(StringConst.LOADINGSCENE);
    }
}
