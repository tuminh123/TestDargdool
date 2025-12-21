using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PopupLose popupLose;
    [SerializeField] private PopupWin popupWin;
    private Popup.Popup currentPopup;
    //get
    public PopupLose PopupLose => popupLose;
    public PopupWin PopupWin => popupWin;

    private void Awake()
    {
        popupLose.gameObject.SetActive(false);
        popupWin.gameObject.SetActive(false);
    }
    public void OpenPopup(Popup.Popup popup)
    {
        CloseCurrentPopup();
        currentPopup = popup;
        currentPopup.OpenPopup();
    }

    public void CloseCurrentPopup()
    {
        if (currentPopup == null) return;

        currentPopup.ClosePopup();
        currentPopup = null;
    }
}
