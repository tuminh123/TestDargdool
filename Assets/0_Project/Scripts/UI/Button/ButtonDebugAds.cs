using UnityEngine;

public class ButtonDebugAds : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdk.SdkConfiguration sdkConfiguration) => {
            // Show Mediation Debugger
        MaxSdk.ShowMediationDebugger();
        };
    }
}
