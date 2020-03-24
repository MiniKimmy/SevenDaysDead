using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanelController : BaseController<ToolBarPanelController>
{
    private ToolBarPanelView m_ToolBarPanelView;
    private ToolBarPanelModel m_ToolBarPanelModel;

    private const int SLOT_NUM = 8;

    public override void Start()
    {
        base.Start();

        this.m_ToolBarPanelView = (ToolBarPanelView)this.View;
        this.m_ToolBarPanelModel = (ToolBarPanelModel)this.Model;

        this.CreateAllSlots();
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
        }

    }
}
