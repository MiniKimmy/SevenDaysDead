using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class CraftingPanelModel : BaseModel {

	// 图谱dic
	Dictionary<int, CraftingMapItem> mapItemDic = new Dictionary<int, CraftingMapItem>();

	public override void Awake ()
    {
		mapItemDic = this.LoadMapJsonData("CraftingMapJsonData");
	}

	// 获取合成选项卡
	public List<List<CraftingContentsItem>> ByNameGetJsonData(string fileName){
		List<List<CraftingContentsItem>> res = new List<List<CraftingContentsItem>>();
		string jsonStr = Resources.Load<TextAsset>("JsonData/" + fileName).text;
		JsonData jsonData = JsonMapper.ToObject(jsonStr);

		for (int i = 0; i < jsonData.Count; i ++)
		{
			var typeList = jsonData[i]["Type"];
			List<CraftingContentsItem> item = new List<CraftingContentsItem>();
			for (int j = 0; j < typeList.Count; j ++)
				item.Add(JsonMapper.ToObject<CraftingContentsItem>(typeList[j].ToJson()));
			
			res.Add(item);
		}

		return res;
	}

	// 合成图谱
	public Dictionary<int, CraftingMapItem> LoadMapJsonData(string fileName){
		Dictionary<int, CraftingMapItem> res = new Dictionary<int, CraftingMapItem>();
		string jsonStr = Resources.Load<TextAsset>("JsonData/" + fileName).text;
		JsonData jsonData = JsonMapper.ToObject(jsonStr);

		for (int i = 0; i < jsonData.Count; i ++)
		{
			var item = JsonMapper.ToObject<CraftingMapItem>(jsonData[i].ToJson());
			res.Add(item.MapId, item);
		}

		return res;
	}

	// id -》 合成图谱vo
	public CraftingMapItem GetItemById(int id)
	{
		CraftingMapItem res = null;
		this.mapItemDic.TryGetValue(id, out res);
		return res;
	}

	// 获取所有的选项卡图标名字
	public string[] GetTabsIconName(){
		return new string[] { "Icon_House", "Icon_Weapon" };
	}

}
