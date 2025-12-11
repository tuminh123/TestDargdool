using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float strength = 10f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float frequency = 10f;
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float endDelay = 0f;
    public void ShakeCam()
    {
        Tween.ShakeCamera(Camera.main,strength,duration,frequency,startDelay,endDelay, false);
    }
}
