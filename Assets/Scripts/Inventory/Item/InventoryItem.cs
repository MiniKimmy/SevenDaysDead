/// 数据实体类(背包Item)
public class InventoryItem {	
    private int                 itemId;                                 // 物品id
    private string              itemName;                               // 物品名称
	private int                 itemNum;                                // 物品数量
    private ItemDragContextEnum dragContext = ItemDragContextEnum.None; // 拖拽上下文

    public int                  ItemId { get { return itemId; } set { itemId = value; } }
    public string               ItemName { get { return itemName; } set { itemName = value; } }
	public int                  ItemNum { get { return itemNum; } set { itemNum = value; } }
    public ItemDragContextEnum  DragContext { get { return dragContext; } set { dragContext = value; } }

    public InventoryItem() { }

    public InventoryItem(int itemId, string name, int num)
	{
        this.itemId = itemId;
		this.itemName = name;
		this.itemNum = num;
    }

    // 是否允许交换位置, true表示允许
    public bool CheckSwap(InventoryItem other)
    {
        bool self_ok = this.dragContext == ItemDragContextEnum.Inventory || this.dragContext == ItemDragContextEnum.ToolBar;
        bool other_ok = other.dragContext == ItemDragContextEnum.Inventory || other.dragContext == ItemDragContextEnum.ToolBar;

        return self_ok && other_ok;
    }

    // 是否允许拆分, true表示允许
    public bool CheckBreak()
    {
        bool check_ctx = this.dragContext == ItemDragContextEnum.Inventory || this.dragContext == ItemDragContextEnum.Crafting;
        bool check_num = this.itemNum > 1;

        if (!check_ctx) UtilsBase.ddd("current dragCtx is not allow. dragCtx =" + this.dragContext);
        if (!check_num) UtilsBase.ddd("not break any more. curNum =" + this.itemNum);

        return check_num && check_ctx;
    }

    public override string ToString()
    {
		return string.Format("name:{0}, num:{1}, dragCtx:{2}", this.itemName, this.itemNum, this.dragContext);
	}
	
}
