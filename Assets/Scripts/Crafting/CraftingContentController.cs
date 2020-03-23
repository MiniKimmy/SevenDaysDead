using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingContentController : MonoBehaviour {

	private Transform m_Transform;

	private int index = -1;
	private int currentIndex = -1; // 当前选择哪一个.

	private List<CraftingContentsItemController> itemList = new List<CraftingContentsItemController>();

	void Awake ()
    {
		m_Transform = this.GetComponent<Transform>();
	}
	
	public void InitItem(int index, GameObject goPrefab, List<CraftingContentsItem> goList)
    {
		this.index = index;
		this.name = "Content" + index;

		this.CreateAllItems(goList, goPrefab);
	}

    // 创建合成所有正文内容
	private void CreateAllItems(List<CraftingContentsItem> goList, GameObject goPrefab)
    {
		for (int i = 0; i < goList.Count; i ++)
        {
			var item = GameObject.Instantiate<GameObject>(goPrefab, this.m_Transform).GetComponent<CraftingContentsItemController>();
			item.InitItem(i, goList[i]);
			this.itemList.Add(item);
		}
	}

	// 点击处理(重置正文选项卡)
	private void ResetItemState(int index)
    {
		if (index == this.currentIndex) return;

		if (this.currentIndex != -1) {
			this.itemList[this.currentIndex].NormalItem();
		}

		this.itemList[index].ActiveItem();
		this.currentIndex = index;

		// 刷新合成图谱
		SendMessageUpwards("ResetAllSlotContents", this.itemList[index].GetData());
	}

}
