using TMPro;
using UnityEngine;

public abstract class TextBase : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(string text)
    {
        if (textUI == null) return;
        if (text.Length > 0)
        {
            textUI.text = text;
        }
        else
        {
            textUI.text = "";
        }
    }

    //public abstract string GetTextName();

}
