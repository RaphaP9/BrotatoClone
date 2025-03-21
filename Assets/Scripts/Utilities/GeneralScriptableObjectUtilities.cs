using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralScriptableObjectUtilities
{
    #region InventoryObjectSOs

    public static List<InventoryObjectSO> AppendInventoryObjectLists(List<List<InventoryObjectSO>> inventoryObjectsListsLists)
    {
        List<InventoryObjectSO> appendedList = new List<InventoryObjectSO>();

        foreach (List<InventoryObjectSO> inventoryObjectList in inventoryObjectsListsLists)
        {
            appendedList.AddRange(inventoryObjectList);
        }

        return appendedList;
    }

    #endregion

}
