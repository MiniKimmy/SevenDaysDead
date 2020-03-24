/// 数据实体类(正文选项卡)
public class CraftingContentsItem {


	private int    itemId;      // 物品id
	private string itemName;    // 物品名称

	public int     ItemID { get { return itemId; } set { itemId = value; } }
	public string  ItemName { get { return itemName; } set { itemName = value; } }

	public override string ToString()
    {
		return string.Format("id:{0}, name:{1}", this.itemId, this.itemName);
	}
}
