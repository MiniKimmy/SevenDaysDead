using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 所有资源加载都在View, 子物体不涉及加载.
public abstract class BaseView : MonoBehaviour {

    protected Transform m_Transform;
    public Transform Transform { get { return this.m_Transform; } }

    protected Dictionary<string, GameObject> prefabs_Obj = null; // 预制体, effect

    protected virtual void Awake()
    {
        m_Transform = this.GetComponent<Transform>();
    }

    protected void InitAssetDict(string[] assetNames)
    {
        if (this.prefabs_Obj == null)
        {
            this.prefabs_Obj = new Dictionary<string, GameObject>();
        }

        for (int i = 0; i < assetNames.Length; i ++)
        {
            string name = assetNames[i];
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogErrorFormat("【InitAssetDict】传入name是空字符串!!");
                return;
            }

            if (this.prefabs_Obj.ContainsKey(name))
            {
                UtilsBase.ddd("【InitAssetDict】重复添加资源", name, "检查是否使用重复的GAssetName");
                continue;
            }
#endif
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
