using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance{get; private set;}
    public float xInput {get; private set;}
    public bool jumpInput{get; private set;}
    public bool attackInput{get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SetInput();
    }

    public void SetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        attackInput = Input.GetMouseButtonDown(0);
    }
    
}
