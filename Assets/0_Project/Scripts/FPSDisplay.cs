using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float _deltaTime = 0.0f;

    [SerializeField] private int _fontSize = 40;
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField] private bool _showInTopRight = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect;
        if (_showInTopRight)
        {
            rect = new Rect(w - 160, 10, 150, h * 2 / 100);
            style.alignment = TextAnchor.UpperRight;
        }
        else
        {
            rect = new Rect(10, 10, 150, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
        }

        style.fontSize = _fontSize;
        style.normal.textColor = _textColor;

        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;

        // string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        string text = string.Format("{1:0.}", msec, fps);

        GUIStyle shadowStyle = new GUIStyle(style);
        shadowStyle.normal.textColor = Color.yellow; ;
        GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width, rect.height), text, shadowStyle);
        GUI.Label(rect, text, style);
    }
}