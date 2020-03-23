// 数据实体类(Item)
public class InventoryItem
{
    private int itemId;             // 物品id
    public int ItemId
    {
        get { return itemId; }
        set { itemId = value; }
    }

    private string itemName;        // 物品名称
	public string ItemName
	{
		get { return itemName; }
		set { itemName = value; }
	}

	private int itemNum;
	public int ItemNum             // 物品数量
	{
		get { return itemNum; }
		set { itemNum = value; }
	}
	
    private ItemDragContextEnum dragContext = ItemDragContextEnum.None; // 拖拽上下文
    public ItemDragContextEnum DragContext
    {
        get { return dragContext; }
        set { dragContext = value; }
    }

    public InventoryItem() { }
    public InventoryItem(int itemId, string name, int num)
	{
        this.itemId = itemId;
		this.itemName = name;
		this.itemNum = num;
    }

    public override string ToString()
    {
		return string.Format("name:{0}, num:{1}, dragCtx:{2}", this.itemName, this.itemNum, this.dragContext);
	}
	
}
