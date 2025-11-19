using System;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private GameObject body;
    private float speedMove;
    private Rigidbody2D rb;
    private Vector2 dir;
    private float timeMove;
    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StardMove();
    }

    private void FixedUpdate()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        dir = (mouseWorld - body.transform.position).normalized;
        if (Input.GetMouseButtonDown(0))
        {
            MoveHandle();
        }
        
    }
    
    private void StardMove()
    {
        timeMove = 0;
        speedMove = speedCurve.Evaluate(timeMove);
    }

    private void MoveHandle()
    {
        timeMove += Time.fixedDeltaTime;
        speedMove = speedCurve.Evaluate(timeMove);

        rb.linearVelocity = dir * speedMove*Time.fixedDeltaTime;
    }
}
