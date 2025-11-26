using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Hàm gọi từ button hoặc code
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Bắt đầu load scene theo dạng async
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Không cho scene kích hoạt ngay lập tức
        operation.allowSceneActivation = false;

        // Vòng lặp chờ load
        while (!operation.isDone)
        {
            // operation.progress trả về từ 0 đến 0.9
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Khi load xong 90% → cho phép scene mới kích hoạt
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null; // chờ frame tiếp theo
        }
    }
}
