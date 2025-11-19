using System;
using UnityEngine;

public class RotationCruve : MonoBehaviour
{
    public AnimationCurve rotateCurve;
    public float angle;
    public float curveDuration = 0.5f;
    public float maxAngle = 90f;

    float timer;
    bool playing;
    [SerializeField]Rigidbody2D rb;

    private void Start()
    {
        PlayRotateCurve();
    }

    private void FixedUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.R)) Rotation();
            
    }

    public void PlayRotateCurve()
    {
        timer = 0;
        angle = rotateCurve.Evaluate(timer);
    }

    public void Rotation()
    {
        timer += Time.fixedDeltaTime;
        float t = timer / curveDuration;

        float percent = rotateCurve.Evaluate(t);
        float angle = percent * maxAngle;
        
        rb.MoveRotation(angle);
    }
 
}