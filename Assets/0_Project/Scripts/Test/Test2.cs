
using System.Collections;
using UnityEngine;

public class Test2 : ObjectController
{

    protected override void OnPressed()
    {
        Debug.Log("Press");
    }

    protected override void OnTapped()
    {
        Debug.Log("Tap");
    }
}