using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Popup.Popup popupLose;

    //get
    public Popup.Popup PopupLose => popupLose;

    private void Awake()
    {
        popupLose.gameObject.SetActive(false);
    }
}
