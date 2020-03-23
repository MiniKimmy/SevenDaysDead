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

    private int index = -1;
    private bool isDraging = false;

    private Transform last_Parent; // 上一个parent

    void Awake ()
    {
		m_Transform = this.GetComponent<RectTransform>();
        m_CanvasGroup = this.GetComponent<CanvasGroup>();
        m_ImageTrans = UtilsUI.GetComponent<RectTransform>(this.m_Transform, "Icon");
        m_Image = UtilsUI.GetComponent<Image>(this.m_ImageTrans);
        m_Text = UtilsUI.GetComponent<Text>(this.m_ImageTrans, "Num");
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
        string name = this.vo.ItemName;
        int num = this.vo.ItemNum;

        this.m_Text.text = num > 1 ? num.ToString() : "";

        string spriteName = "Item/" + name;
        this.m_Image.sprite = Resources.Load<Sprite>(spriteName); // 物品Item比较多, 使用冷加载

        if (goDragCtx == ItemDragContextEnum.Crafting)
            UtilsUI.SetWidthAndHeight(this.m_ImageTrans, 70f, 62f);
        else
            UtilsUI.SetWidthAndHeight(this.m_ImageTrans, 85f, 85f);
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
            this.SetData(ItemDragContextEnum.Inventory);
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

            // [特判]若不在背包内不允许交换位置
            else if (this.vo.DragContext != ItemDragContextEnum.Inventory || otherItem.vo.DragContext != ItemDragContextEnum.Inventory)
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
            this.SetData(ItemDragContextEnum.Crafting);
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
        int newNum = data.ItemNum / 2;

        if (newNum < 1)
        {
            UtilsBase.ddd("not break any more. curNum =" + data.ItemNum);
            return; // 如果不能继续拆
        }
        EventManager.Instance.Fire(EventName.BreakMaterials, this, newNum);
    }

    // 合并同类合成材料.
    private void MergeMaterials(InventoryItemController other)
    {
        var data = this.vo;
        int newNum = data.ItemNum + other.vo.ItemNum;
        other.vo.ItemNum = newNum;
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
