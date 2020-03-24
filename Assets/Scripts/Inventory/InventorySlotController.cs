using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotController : MonoBehaviour {

    private int index = -1;
    private Transform m_Transform;

    private InventoryItemController item = null;

    void Start ()
    {
        m_Transform = this.GetComponent<Transform>();
    }

    public void InitData(int index)
    {
        this.index = index;
        this.name = "InventorySlot" + index;
    }

    public void SetData(InventoryItemController item)
    {
        this.item = item;
    }

    // 有Item拖拽进来slot
    private void OnDragItemIntoSlot(InventoryItemController item)
    {
        this.SetData(item);
        item.SetIndex(this.index);
        item.SetData(ItemDragContextEnum.Inventory);
    }

    // 有Item拖拽出去slot
    private void OnDragItemOutSlot()
    {
        this.SetData(null);
    }

    // 当前slot是否有Item
    public bool IsEmptySlot()
    {
        return this.item == null;
    }

    // 获取当前item
    public InventoryItemController GetItem()
    {
        return this.item;
    }
}
