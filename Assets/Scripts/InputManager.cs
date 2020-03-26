using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown((KeyCode)ShortCutEnum.InventoryPanelKey))
        {
            UtilsUI.SetUIActive(InventoryPanelController.Instance, !InventoryPanelController.Instance.gameObject.activeSelf);
        }
		
	}
}
