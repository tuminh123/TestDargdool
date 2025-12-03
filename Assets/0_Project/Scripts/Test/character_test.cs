using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Action
{
    public action_type type;
    public List<ActionDataSO> actionDataList;

    public ActionDataSO GetActionData(string actionName)
    {
        foreach (var item in actionDataList)
        {
            if (item == null) continue;
            if (item.actionName == actionName) return item;
        }
        return null;
    }

    public BalanceData GetBalanceData( string actionName, BalanceType type)
    {
        ActionDataSO actionData = GetActionData(actionName);
        if (actionData == null) return null;
        return actionData.GetBalanceData(type);
    }

}
public class character_test : MonoBehaviour
{
    public Action[] actions;
    public Balance[] balances;

    public character_attack attack_state;
    public character_idle idle_state;
    public StateMachine stateMachine;

    public Vector2 attackDir;
    private void Awake()
    {
        stateMachine = new StateMachine();
        idle_state = new character_idle(this,stateMachine,"idle_1",action_type.idle);
        attack_state = new character_attack(this,stateMachine,"punch_right",action_type.attack);
    }
    private void Start()
    {
        stateMachine.InitState(idle_state);
    }
    private void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        attackDir = ( mouse - GetBalance(BalanceType.body_bottom).transform.position ).normalized; 

        stateMachine.UpdateState();
    }
    private void FixedUpdate()
    {
        stateMachine.UpdatePhysicState();
    }
    private void Reset()
    {
        balances = GetComponentsInChildren<Balance>();
    }

    public void SetVelocity()
    {
        foreach (Balance b in balances)
        {
            if (b == null) continue;
            b.Rb.linearVelocity = attackDir * 10;
        }
    }
    public void SetTrigget(bool value)
    {
        foreach (Balance b in balances)
        {
            if (b == null) continue;
            b.SetIsTrigger(value);
        }
    }

    #region GetData
    public Action GetAction(action_type type)
    {
        foreach (var item in actions)
        {
            if (item == null) continue;
            if (item.type == type) return item;
        }
        return null;
    }

    public BalanceData GetBalanceData(action_type type, string actionName, BalanceType balanceType)
    {
        Action action = GetAction(type);
        if(action == null) return null;

        BalanceData balanceData = action.GetBalanceData(actionName, balanceType);
        if(balanceData == null) return null;

        return balanceData;
    }
    public Balance GetBalance(BalanceType type)
    {
        foreach (var item in balances)
        {
            if (item == null) continue;
            if (item.DataSO.Type == type) return item;
        }
        return null;
    }
    public void Set(action_type action_type, string actionName, BalanceType type)
    {
        BalanceData balanceData = GetBalanceData(action_type,actionName, type);

        if (balanceData == null) return;
        float rot = balanceData.rotChange;
        float force = balanceData.forceChange;

        Balance balance = GetBalance(type);

        if (balance == null) return;

        balance.SetRotation(rot);
        balance.SetForce(force);
    }


    public void ResetBalance(BalanceType type)
    {
        Balance balance = GetBalance(type);
        balance.ResetData();
    }
    #endregion
}
