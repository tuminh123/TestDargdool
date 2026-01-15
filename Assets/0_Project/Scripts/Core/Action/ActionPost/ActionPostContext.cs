using System.Collections;
using UnityEngine;

public class ActionPostContext 
{
    private IPostAction postAction;

    public ActionPostContext(IPostAction postAction)
    {
        this.postAction = postAction;
    }
    public void SetPostAction(IPostAction postAction)
    {
        this.postAction = postAction;
    }
    public void GetAction(string name)
    {
        this.postAction.SetAction(name);
    }
}