using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotController : MonoBehaviour {

    private CraftingMapItem vo = null;

    private Transform m_Transform;
	private Image m_Image;
	private Transform item_Transform;

    private int index = -1;

    void Awake () {
		m_Transform = this.GetComponent<Transform>();
		item_Transform = this.m_Transform.Find("Item");
		m_Image = item_Transform.GetComponent<Image>();
		UtilsUI.SetScaleActive(item_Transform, false);
	}

    public void InitData(int index, CraftingMapItem data = null)
    {
        this.vo = data;
        this.index = index;
        this.name = "CraftingSlot" + index;
    }

    // 刷新合成槽.
    public void SetData(Sprite goSprite)
    {
		UtilsUI.SetScaleActive(item_Transform, true);
		m_Image.sprite = goSprite;
	}

	// 重置合成槽
	public void ResetSelf()
    {
		UtilsUI.SetScaleActive(item_Transform, false);
	}

    // 当前位置是否可以接收材料
    public bool CheckCanReceive(int materialID)
    {
        return this.vo != null && this.vo.GetMaterialsID(this.index) == materialID;
    }

    // 有Item拖拽进来slot
    private void OnDragItemIntoSlot(InventoryItemController item)
    {
        item.SetIndex(this.index);
        item.SetData(ItemDragContextEnum.Crafting);
    }

    public CraftingMapItem GetData()
    {
        return this.vo;
    }
}
