using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 所有资源加载都在View, 子物体不涉及加载.
public abstract class BaseView : MonoBehaviour {

    protected Transform m_Transform;
    public Transform Transform { get { return this.m_Transform; } }

    protected Dictionary<string, GameObject> prefabs_Obj = null; // 预制体, effect

    public virtual void Awake()
    {
        m_Transform = this.GetComponent<Transform>();
    }

    protected void InitAssetDict(string[] assetNames)
    {
        if (prefabs_Obj != null)
        {
            UtilsBase.ddd("[InitAssetDict]方法仅允许调用1次初始化");
            return;
        }

        this.prefabs_Obj = new Dictionary<string, GameObject>();
        for (int i = 0; i < assetNames.Length; i ++)
        {
            string name = assetNames[i];
            this.prefabs_Obj.Add(name, Resources.Load<GameObject>(name));
        }
    }

    // assetName:定义在 GUIName.cs / GAssetName.cs
    public GameObject GetPrefabByDict(string assetName)
    {
        GameObject res = UtilsBase.ByNameGetAsset<GameObject>(assetName, prefabs_Obj);
        if (res == null) UtilsBase.ddd("请检查是否添加过该资源到【InitAssetDict】方法中", assetName);
        return res;
    }
}
