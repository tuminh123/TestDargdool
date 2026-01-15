using UnityEngine;

public class BoxDetect : MonoBehaviour
{
    public event System.Action<Box> OnBoxDetect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Box box)) return;
        OnBoxDetect?.Invoke(box);
    }
}
