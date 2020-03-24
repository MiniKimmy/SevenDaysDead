using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarSlotController : MonoBehaviour {

    private Transform m_Transform;
    private Text m_Text;

    private int index = -1;

	void Awake ()
    {
        m_Transform = this.GetComponent<Transform>();
        m_Text = UtilsUI.GetComponent<Text>(m_Transform, "Text");
    }

    public void SetData(int index)
    {
        this.index = index;

        this.name = "ToolBarSlot" + index;
        this.m_Text.text = (index + 1).ToString();
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
