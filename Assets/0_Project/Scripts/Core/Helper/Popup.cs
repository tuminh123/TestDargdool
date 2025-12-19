using PrimeTween;
using UnityEngine;

namespace Popup
{
    public class Popup : MonoBehaviour
    {
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
            gameObject.SetActive(true);

            SingletonManager.Instance.soundManager.PlaySound(SoundType.Pop);
            GameEventBus.RaiseGamePause();
        }
        public void ClosePopup()
        {
            TweenPopupClose();

            SingletonManager.Instance.soundManager.PlaySound(SoundType.Pop);
            GameEventBus.RaiseGameResume();
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