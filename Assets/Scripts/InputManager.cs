using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    void Update ()
    {
        if (Input.GetKeyDown((KeyCode)ShortCutEnum.InventoryPanelKey))
        {
            UtilsUI.SetUIActive(InventoryPanelController.Instance, !InventoryPanelController.Instance.gameObject.activeSelf);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_1))
        {
            ToolBarPanelController.Instance.Click(0);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_2))
        {
            ToolBarPanelController.Instance.Click(1);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_3))
        {
            ToolBarPanelController.Instance.Click(2);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_4))
        {
            ToolBarPanelController.Instance.Click(3);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_5))
        {
            ToolBarPanelController.Instance.Click(4);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_6))
        {
            ToolBarPanelController.Instance.Click(5);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_7))
        {
            ToolBarPanelController.Instance.Click(6);
        }
        else if (Input.GetKeyDown((KeyCode)ShortCutEnum.ToolBarPanelKey_8))
        {
            ToolBarPanelController.Instance.Click(7);
        }
    }
}
