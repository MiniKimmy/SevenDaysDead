using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelController : BaseController<InventoryPanelController> {

	private InventoryPanelView m_InventoryPanelView;
	private InventoryPanelModel m_InventoryPanelModel;

    private const int slotNum = 27;

	private InventorySlotController[] slotList = new InventorySlotController[slotNum];

    public override void Start ()
    {
        base.Start();
        this.m_InventoryPanelView = (InventoryPanelView)this.View;
        this.m_InventoryPanelModel = (InventoryPanelModel)this.Model;

        this.CreateAllSlots();
		this.CreateAllItems();
    }
	
    // 创建所有物品槽Slot
	private void CreateAllSlots()
    {
		for (int i = 0; i < slotNum; i ++)
		{
            var prefab = this.m_InventoryPanelView.GetPrefabByDict(GUIName.InventorySlot);
            var parent = this.m_InventoryPanelView.GridTransform;
            var item = GameObject.Instantiate<GameObject>(prefab, parent).GetComponent<InventorySlotController>();
            item.InitData(i);
            this.slotList[i] = item;
		}
	}

    // 创建所有物品Item
	private void CreateAllItems()
    {		
		var list = this.m_InventoryPanelModel.GetJsonList("InventoryJsonData");

		for (int i = 0; i < list.Count; i ++)
		{
            var data = list[i];
            var slotItem = this.GetSlot(i);
            var prefab = this.m_InventoryPanelView.GetPrefabByDict(GUIName.InventoryItem);

            var item = UtilsBase.Clone<InventoryItemController>(prefab, slotItem.transform);
            item.InitData(i, data);
			item.SetData(ItemDragContextEnum.Inventory);
            slotItem.SetData(item);
        }
	}

    // 找到有空位置的下标
    private int GetEmptyIndex(int exceptIndex = -1)
    {
        for (int i = 0; i < slotNum; i++)
        {
            var slot = this.GetSlot(i);
            if (slot.IsEmptySlot() && i != exceptIndex)
                return i;
        }

        return -1;
    }

    // 拆分材料event
    private void BreakMaterialsEvent(params object[] args)
    {
        InventoryItemController dragItem = (InventoryItemController)args[0];
        var data = dragItem.GetData();

        if (data.DragContext == ItemDragContextEnum.Crafting) return;

        int newIndex = this.GetEmptyIndex(dragItem.GetIndex());
        if (newIndex == -1)
        {
            UtilsBase.ddd("not empty space Inventory");
            return; // 若没有空位
        }

        int newNum = (int)args[1];
        var newItem = InventoryItemController.Clone (
            dragItem.gameObject, 
            this.slotList[newIndex].transform, 
            new InventoryItem(data.ItemId, data.ItemName, newNum),
            newIndex
        );

        this.GetSlot(newIndex).SetData(newItem);
        newItem.SetData(ItemDragContextEnum.Inventory);

        data.ItemNum -= newNum;
        dragItem.SetData(data.DragContext);
    }

    // 获取某个slot
    public InventorySlotController GetSlot(int goIndex)
    {
        return this.slotList[goIndex];
    }

    // 找到第一个匹配goItemId的slot下标
    public int GetItem(int goItemId)
    {
        for (int i = 0; i < slotNum; i++)
        {
            var item = this.GetSlot(i).GetItem();
            if (item != null && item.GetData().ItemId == goItemId)
                return i;
        }

        return -1;
    }

    // 回收所有的合成材料到背包
    private void ResetMaterialsToInventory(List<InventoryItemController> goList)
    {
        int n = goList.Count;
        InventoryItemController[] tmp = new InventoryItemController[n];

        // 合并同种材料再放回背包的同类物品上.
        goList.Sort((a, b) => {
            return a.GetData().ItemId.CompareTo(b.GetData().ItemId);
        });

        // 去重unique
        int j = 0;
        for(int i = 0; i < n; i ++)
        {
            var data = goList[i].GetData();
            if (i == 0 || goList[i - 1].GetData().ItemId != data.ItemId)
                tmp[j ++] = goList[i];
            else
            {
                tmp[j - 1].GetData().ItemNum += data.ItemNum;
                GameObject.Destroy(goList[i].gameObject);
            }
        }

        // 重置到背包(优先和背包内同类物品叠加)
        while (-- j >= 0)
        {
            var item = tmp[j];
            var data = item.GetData();
            int preIndex = this.GetItem(data.ItemId);
            int emptyIndex = this.GetEmptyIndex();

            if (preIndex != -1)
            {
                var preItem = this.GetSlot(preIndex).GetItem();
                data.ItemNum += preItem.GetData().ItemNum;
                GameObject.Destroy(preItem.gameObject);
                preIndex = Math.Min(preIndex, emptyIndex);
            }
            emptyIndex = Math.Max(emptyIndex, preIndex);

            var slot = this.GetSlot(emptyIndex);
            slot.SetData(item);
            item.SetData(ItemDragContextEnum.Inventory);
            item.SetSlotParent(slot.transform);
            item.ResetSelf();
        }
    }

    protected override void InitEventListener()
    {
        EventManager.Instance.AddListener(EventName.BreakMaterials, (args) => this.BreakMaterialsEvent(args));
    }

    protected override void RemoveEventListener()
    {
        EventManager.Instance.RemoveListener(EventName.BreakMaterials, this.BreakMaterialsEvent);
    }
}
