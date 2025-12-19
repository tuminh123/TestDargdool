using UnityEngine;

public static class LayerMaskExtensions
{
    /// <summary>
    /// So sanh layer - !=0 la co chua layer, ==0 la khong chua layer
    /// </summary>

    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }
    public static void SetTargetLayer(LayerMask targetLayer, string layerName)
    {
        targetLayer = LayerMask.GetMask(layerName);
    }
}
