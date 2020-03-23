using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTabsItemController : MonoBehaviour {
	
	private Transform m_Transform;
	private Transform m_ButtonBG;
	private Image m_Image;

	private int index = -1;

	void Awake () {
		this.m_Transform = this.GetComponent<Transform>();
		this.m_ButtonBG = this.m_Transform.Find("Center_BG");
		this.m_Image = this.m_Transform.Find("Icon").GetComponent<Image>();
		
		this.m_Transform.GetComponent<Button>().onClick.AddListener(this.ButtonOnClick);
	}

	public void InitItem(int index, Sprite goSprite){
		this.index = index;
		this.name = "Tab" + index;
		this.m_Image.sprite = goSprite;
	}

	public void NormalTab()
	{
		UtilsUI.SetScaleActive(m_ButtonBG, true);
	}

	public void ActiveTab()
	{
		UtilsUI.SetScaleActive(m_ButtonBG, false);
	}
	
	public void ButtonOnClick()
	{
		SendMessageUpwards("ResetTabsAndContents", this.index);
	}
}
