using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BulletMark : MonoBehaviour {

    private Texture2D m_MarkTexture;                // 弹痕贴图
    private Texture2D backup_MainTexture_1;         // 主贴图备份1(用于显示)
    private Texture2D backup_MainTexture_2;         // 主贴图备份2(用于重置)
    private Queue<Vector2> queue = new Queue<Vector2>();

    [SerializeField] BulletMarkEnum @enum;          // Inspector里选择贴图类型.
    private GameObject effect;                      // 特效

    // 简单工厂 - Inspector设置类型
    private Texture2D GetMarkTexture()
    {
        var root = "Weapon/BulletMarks/";
        switch (@enum)
        {
            case BulletMarkEnum.Wood:
                this.effect = Resources.Load<GameObject>(GAssetName.BulletMarkEffect_Wood);
                return Resources.Load<Texture2D>(root + "Bullet Decal_Wood");
            case BulletMarkEnum.Stone:
                this.effect = Resources.Load<GameObject>(GAssetName.BulletMarkEffect_Stone);
                return Resources.Load<Texture2D>(root + "Bullet Decal_Stone");
            case BulletMarkEnum.Metal:
                this.effect = Resources.Load<GameObject>(GAssetName.BulletMarkEffect_Metal);
                return Resources.Load<Texture2D>(root + "Bullet Decal_Metal");
            case BulletMarkEnum.Flesh:
                this.effect = Resources.Load<GameObject>(GAssetName.BulletMarkEffect_Flesh);
                return Resources.Load<Texture2D>(root + "Bullet Decal_Flesh");
            default:
#if UNITY_EDITOR
                UtilsBase.ddd("当前obj的Inspector面板未设置好[BulletMarkEnum]");
#endif
                break;
        }

        return null;
    }

    void Start ()
    {
        m_MarkTexture = this.GetMarkTexture();
        var mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
        backup_MainTexture_1 = GameObject.Instantiate<Texture2D>(mainTexture);
        backup_MainTexture_2 = GameObject.Instantiate<Texture2D>(mainTexture);
        this.GetComponent<MeshRenderer>().material.mainTexture = backup_MainTexture_1;
    }
	
    // 弹痕融合。ps:碰撞体必须使用Mesh Collider
    public void CreateMark(RaycastHit hit)
    {
        Vector2 uv = hit.textureCoord; // 获取碰撞到的主贴图uv坐标(该点也就是弹痕贴图融合后的中心点）
        queue.Enqueue(uv);

        // 遍历弹痕贴图的uv坐标
        for (int x = 0; x < m_MarkTexture.width; x ++) {
            for (int y = 0; y < m_MarkTexture.height; y ++) {
                /// 计算弹痕贴图当前(x,y)融合到主贴图后的坐标(a,b)
                int a = (int)(uv.x * backup_MainTexture_1.width - m_MarkTexture.width / 2 + x);   // uv.x * 主贴图.width - 弹痕贴图.width/2 + x
                int b = (int)(uv.y * backup_MainTexture_1.height - m_MarkTexture.height / 2 + y); // uv.y * 主贴图.height - 弹痕贴图.height/2 + y 

                Color mark_color = m_MarkTexture.GetPixel(x, y);

                if (mark_color.a >= 0.2) // 当前uv的点透明度太高不融合
                    backup_MainTexture_1.SetPixel(a, b, mark_color);
            }
        }
        backup_MainTexture_1.Apply(); // 保存.
        UtilsBase.PlayEffect(this.effect, hit.point, Quaternion.LookRotation(hit.normal));
   
        this.Invoke("ResetMark", 5f);
    }

    // 清除1张弹痕贴图
    private void ResetMark()
    {
        if (queue.Count > 0)
        {
            var uv = queue.Dequeue();
            for (int x = 0; x < m_MarkTexture.width; x ++)
            {
                for (int y = 0; y < m_MarkTexture.height; y ++)
                {
                    int a = (int)(uv.x * backup_MainTexture_1.width - m_MarkTexture.width / 2 + x);
                    int b = (int)(uv.y * backup_MainTexture_1.height - m_MarkTexture.height / 2 + y);
                    backup_MainTexture_1.SetPixel(a, b, backup_MainTexture_2.GetPixel(a, b));
                }
            }
            backup_MainTexture_1.Apply();
        }
    }

    // 退出游戏时, 恢复所有弹痕.
    private void OnDestroy()
    {
        StopAllCoroutines();
        while(queue.Count > 0) this.ResetMark();
    }
}
