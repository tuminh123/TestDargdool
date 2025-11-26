using UnityEngine;

public class PlayButton : ButtonBase
{
    public override void Clicked()
    {
        SceneLoader.Instance.LoadScene("GamePlay");
    }
}
