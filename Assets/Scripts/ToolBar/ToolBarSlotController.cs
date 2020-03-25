using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarSlotController : MonoBehaviour {

    private Transform m_Transform;
    private Text m_Text;
    private Image m_Image;

    private int index = -1;
    private bool is_selected = false;

    void Awake ()
    {
        m_Transform = this.GetComponent<Transform>();
        m_Text = UtilsUI.GetComponent<Text>(m_Transform, "Text");
        m_Image = UtilsUI.GetComponent<Image>(m_Transform, "");
        UtilsUI.AddButtonListener(m_Transform, "", OnBtnClick);
        this.SetSelect(false);
    }

    public void SetData(int index)
    {
        this.index = index;
        this.name = "ToolBarSlot" + index;
        this.m_Text.text = (index + 1).ToString();
    }

    private void OnBtnClick()
    {
        SendMessageUpwards("OnItemClick", this);
    }

    public void SetSelect(bool is_selected)
    {
        m_Image.color = UtilsBase.Hex2Color(is_selected ? "E72C2CFF" : "FFFFFFFF");
    }

    // 当有物体拖拽进来.
    private void OnDragItemIntoSlot(InventoryItemController item)
    {
        item.SetIndex(this.index);
        item.SetData(ItemDragContextEnum.ToolBar);
    }

    // 当有物体拖拽离开.
    private void OnDragItemOutSlot(InventoryItemController item) {
        // 不作处理.
    }
}
