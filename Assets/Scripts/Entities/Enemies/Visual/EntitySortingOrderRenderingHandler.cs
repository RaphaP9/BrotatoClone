using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntitySortingOrderRenderingHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform renderingRefference;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Canvas UICanvas;

    private int previousSortingOrder;

    private const int DECIMAL_PRECISION = 2;

    private void Start()
    {
        SetPreviousSortingOrder(0);
    }

    private void Update()
    {
        HandleSortingOrder();
    }

    private void HandleSortingOrder()
    {
        int newSortingOrder = CalculateSortingOrderDueToPosition();

        if (newSortingOrder == previousSortingOrder) return;

        SetSpriteRendererSortingOrder(newSortingOrder);
        SetUISortingGroupSortingOrder(newSortingOrder + 1); //UI on top
        SetPreviousSortingOrder(newSortingOrder);
    }

    private int CalculateSortingOrderDueToPosition()
    {
        int sortingOrder = Mathf.RoundToInt(-renderingRefference.position.y * Mathf.Pow(10f, DECIMAL_PRECISION +1));
        return sortingOrder;
    }


    private void SetPreviousSortingOrder(int sortingOrder) => previousSortingOrder = sortingOrder;
    private void SetSpriteRendererSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
      
    private void SetUISortingGroupSortingOrder(int sortingOrder) => UICanvas.sortingOrder = sortingOrder;

}
