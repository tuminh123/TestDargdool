
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private List<AttackBase> attackBases = new List<AttackBase>();

    [SerializeField] Balance body_2;
    //[SerializeField] AnimationCurve speedCurve;
    //public float speed;
    //private float time;

    Vector2 attackDir;
   
    private void FixedUpdate()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body_2.transform.position).normalized;
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
    {

        attackBases[0].AttackHandle(attackDir);

        //time += Time.fixedDeltaTime;
        //speed =  speedCurve.Evaluate(time);

        //SetTriggerBalance(false);

        //if(attackDir.x < 0)
        //{
        //    left_up_arm.SetRotation(-115);

        //    left_down_arm.SetRotation(50);
        //    left_hand.SetRotation(50);
        //    //body.SetRotation(30);
        //    //hip.SetRotation(30);
        //    //body_2.SetRotation(30);
           
        //}
        //else if(attackDir.x > 0)
        //{
        //    left_up_arm.SetRotation(115);

        //    left_down_arm.SetRotation(-50);
        //    left_hand.SetRotation(-50);
        //    //body.SetRotation(-30);
        //    //hip.SetRotation(-30);
        //    //body_2.SetRotation(-30);
        //}


        //left_up_arm.Rb.linearVelocity = attackDir * speed * Time.fixedDeltaTime;

        //left_down_arm.Rb.linearVelocity = attackDir * speed * Time.fixedDeltaTime;
        //left_hand.Rb.linearVelocity = attackDir * speed * Time.fixedDeltaTime;
        //body.Rb.linearVelocity = attackDir * speed * Time.fixedDeltaTime;
        ////hip.Rb.linearVelocity = attackDir * speed * Time.fixedDeltaTime;
        ////body_2.Rb.linearVelocity = attackDir * speed * Time.fixedDeltaTime;

        ////foreach (var item in balances)
        ////{
        ////    if (item == null) continue;
        ////    item.Rb.linearVelocity = attackDir * speed * Time.fixedDeltaTime;
        ////}


        yield return new WaitForSeconds(0.5f);

        attackBases[0].AttackEnd();

        ////time = 0;
        ////speed = speedCurve.Evaluate(time);

        //SetTriggerBalance(true);

        //left_up_arm.ResetData();
        //left_down_arm.ResetData();
        //left_hand.ResetData();
        //body.ResetData();
        //hip.ResetData();
        //body_2.ResetData();
    }

    //private void SetTriggerBalance(bool value)
    //{
    //    foreach (var item in balances)
    //    {
    //        if (item == null) continue;
    //        item.SetIsTrigger(value);
    //    }
    //}
}

