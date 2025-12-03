using UnityEngine;
using UnityEngine.UI;


public abstract class ToggleBase : MonoBehaviour
{
    protected Toggle toggle;
    protected virtual void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    protected virtual void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }
    protected abstract void OnToggleChanged(bool isOn);
}
