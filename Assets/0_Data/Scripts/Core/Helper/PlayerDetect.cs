using System.Collections;
using UnityEngine;
public class PlayerDetect : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask playerLayer;
    public bool IsPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        return collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    //private void Awake()
    //{
    //    IsPlayer = false;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.CompareTag("Player")) IsPlayer = true;
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player")) IsPlayer = false;
    //}
}