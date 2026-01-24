using System.Collections;
using UnityEngine;
public class PlayerDetect : MonoBehaviour
{
    public CharacterCtrl player { get;private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CharacterCtrl player)) return;
        this.player = player;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CharacterCtrl player)) return;
        this.player = player;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CharacterCtrl player)) return;
        this.player = null;
    }
}