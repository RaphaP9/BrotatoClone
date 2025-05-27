using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollPriceTextHandler : PriceTextHandler
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ShopManager.OnRerollCostSet += ShopManager_OnRerollCostSet;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ShopManager.OnRerollCostSet -= ShopManager_OnRerollCostSet;
    }

    protected override int GetPrice() => ShopManager.Instance.CurrentRerollCost;

    #region Subscriptions
    private void ShopManager_OnRerollCostSet(object sender, ShopManager.OnRerollCostEventArgs e)
    {
        UpdatePriceTag();
        UpdatePriceColor();
    }
    #endregion
}
