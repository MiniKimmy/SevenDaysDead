using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanelView : BaseView {

    private Transform grid_Transform;
    public Transform GridTransform { get { return this.grid_Transform; } }

    public override void Awake () {
        base.Awake();
        this.grid_Transform = UtilsUI.GetComponent<Transform>(this.Transform, "Grid");

        this.InitAssetDict(new[]
        {
            GUIName.ToolBarSlot,
        });
    }
	
}
