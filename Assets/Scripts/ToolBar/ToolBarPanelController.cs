using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanelController : BaseController<ToolBarPanelController>
{
    private ToolBarPanelView m_ToolBarPanelView;

    private const int SLOT_NUM = 8;

    private ToolBarSlotController current = null; // 当前选择的slot
    List<ToolBarSlotController> slotList = new List<ToolBarSlotController>();

    public override void Start()
    {
        base.Start();
        this.CreateAllSlots();
    }

    protected override void OnInit()
    {
        m_BaseView = this.GetComponent<BaseView>();
        this.m_ToolBarPanelView = (ToolBarPanelView)m_BaseView;
        this.InitEventListener();
    }

    // 生成所有的工具槽.
    private void CreateAllSlots()
    {
        for (int i = 0; i < SLOT_NUM; i++)
        {
            var prefab = this.m_ToolBarPanelView.GetPrefabByDict(GUIName.ToolBarSlot);
            var parent = this.m_ToolBarPanelView.GridTransform;
            var item = GameObject.Instantiate<GameObject>(prefab, parent).GetComponent<ToolBarSlotController>();
            item.SetData(i);
            slotList.Add(item);
        }
    }

    // 选择slot
    private void OnItemClick(ToolBarSlotController item)
    {
        if (current != null) current.SetSelect(false);

        bool is_active = current == null || current != item;
        if (item.gameObject.activeSelf != !is_active)
        {
            item.SetSelect(is_active);
        }
        current = current != item ? item : null;
    }

    // 快捷键使用
    public void Click(int index)
    {
        this.OnItemClick(this.slotList[index]);
    }
}
