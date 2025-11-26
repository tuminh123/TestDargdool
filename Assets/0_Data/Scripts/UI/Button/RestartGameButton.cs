using UnityEngine;

public class RestartGameButton : ButtonBase
{
    public override void Clicked()
    {
        Time.timeScale = 1;
        SceneLoader.Instance.LoadScene("GamePlay");
    }
}
