using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGround()
    {
        return Physics2D.OverlapCircle(transform.position, radius, groundLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}