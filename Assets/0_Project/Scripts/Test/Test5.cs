using PrimeTween;
using System.Collections;
using UnityEngine;


public class Test5 : ObjectController
{
   /* public Balance[] balances;

    private void Start()
    {
        foreach (var item in balances)
        {
            if (item == null) continue;
            item.SetIsTrigger(false);
        }
    }*/

    public string GetNameObj()
    {
        return "Test5";
    }

    protected override void OnPressed()
    {
        Debug.Log("Press");
    }

    protected override void OnTapped()
    {
        Debug.Log("Tap");
    }
}
