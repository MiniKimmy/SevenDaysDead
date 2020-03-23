/// 数据实体类(正文选项卡)
public class CraftingContentsItem {

	private int itemId;
	public int ItemID
	{
		get { return itemId; }
		set { itemId = value; }
	}

	private string itemName;
	public string  ItemName
	{
		get { return itemName; }
		set { itemName = value; }
	}

	// Litjson可以不写构造函数, 默认不写构造函数就是空参构造函数.

	public override string ToString(){
		return string.Format("id:{0}, name:{1}", this.itemId, this.itemName);
	}
}
