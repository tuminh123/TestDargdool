using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace Popup
{
    public static class UtilitiesTween
    {
        public static void DoScaleRect(this RectTransform rect, bool instant , Vector3 scale, float duration, Ease ease)
        {
            if (instant) rect.localScale = scale;
            else Tween.Scale(rect, new TweenSettings<Vector3>(scale, duration, ease, useUnscaledTime: true));
        }

        public static void DoSetImageColor(this Image image, bool instant , Color color, float duration, Ease ease)
        {
            if (instant) image.color = color;
            else Tween.Color(image, color, duration, ease, useUnscaledTime: true);
        }
        public static Tween TweenAnchoredY(this RectTransform rt, float targetY, float duration, Ease ease)
        {
            float startY = rt.anchoredPosition.y;
            return Tween.Custom(
                startY, targetY, duration, value =>
                {
                    var pos = rt.anchoredPosition;
                    pos.y = value;
                    rt.anchoredPosition = pos;
                }, ease, useUnscaledTime: true
            );
        }

        public static Tween TweenAnchoredX(this RectTransform rt, float targetX, float duration, Ease ease, int cycles, CycleMode cycleMode)
        {
            float startX = rt.anchoredPosition.x;
            return Tween.Custom(
                startX, targetX, duration, value =>
                {
                    var pos = rt.anchoredPosition;
                    pos.x = value;
                    rt.anchoredPosition = pos;
                }, ease, cycles, cycleMode,useUnscaledTime: true
            );
        }
    }
}