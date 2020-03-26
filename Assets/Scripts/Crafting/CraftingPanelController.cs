using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AutoSingleton(true, "CraftingPanel")]
public class CraftingPanelController : BaseController<CraftingPanelController>, IUIPanelShowAndHide {

	private CraftingPanelView m_CraftingPanelView;
	private CraftingPanelModel m_CraftingPanelModel;

	private static int tabsNum = 0;
	private const int SLOT_NUM = 25;

	private List<CraftingTabsItemController> tabsList = new List<CraftingTabsItemController>();   // 合成列表所有选项卡
	private List<CraftingContentController> contentsList = new List<CraftingContentController>(); // 合成列表所有正文内容
	private List<CraftingSlotController> slotList = new List<CraftingSlotController>();           // 合成图谱的所有slot槽
    private List<InventoryItemController> itemList = new List<InventoryItemController>();         // 合成图谱里的所有合成材料

	private int currentTabIndex = -1;                          // 当前选项卡下标
    private CraftingContentsItem currentContent = null;        // 当前图谱数据
    private CraftingRightController m_CraftingRightController; // 合成区ctrl

    public override void Start ()
    {
        base.Start();

        this.m_CraftingPanelView = (CraftingPanelView)this.View;
        this.m_CraftingPanelModel = (CraftingPanelModel)this.Model;
        this.m_CraftingRightController = this.View.Transform.Find("Right").GetComponent<CraftingRightController>();

        this.CreateAllTabs();
		this.CreateAllContents();
		this.CreateAllSlots();

		this.ResetTabsAndContents(0); // 默认选择第一个选项卡.
	}

	// 创建所有选项卡
	private void CreateAllTabs()
    {
        var tab_nams = this.m_CraftingPanelModel.GetTabsIconName();
        tabsNum = tab_nams.Length;

        for (int i = 0; i < tabsNum; i ++ )
        {
            var prefab = this.m_CraftingPanelView.GetPrefabByDict(GUIName.CraftingTabsItem);
            var parent = this.m_CraftingPanelView.Tabs_Transform;
            var item = GameObject.Instantiate<GameObject>(prefab, parent).GetComponent<CraftingTabsItemController>();
            Sprite sprite = UtilsBase.ByNameGetAsset<Sprite>(tab_nams[i], this.m_CraftingPanelView.TabIconDic);
			item.InitItem(i, sprite);
			tabsList.Add(item);
		}
	}

	// 生成所有正文
	private void CreateAllContents()
    {
		var list = this.m_CraftingPanelModel.ByNameGetJsonData("CraftingContentsJsonData");

		for (int i = 0; i < tabsNum; i ++){
            var prefab = this.m_CraftingPanelView.GetPrefabByDict(GUIName.CraftingContent);
            var parent = this.m_CraftingPanelView.Contents_Transform;
            var item = GameObject.Instantiate<GameObject>(prefab, parent).GetComponent<CraftingContentController>();

            var goPrefab = this.m_CraftingPanelView.GetPrefabByDict(GUIName.CraftingContentItem);
            item.InitItem(i, goPrefab, list[i]);
			contentsList.Add(item);
		}
	}
	
	// 创建所有合成槽
	private void CreateAllSlots()
    {
		for (int i = 0; i < SLOT_NUM; i ++)
		{
            var prefab = this.m_CraftingPanelView.GetPrefabByDict(GUIName.CraftingSlot);
            var parent = this.m_CraftingPanelView.Center_Transform;
            var item = GameObject.Instantiate<GameObject>(prefab, parent).GetComponent<CraftingSlotController>();
			this.slotList.Add(item);
		}
	}

	// 刷新合成图谱.
	private void ResetAllSlotContents(CraftingContentsItem data)
    {
        this.currentContent = data;

        this.ResetSlotContents();
        this.ResetAllItems();
        this.CreateAllSlotContents(data);
	}

    // 根据合成id生成合成图谱.
    private void CreateAllSlotContents(CraftingContentsItem data)
    {
        int id = this.currentContent.ItemID;
        var mapItem = this.m_CraftingPanelModel.GetItemById(id);
        this.m_CraftingRightController.SetData(mapItem);
 
        if (mapItem == null) return;
		for (int i = 0; i < SLOT_NUM; i ++){
            var item = this.slotList[i];
            int materialID = mapItem.GetMaterialsID(i);
            item.InitData(i, mapItem);
            if (materialID != 0)
			{
				string spriteName = materialID.ToString();
                Sprite sprite = UtilsBase.ByNameGetAsset<Sprite>(spriteName, this.m_CraftingPanelView.MaterialIconDic);
                item.SetData(sprite);
			}
		}
    }

	// 重置合成图谱
	private void ResetSlotContents()
    {
		for (int i = 0; i < SLOT_NUM; i ++)
            this.GetSlot(i).ResetSelf();
	}

    // 回收所有物品到背包
    private void ResetAllItems()
    {
        // 将所有合成材料回收到背包
        if (this.itemList.Count > 0)
        {
            var goList = this.itemList;
            this.itemList = new List<InventoryItemController>();
            SendMessageUpwards("ResetItemsToInventory", goList);
        }

        // 将生成的物品回收到背包
        var item = this.m_CraftingRightController.GetProduct();
        if (item != null)
        {
            this.m_CraftingRightController.ResetData();
            SendMessageUpwards("ResetItemToInventory", item);
        }
    }

    // 点击处理.(选项卡切换)
    private void ResetTabsAndContents(int index)
    {
		if (index == this.currentTabIndex) return;

		for (int i = 0; i < this.tabsList.Count; i ++ ) {
			this.tabsList[i].NormalTab();
			UtilsUI.SetScaleActive(contentsList[i].transform, false);
		}
	
		this.tabsList[index].ActiveTab();
		UtilsUI.SetScaleActive(this.contentsList[index].transform, true);
		this.currentTabIndex = index;
	}

    // Item拖入Slot
    private void OnDragItemIntoSlot(InventoryItemController item)
    {
        int newIdx = item.GetIndex();
        var curItem = this.itemList.Find((a) => a.GetIndex() == newIdx);

        // 重叠物品
        if (curItem != null)
        {
            int emptyIdx = this.GetEmptyIndex(item.GetData().ItemId);
            if (emptyIdx == -1) return;

            var slot = this.GetSlot(emptyIdx);
            item.SetSlotParent(slot.transform);
            item.SetIndex(emptyIdx);
            item.ResetSelf();
        }
        
        this.itemList.Add(item);
        var mapItem = this.m_CraftingRightController.GetData();
        this.m_CraftingRightController.MaterialCountUpdateEvent.Fire(this.GetCount() >= mapItem.MaterialsCount);
    }

    // Item离开Slot
    private void OnDragItemOutSlot(InventoryItemController item)
    {
        this.itemList.Remove(item);
    }

    // 获得某个slot
    public CraftingSlotController GetSlot(int index)
    {
        return this.slotList[index];
    }

    // 获取空slot位置, expectIndex 期望放在哪个slot
    private int GetEmptyIndex(int goItemID, int expectIndex = -1)
    {
        var mapItem = this.m_CraftingRightController.GetData();
        var mapStr = mapItem.MapContentsToChar();

        // 标记所有已放入slot的为"0"
        foreach (var item in this.itemList)
        {
            int idx = item.GetIndex();
            mapStr[idx] = "0";
        }
        
        // 若期望的位置是空就放入
        if (expectIndex != -1 && mapStr[expectIndex] != "0") return expectIndex;

        // 寻找第一个空slot可放入的下标
        string itemID = goItemID.ToString();
        for (int i = 0; i < SLOT_NUM; i++)
        {
            if (mapStr[i] != "0" && mapStr[i] == itemID)
                return i;
        }

        return -1;
    }

    // 获取当前图谱cfg
    private CraftingMapItem GetCurMapItem()
    {
        var data = this.currentContent;
        int id = data.ItemID;

        return this.m_CraftingPanelModel.GetItemById(id);
    }

    // 拆分材料
    private void BreakMaterialsEvent(params object[] args)
    {
        InventoryItemController dragItem = (InventoryItemController)args[0];
        var data = dragItem.GetData();

        if (data.DragContext == ItemDragContextEnum.Inventory) return;

        int materialID = data.ItemId;
        var mapItem = this.m_CraftingRightController.GetData();

        int need = mapItem.GetNeedCount(materialID);
        int has = this.GetCount(materialID);

        if (has + 1 >= need)
        {
            UtilsBase.ddd("crafting is enough. do not need break any more!");
            return; // 已经足够, 无需拆分
        }

        int newIndex = this.GetEmptyIndex(materialID, dragItem.GetIndex());
        int newNum = (int)args[1];
        var newItem = InventoryItemController.Clone(
            dragItem.gameObject,
            this.slotList[newIndex].transform,
            new InventoryItem(data.ItemId, data.ItemName, newNum),
            newIndex
        );

        this.itemList.Add(newItem); // 存入list
        newItem.SetData(ItemDragContextEnum.Crafting);

        data.ItemNum -= newNum;
        dragItem.SetData(data.DragContext);
    }

    // 获取当前已放置的材料数量
    private int GetCount(int materialID = -1)
    {
        if (materialID == -1) return this.itemList.Count;

        int res = 0;
        foreach (var item in this.itemList)
            if (item.GetData().ItemId == materialID)
                res ++;

        return res;
    }

    // 单个合成处理.
    private void Craft()
    {
        var mapItem = this.m_CraftingRightController.GetData();
        int has = this.GetCount();
        int need = mapItem.MaterialsCount;

        if (has < need)
        {
            UtilsBase.ddd("craft materials is not enough!");
            return;
        }

        // 生产产品
        var product = this.m_CraftingRightController.GetProduct();
        if (product == null) this.m_CraftingRightController.MakeGoodItem();
        else {
            var data = product.GetData();
            data.ItemNum++;
            product.SetData(data.DragContext);
        }

        // clean
        List<InventoryItemController> tmp = new List<InventoryItemController>();
        foreach (var item in this.itemList)
        {
            var data = item.GetData();
            data.ItemNum --;
            if (data.ItemNum <= 0){
                tmp.Add(item);
            } else {
                item.SetData(data.DragContext);
            }
        }

        this.RunOutOfMaterials(tmp);
        this.m_CraftingRightController.MaterialCountUpdateEvent.Fire(this.GetCount() >= need);
    }

    // 批量合成
    private void CraftAll()
    {
        var mapItem = this.m_CraftingRightController.GetData();
        int has = this.GetCount();
        int need = mapItem.MaterialsCount;

        if (has < need)
        {
            UtilsBase.ddd("craftall materials is not enough!");
            return;
        }

        // 计算生产次数
        int makeNum = 0x3f3f3f3f;
        foreach (var item in this.itemList)
        {
            var data = item.GetData();
            makeNum = Mathf.Min(makeNum, data.ItemNum);
        }

        // 生产产品
        this.m_CraftingRightController.MakeGoodItem(makeNum);

        // clean
        List<InventoryItemController> tmp = new List<InventoryItemController>();
        foreach (var item in this.itemList)
        {
            var data = item.GetData();
            data.ItemNum -= makeNum;
            if (data.ItemNum <= 0) {
                tmp.Add(item);
            } else {
                item.SetData(data.DragContext);
            }
        }

        this.RunOutOfMaterials(tmp);
        this.m_CraftingRightController.MaterialCountUpdateEvent.Fire(false);
    }

    // 清理掉使用完的材料.
    private void RunOutOfMaterials(List<InventoryItemController> goList)
    {
        foreach (var item in goList)
        {
            this.itemList.Remove(item);
            GameObject.Destroy(item.gameObject);
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

    public void UIPanelShow()
    {
        base.OnShow();
    }

    public void UIPanelHide()
    {
        base.OnHide();
    }
}
