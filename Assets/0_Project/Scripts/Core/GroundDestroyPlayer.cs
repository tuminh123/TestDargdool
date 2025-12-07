using System.Collections;
using UnityEngine;

public class GroundDestroyPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out CharacterParent ctrl))
        {
            ctrl.OnDead();
        }
    }
}