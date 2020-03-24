using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPanelView : BaseView {

	private Transform tabs_Transform;
	private Transform contents_Transform;
	private Transform center_Transform;

	public Transform Tabs_Transform { get { return tabs_Transform; } }
	public Transform Contents_Transform { get { return contents_Transform; } }
	public Transform Center_Transform { get { return center_Transform; } }

    private Dictionary<string, Sprite> tabIconDic = null;
	private Dictionary<string, Sprite> materialIconDic = null;
    public Dictionary<string, Sprite> TabIconDic { get { return tabIconDic; } }
    public Dictionary<string, Sprite> MaterialIconDic { get { return materialIconDic; } }

    public override void Awake()
    {
        base.Awake();

        tabs_Transform = this.m_Transform.Find("Left/Tabs");
        contents_Transform = this.m_Transform.Find("Left/Contents");
        center_Transform = this.m_Transform.Find("Center");

        this.InitAssetDict(new[]
        {
            GUIName.CraftingTabsItem,
            GUIName.CraftingContent,
            GUIName.CraftingContentItem,
            GUIName.CraftingSlot,
        });

        tabIconDic = UtilsBase.LoadFolderAllSprites("TabIcon");       // 加载合成选项卡所有图片
        materialIconDic = UtilsBase.LoadFolderAllSprites("Material"); // 加载合成材料所有图片 
    }
}
