using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerDetect : MonoBehaviour
{
    public bool IsPlayer { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) IsPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) IsPlayer = false;
    }
}