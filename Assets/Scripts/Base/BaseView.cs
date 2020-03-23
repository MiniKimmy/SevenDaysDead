using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseView : MonoBehaviour {

    protected Transform m_Transform;
    public Transform Transform { get { return this.m_Transform; } }

    protected Dictionary<string, GameObject> prefabs_Obj = null;

    public virtual void Awake()
    {
        m_Transform = this.GetComponent<Transform>();
    }

    public virtual void InitAssetDict(string[] guiName)
    {
        this.prefabs_Obj = new Dictionary<string, GameObject>();
        for (int i = 0; i < guiName.Length; i ++)
        {
            string name = guiName[i];
            this.prefabs_Obj.Add(name, Resources.Load<GameObject>(name));
        }
    }

    public virtual GameObject GetPrefabByDict(string guiName)
    {
        GameObject res = null;
        this.prefabs_Obj.TryGetValue(guiName, out res);
        return res;
    }
}
