using UnityEngine;
public class DamageDetect : MonoBehaviour
{
    [SerializeField]private float radius;
    [SerializeField] private LayerMask layer;

    //get
    public float Radius => radius;
    public LayerMask Layer => layer;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
