using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    protected Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(Clicked);
    }
    public virtual void Clicked()
    {
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Click);
    }
}
