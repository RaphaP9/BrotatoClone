using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button sellButton;

    [Header("Runtime Filled")]
    [SerializeField] private PrimitiveInventoryObject selectedInventoryObject;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        sellButton.onClick.AddListener(HandleSell);
    }

    private void HandleSell()
    {

    }
}
