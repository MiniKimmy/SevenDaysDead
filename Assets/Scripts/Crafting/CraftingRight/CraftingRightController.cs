﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRightController : MonoBehaviour {

    private CraftingMapItem vo = null;

    private Transform m_Transform;
    private Image m_Image;
    private Transform m_ImageTrans;
    private Text m_CraftBtnText;
    private Text m_CraftAllBtnText;
    private Button m_CraftAllBtn;
    private Button m_CraftBtn;

    private InventoryItemController product = null; // 生产产品

    private EventVo materialCountUpdateEvent = null;
    public EventVo MaterialCountUpdateEvent { get { return materialCountUpdateEvent; } }

    void Start ()
    {
        m_Transform = this.GetComponent<Transform>();
        m_ImageTrans = UtilsUI.GetComponent<Transform>(m_Transform, "GoodItem/ItemImage");
        m_Image = UtilsUI.GetComponent<Image>(m_Transform, "GoodItem/ItemImage");
        m_CraftBtnText = UtilsUI.GetComponent<Text>(m_Transform, "Craft/Text");
        m_CraftAllBtnText = UtilsUI.GetComponent<Text>(m_Transform, "CraftAll/Text");
        m_CraftBtn = UtilsUI.GetComponent<Button>(m_Transform, "Craft");
        m_CraftAllBtn = UtilsUI.GetComponent<Button>(m_Transform, "CraftAll");

        UtilsUI.AddButtonListener(m_Transform, "Craft", this.OnCraftClick);
        UtilsUI.AddButtonListener(m_Transform, "CraftAll", this.OnCraftAllClick);
        UtilsBase.SetEvent(ref materialCountUpdateEvent, (args) => this.OnMaterialCountUpdateEvent(args));

        this.ResetData();
    }

    // 重置初始化数据
    public void ResetData()
    {
        this.vo = null;
        this.product = null;
        UtilsUI.SetScaleActive(m_ImageTrans, false);
        this.SetIsGrey(true);
    }

    public void SetData(CraftingMapItem data)
    {
        if (data == null)
        {
            this.ResetData();
            return;
        }

        this.vo = data;
        string spriteName = data.MapName;
        UtilsUI.SetScaleActive(m_ImageTrans, true);
        m_Image.sprite = Resources.Load<Sprite>("Item/" + spriteName);
    }

    // 监听合成图谱内材料数量变化
    private void OnMaterialCountUpdateEvent(params object[] args)
    {
        bool isEnough = (bool)args[0];
        if (isEnough != this.m_CraftBtn.interactable)
        {
            Debug.Log(isEnough);
            this.SetIsGrey(!isEnough);
        }
    }

    // 设置是否置灰
    private void SetIsGrey(bool isGrey)
    {
        if (isGrey)
        {
            m_CraftBtn.interactable = false;
            m_CraftAllBtn.interactable = false;
            m_CraftBtnText.text = UtilsBase.Color("808080", "合 成");
            m_CraftAllBtnText.text = UtilsBase.Color("808080", "合 成 所 有");
        }
        else
        {
            m_CraftBtn.interactable = true;
            m_CraftAllBtn.interactable = true;
            m_CraftBtnText.text = UtilsBase.Color("FFFFFFFF", "合 成");
            m_CraftAllBtnText.text = UtilsBase.Color("FFFFFFFF", "合 成 所 有");
        }
    }

    // 生产产品
    public void MakeGoodItem(int makeNum = 1)
    {
        var data = this.vo;

        var prefab = InventoryPanelController.Instance.View.GetPrefabByDict(GUIName.InventoryItem);
        var item = GameObject.Instantiate<GameObject>(prefab, this.m_ImageTrans).GetComponent<InventoryItemController>();
        item.InitData(-1, new InventoryItem(data.MapId, data.MapName, makeNum));
        item.SetData(ItemDragContextEnum.Crafted);
        this.product = item;
    }

    // 单个合成
    private void OnCraftClick()
    {
        SendMessageUpwards("Craft");
    }

    // 合成所有
    private void OnCraftAllClick()
    {
        SendMessageUpwards("CraftAll");
    }

    // 获取当前合成物品data
    public CraftingMapItem GetData()
    {
        return this.vo;
    }

    // 获取当前生产的物品item
    public InventoryItemController GetProduct()
    {
        return this.product;
    }

}
