using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelModel : BaseModel {

	public List<InventoryItem> GetJsonList (string fileName)
    {
        return UtilsBase.GetJsonList<InventoryItem>(fileName);
	}
}
