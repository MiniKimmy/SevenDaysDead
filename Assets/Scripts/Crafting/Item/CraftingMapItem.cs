using System.Collections.Generic;

/// 数据实体类(合成图谱)
public class CraftingMapItem
{
	private int    mapId;           // 图谱id
	private string mapName;         // 图谱名称
	private string mapContents;     // 图谱图案
    private int    materialsCount;  // 图谱材料数量

	public int     MapId { get { return mapId; } set { mapId = value; } }
	public string  MapName { get { return mapName; } set { mapName = value; } }
	public string  MapContents { get { return mapContents; } set { mapContents = value; } }
    public int     MaterialsCount { get { return materialsCount; } set { materialsCount = value; } }

    private Dictionary<int, int> materialCount = null;  // <图谱材料id, 所需合成数量>

    public CraftingMapItem() {}

    // 格式:"0,1,0,2,0,1,0,1,3"
    public int GetMaterialsID(int index)
	{
		return (int)this.mapContents[index * 2] - 48;
	}

	// 图谱string 转 char[]
	public string[] MapContentsToChar()
	{
		return this.mapContents.Split(',');
	}

    // 材料id -》所需数量
    public int GetNeedCount(int materialID)
    {
        // 第一次初始化
        if (this.materialCount == null)
        {
            this.materialCount = new Dictionary<int, int>();
            int n = this.mapContents.Length;
            for (int i = 0; i < n; i += 2)
            {
                int matID = (int)this.mapContents[i] - 48;
                if (matID != 0)
                {
                    if (!this.materialCount.ContainsKey(matID))
                        this.materialCount.Add(matID, 1);
                    else
                        this.materialCount[matID] ++;
                }
            }
        }

        return this.materialCount[materialID];
    }

    public override string ToString()
    {
		return string.Format("id:{0}, mapName:{1}, map:{2}, count:{3}", this.mapId, this.mapName, this.mapContents, this.materialsCount);
	}

}