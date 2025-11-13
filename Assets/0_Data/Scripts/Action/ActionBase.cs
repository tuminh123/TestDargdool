using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    protected CharacterCtrl characterCtrl;
    protected virtual void Awake()
    {
        characterCtrl = GetComponent<CharacterCtrl>();

    }
}
