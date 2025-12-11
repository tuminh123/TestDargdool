using PrimeTween;
using System.Collections;
using UnityEngine;


public class Test5 : MonoBehaviour
{


        void Update()
        {
            Tween.ShakeCamera(Camera.main, 10, 0.5f, 20, 0, 0, false);

    } 
}
