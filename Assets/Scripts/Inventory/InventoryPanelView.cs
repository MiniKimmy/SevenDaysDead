using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelView : BaseView {

	private Transform grid_Transform;
	public Transform GridTransform { get { return this.grid_Transform; } }

    public override void Awake ()
    {
        base.Awake();
		grid_Transform = m_Transform.Find("Background/Grid");

        this.InitAssetDict(new []
        {
            GUIName.InventorySlot,
            GUIName.InventoryItem
        });
	}


}
