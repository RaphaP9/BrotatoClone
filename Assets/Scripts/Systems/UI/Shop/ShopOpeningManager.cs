using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.PackageManager;

public class ShopOpeningManager : MonoBehaviour
{
    public static ShopOpeningManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private bool isOpen;

    public static event EventHandler OnShopOpen;
    public static event EventHandler OnShopClose;

    public static event EventHandler OnShopOpenInmediately;
    public static event EventHandler OnShopCloseInmediately;

    public static event EventHandler OnShopClosedFromUI;

    public bool IsOpen => isOpen;

    private void OnEnable()
    {
        ShopUI.OnShopClosedFromUI += ShopUI_OnShopClosedFromUI;
    }

    private void OnDisable()
    {
        ShopUI.OnShopClosedFromUI -= ShopUI_OnShopClosedFromUI;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShopOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void OpenShopInmediately()
    {
        OnShopOpenInmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }
    private void CloseShopInmediately()
    {
        OnShopCloseInmediately?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }

    public void OpenShop()
    {
        OnShopOpen?.Invoke(this, EventArgs.Empty);
        SetIsOpen(true);
    }

    private void CloseShop()
    {
        OnShopClose?.Invoke(this, EventArgs.Empty);
        SetIsOpen(false);
    }

    private bool SetIsOpen(bool isOpen) => this.isOpen = isOpen;

    private void CloseShopFromUI()
    {
        CloseShop();
        OnShopClosedFromUI?.Invoke(this, EventArgs.Empty);
    }


    #region Subscriptions
    private void ShopUI_OnShopClosedFromUI(object sender, EventArgs e)
    {
        CloseShopFromUI();
    }
    #endregion
}
