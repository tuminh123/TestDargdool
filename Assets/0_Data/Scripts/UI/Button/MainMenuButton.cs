using UnityEngine;

public class MainMenuButton : ButtonBase
{
    public override void Clicked()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
