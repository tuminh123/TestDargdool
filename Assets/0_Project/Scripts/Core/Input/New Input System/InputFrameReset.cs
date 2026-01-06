using UnityEngine;

public class InputFrameReset : MonoBehaviour
{
    private void LateUpdate()
    {
        InputContext.ResetFrame();
    }

}
