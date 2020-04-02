/// 数据实体类(枪械)
public class WeaponItem {

    private int id; // 道具id
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    private int life;
    public int Life // 耐久值
    {
        get { return life; }
        set { life = value; }
    }

    private WeaponEnum weaponType = WeaponEnum.None;
    public WeaponEnum WeaponType
    {
        get { return weaponType; }
    }

    private int damage; // 伤害值
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public override string ToString()
    {
        return string.Format("id:{0}, Life:{1}, Dam:{2}, type:{3}", this.id, this.life, this.damage, this.weaponType);
    }

}
