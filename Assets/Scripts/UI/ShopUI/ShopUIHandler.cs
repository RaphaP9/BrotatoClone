using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShopUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform shopInventoryObjectPrefab;
    [SerializeField] private Button rerollButton;
    [SerializeField] private Toggle lockShopToggle;

    public static event EventHandler OnRerollClick;
    public static event EventHandler<OnLockShopToggledEventArgs> OnLockShopToggled;

    public class OnLockShopToggledEventArgs : EventArgs
    {
        public bool isOn;
    }


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        InitializeLockShopToggle();
        InitializeListeners();
    }

    private void InitializeLockShopToggle()
    {
        lockShopToggle.isOn = false;
    }

    private void InitializeListeners()
    {
        rerollButton.onClick.AddListener(Reroll);
        lockShopToggle.onValueChanged.AddListener(ToggleShopLock);
    }

    private void Reroll()
    {
        OnRerollClick?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleShopLock(bool isOn)
    {
        OnLockShopToggled?.Invoke(this, new OnLockShopToggledEventArgs{isOn = isOn});
    }

}
