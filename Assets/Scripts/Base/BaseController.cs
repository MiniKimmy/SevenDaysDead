using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoSingletonAttribute : Attribute
{
    public bool autoCreate; //是否自动创建单例
    public string resPath;  //从指定的预制体路径生成单例

    // _autoCreate ：若获取不到单例则自动创建单例, true表示 是
    // _resPath    : 单例obj的资源Resources下的路径
    // 调用方式参考: 
    // [AutoSingleton(true, "Manager/MyMonoController")]
    // public class MyMonoController : BaseController<MyMonoController> 
    public AutoSingletonAttribute(bool _autoCreate, string _resPath = "")
    {
        this.autoCreate = _autoCreate;
        this.resPath = _resPath;
    }
}

public class BaseController<T> : MonoBehaviour where T : MonoBehaviour
{
    protected BaseView m_BaseView;
    protected BaseModel m_BaseModel;
    public BaseView View { get { return this.m_BaseView; } }
    public BaseModel Model { get { return this.m_BaseModel; } }

    private static T _instance;
    private static object _lock = new object();
    private static bool _destroyed = false;

    public virtual void Start ()
    {
        if (_instance != null && _instance.gameObject != gameObject)
        {
            if (Application.isPlaying) {
                GameObject.Destroy(gameObject);
            } else{
                GameObject.DestroyImmediate(gameObject); // Destroy 会在下一帧才销毁
            }
        }

        else
        {
            _instance = this.GetComponent<T>();
            if (!this.transform.parent)
                DontDestroyOnLoad(gameObject);

            this.OnInit();
        }
    }

    protected virtual void OnInit()
    {
        m_BaseView = this.GetComponent<BaseView>();
        m_BaseModel = this.GetComponent<BaseModel>();

        this.InitEventListener();
    }

    // 添加事件监听
    protected virtual void InitEventListener() { }

    // 删除事件监听
    protected virtual void RemoveEventListener() { }

    public virtual void OnShow()
    {
        this.InitEventListener();
        UtilsUI.SetActive(this.gameObject, true);
    }

    public virtual void OnHide()
    {
        this.RemoveEventListener();
        UtilsUI.SetActive(this.gameObject, false);
    }

    public static T Instance{
        get
        {
            Type _type = typeof(T);
            if (_destroyed) {
                Debug.LogWarningFormat("[Singleton]【{0}】已被标记为销毁，返 Null！", _type.Name);
                return (T)((object)null);
            }

            lock (_lock)
            {
                if (null != _instance) return _instance;

                _instance = (T)FindObjectOfType(_type);
                if (FindObjectsOfType(_type).Length > 1) {
                    Debug.LogErrorFormat("[Singleton]类型【{0}】存在多个实例.", _type.Name);
                    return _instance;
                }

                if (null == _instance) {
                    object[] customAttributes = _type.GetCustomAttributes(typeof(AutoSingletonAttribute), true);
                    AutoSingletonAttribute autoAttribute = (customAttributes.Length > 0) ? (AutoSingletonAttribute)customAttributes[0] : null;
                    if (null == autoAttribute || !autoAttribute.autoCreate)
                    {
                        Debug.LogWarningFormat("[Singleton]欲访问单例【{0}】不存在且设置了非自动创建~", _type.Name);
                        return (T)((object)null);
                    }

                    // Resources加载res
                    GameObject go = null;
                    if (string.IsNullOrEmpty(autoAttribute.resPath)) 
                    {
                        go = new GameObject(_type.Name);
                        _instance = go.AddComponent<T>();
                    } else {
                        go = Resources.Load<GameObject>(autoAttribute.resPath);
                        if (null != go) {
                            go = GameObject.Instantiate(go, CameraManager.Instance.CanvasUITran);
                        } else {
                            Debug.LogErrorFormat("[Singleton]类型【{0}】ResPath设置的路径不对【{1}】", _type.Name, autoAttribute.resPath);
                            return (T)((object)null);
                        }
                        _instance = go.GetComponent<T>();
                        if (null == _instance) {
                            Debug.LogErrorFormat("[Singleton]指定预制体未挂载该脚本【{0}】，ResPath【{1}】", _type.Name, autoAttribute.resPath);
                        }
                    }
                }

                return _instance;
            }

        }
    }

    // 销毁inst
    public static void DestroyInstance()
    {
        if (_instance != null)
            GameObject.Destroy(_instance.gameObject);
 
        _destroyed = true;
        _instance = null;
    }

    // 清除lock, inst
    public static void ClearDestroy()
    {
        DestroyInstance();
        _destroyed = false;
    }

    // 结束unity时候的单例被重新调用特殊情况处理
    public void OnDestroy()
    {
        if (_instance != null && _instance.gameObject == base.gameObject)
        {
            _instance = null;
            _destroyed = true;

            this.RemoveEventListener();
        }
    }

}

