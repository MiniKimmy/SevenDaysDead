using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingContentsItemController : MonoBehaviour {

	private CraftingContentsItem vo = null;

	private Transform m_Transform;
	private Transform m_BG;
	private Text m_Text;
	private int index = -1;

	void Awake ()
    {
		this.m_Transform = this.GetComponent<Transform>();
        m_Text = UtilsUI.GetComponent<Text>(this.m_Transform, "Text");
        m_BG = this.m_Transform.Find("BG");
        UtilsUI.AddButtonListener(this.m_Transform, "", this.OnButtonClick);
        UtilsUI.SetScaleActive(m_BG, false);
	}

	public void InitItem(int index, CraftingContentsItem item)
    {
		this.index = index;
		this.vo = item;
		this.m_Text.text = item.ItemName;
	}

	// 默认状态
	public void NormalItem()
    {
		UtilsUI.SetScaleActive(m_BG, false);
	}

	// 激活状态
	public void ActiveItem()
    {
		UtilsUI.SetScaleActive(m_BG, true);
	}

	private void OnButtonClick()
    {
		SendMessageUpwards("ResetItemState", this.index);
	}

	public CraftingContentsItem GetData()
    {
		return this.vo;
	}

	public override string ToString()
    {
		return this.vo.ToString();
	}
}
