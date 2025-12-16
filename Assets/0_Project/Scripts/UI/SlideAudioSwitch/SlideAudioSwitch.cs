using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PrimeTween;
using Popup;
using System.Resources;
public abstract class SlideAudioSwitch : MonoBehaviour, IPointerClickHandler
{

    [Header("References")]
    [SerializeField] protected RectTransform knob;
    [SerializeField] protected Image background;

    [Header("Position")]
    [SerializeField] protected float onX = -40f;   // Trái
    [SerializeField] protected float offX = 40f;   // Phải

    [Header("Color")]
    [SerializeField] protected Color onColor = new Color(1f, 0.8f, 0.2f);
    [SerializeField] protected Color offColor = new Color(0.2f, 0.25f, 0.35f);

    [Header("Animation")]
    [SerializeField] protected float moveDuration = 0.25f;
    [SerializeField] protected float scaleOn = 1.15f;
    [SerializeField] protected float scaleOff = 0.9f;

    protected bool isOn;

    private void Start()
    {
        LoadState();
        ApplyVisualInstant();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Click);
    }

    private void Toggle()
    {
        isOn = !isOn;
        SaveState();
        ApplyLogic();
        PlayAnimation();
    }

    public abstract void ApplyLogic();
    public abstract void LoadState();
    public abstract void SaveState();

    #region Tween animtion
    private void PlayAnimation()
    {
        Tween.StopAll(knob);
        Tween.StopAll(background);

        float targetX = isOn ? onX : offX;
        float targetScale = isOn ? scaleOn : scaleOff;
        Color targetColor = isOn ? onColor : offColor;

        // Slide (slow → fast)
        UtilitiesTween.TweenAnchoredX(knob, targetX, moveDuration, Ease.InOutBack, 1, CycleMode.Yoyo);

        // Scale popup feel
        UtilitiesTween.DoScaleRect(knob,false ,Vector3.one * targetScale, moveDuration, Ease.InOutBack);

        // Background color
        UtilitiesTween.DoSetImageColor(background,false, targetColor,moveDuration,Ease.InOutBack);
    }

    private void ApplyVisualInstant()
    {
        knob.anchoredPosition = new Vector2(isOn ? onX : offX, knob.anchoredPosition.y);
        knob.localScale = Vector3.one * (isOn ? scaleOn : scaleOff);
        background.color = isOn ? onColor : offColor;
    }
    #endregion
}
