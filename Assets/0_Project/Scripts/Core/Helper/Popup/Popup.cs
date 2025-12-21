using PrimeTween;
using UnityEngine;

namespace Popup
{
    public class Popup : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        private void OnEnable()
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            TweenPopupOpen();
        }

        private void OnDisable()
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public void OpenPopup()
        {
            IsOpen = true;

            SingletonManager.Instance.soundManager.PlaySound(SoundType.Pop);
            GameEventBus.RaiseGamePause();

            gameObject.SetActive(true);
        }
        public void ClosePopup()
        {
            IsOpen = false;

            SingletonManager.Instance.soundManager.PlaySound(SoundType.Pop);
            GameEventBus.RaiseGameResume();

            TweenPopupClose();
            gameObject.SetActive(false);
        }

        private void TweenPopupOpen()
        {
            Tween.Scale(gameObject.transform, Vector3.one, 0.25f, Ease.OutBack);
        }

        private void TweenPopupClose()
        {
            Tween.Scale(gameObject.transform, Vector3.zero, 0.25f, Ease.InBack)
                .OnComplete(() => { gameObject.SetActive(false); });
        }
    }
}