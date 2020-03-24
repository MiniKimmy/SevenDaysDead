using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private InventoryItem vo = null;

	private RectTransform m_Transform;
    private CanvasGroup m_CanvasGroup;
	private Text m_Text;
	private Image m_Image;
    private RectTransform m_ImageTrans;

    private Transform last_Parent;  // 上一个parent
    private int index = -1;         // 处于slot的下标
    private bool isDraging = false; // 是否在拖拽中


    void Awake ()
    {
		m_Transform = this.GetComponent<RectTransform>();
        m_CanvasGroup = this.GetComponent<CanvasGroup>();
        m_ImageTrans = UtilsUI.GetComponent<RectTransform>(m_Transform, "Icon");
        m_Image = UtilsUI.GetComponent<Image>(m_ImageTrans);
        m_Text = UtilsUI.GetComponent<Text>(m_ImageTrans, "Num");
	}

    void Update()
    {
        if (isDraging && Input.GetMouseButtonDown(1)) // 右键
        {
            this.BreakMaterials();
        }
    }

    // 克隆
    public static InventoryItemController Clone(GameObject go, Transform parent, InventoryItem data, int index)
    {
        var res = UtilsBase.Clone<InventoryItemController>(go, parent);
        res.InitData(index, data);
        res.SetData(data.DragContext);
        res.ResetSelf();
        return res;
    }

    // 第一次初始化
    public void InitData(int index, InventoryItem data)
    {
        this.vo = data;
        this.SetIndex(index);
    }

    // 设置index下标
    public void SetIndex(int index)
    {
        this.index = index;
        this.name = "Item" + index;
    }

    // 获取index下标
    public int GetIndex()
    {
        return this.index;
    }

    public void SetData(ItemDragContextEnum goDragCtx = ItemDragContextEnum.None)
    {
        this.vo.DragContext = goDragCtx;

        int num = this.vo.ItemNum;
        this.m_Text.text = num > 1 ? num.ToString() : "";

        // 第一次加载
        if (index == -1)
        {
            string name = this.vo.ItemName;
            string spriteName = "Item/" + name;
            this.m_Image.sprite = Resources.Load<Sprite>(spriteName); // 物品Item比较多, 使用冷加载
        }

        if (goDragCtx == ItemDragContextEnum.Crafting)
            UtilsUI.SetWidthAndHeight(this.m_ImageTrans, 70, 62);
        else if (goDragCtx == ItemDragContextEnum.Inventory || goDragCtx == ItemDragContextEnum.ToolBar)
            UtilsUI.SetWidthAndHeight(this.m_ImageTrans, 85, 85);
        else if (goDragCtx == ItemDragContextEnum.Crafted)
            UtilsUI.SetWidthAndHeight(this.m_ImageTrans, 110, 110);

    }

    // 设置parent
    public void SetSlotParent(Transform parent)
    {
        this.m_Transform.SetParent(parent);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("开始拖拽");
        this.last_Parent = this.m_Transform.parent;
        this.isDraging = true;

        SendMessageUpwards("OnDragItemOutSlot", this);

        this.SetSlotParent(CameraManager.Instance.CanvasTran);
        this.m_CanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("拖拽中");
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_Transform, eventData.position, eventData.enterEventCamera, out pos);
        this.m_Transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("拖拽结束");
        GameObject other = eventData.pointerEnter;

        // 非UI位置
        if (other == null)
        {
            this.SetSlotParent(this.last_Parent);
        }
        
        // 拖拽到背包空Slot位置
        else if (other.tag == "InventorySlot")
        {
            this.SetSlotParent(other.transform);
        }

        // 拖拽到工具栏空Slot位置
        else if (other.tag == "ToolBarSlot")
        {
            this.SetSlotParent(other.transform);
        }

        // 拖拽到背包Item位置
        else if (other.tag == "InventoryItem")
        {
            var otherItem = other.GetComponent<InventoryItemController>();

            // 同一个材料 + 不在合并模块 就可以合并
            if (otherItem.vo.ItemId == this.vo.ItemId && otherItem.vo.DragContext != ItemDragContextEnum.Crafting)
            {
                this.MergeMaterials(otherItem);
                return;
            }

            // 不允许交换位置的情况
            else if (!this.vo.CheckSwap(otherItem.vo))
            {
                this.SetSlotParent(this.last_Parent);
                this.ResetSelf();
            }

            // 不同种类物品交换位置
            else
            {
                this.SetSlotParent(other.transform.parent);
                otherItem.SetSlotParent(this.last_Parent);
                otherItem.ResetSelf();
                otherItem.SendMessageUpwards("OnDragItemIntoSlot", otherItem);
            }
        }

        // 拖拽到合成空Slot位置 && 合成空Slot可以接收材料
        else if (other.tag == "CraftingSlot" && other.GetComponent<CraftingSlotController>().CheckCanReceive(this.vo.ItemId))
        {
            this.SetSlotParent(other.transform);
        }

        // 其余情况
        else
        {
            this.SetSlotParent(this.last_Parent);
        }

        // 最后重置为默认状态, 松开鼠标最后肯定是在slot中
        this.ResetSelf();
        SendMessageUpwards("OnDragItemIntoSlot", this);
    }

    // 拆分合成材料.
    private void BreakMaterials()
    {
        var data = this.vo;
        if (!data.CheckBreak()) return;

        EventManager.Instance.Fire(EventName.BreakMaterials, this, data.ItemNum / 2);
    }

    // 合并同类合成材料.
    private void MergeMaterials(InventoryItemController other)
    {
        var data = this.vo;
        other.vo.ItemNum = data.ItemNum + other.vo.ItemNum;
        other.index = Mathf.Max(this.index, other.index);
        other.SetData(other.vo.DragContext);
        GameObject.Destroy(this.gameObject);
    }

    // 重置item
    public void ResetSelf()
    {
        this.m_Transform.localScale = Vector3.one;
        this.m_Transform.localPosition = Vector3.zero;
        this.m_CanvasGroup.blocksRaycasts = true;
        this.isDraging = false;
    }

    public InventoryItem GetData()
    {
        return this.vo;
    }

    public override string ToString()
    {
        return this.vo.ToString() + " index =" + this.index;
    }
}
