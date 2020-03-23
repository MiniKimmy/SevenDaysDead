using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager {

    public Hashtable hash = new Hashtable(); // <string, event>
    public Dictionary<string, EventVo> dict = new Dictionary<string, EventVo>();

    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (instance == null) instance = new EventManager();
            return instance;
        }
    }

    // 注册方法:EventManager.Instance.AddListener(eventName, (args) => methodName(args))
    public void AddListener(string eventName, MAction func)
    {
        if (!this.hash.ContainsKey(eventName)) {
            this.hash.Add(eventName, new EventVo(eventName));
        }

        var vo = (EventVo)this.hash[eventName];
        vo.Add(func);
    }

    // 注册方法:EventManager.Instance.AddListener(eventName, (args) => methodName(args))
    public void AddListener(string eventName, Action func)
    {
        if (!this.hash.ContainsKey(eventName))
        {
            this.hash.Add(eventName, new EventVo(eventName));
        }

        var vo = (EventVo)this.hash[eventName];
        vo.Add(func);
    }

    public void RemoveListener(string eventName, MAction func)
    {
        var vo = (EventVo)this.hash[eventName];
        vo.Remove(func);

        if (vo.IsEmpty())
            this.hash.Remove(eventName);
    }

    // tmp
    //public void RemoveListener(string eventName, Action func)
    //{
    //    var vo = (EventVo)this.hash[eventName];
    //    vo.Remove(func);

    //    if (vo.IsEmpty())
    //        this.hash.Remove(eventName);
    //}

    public void Fire(string eventName)
    {
        if (!this.hash.ContainsKey(eventName))
        {
            UtilsBase.ddd(eventName, "is not exist!");
            return;
        }

        var vo = (EventVo)this.hash[eventName];
        vo.Fire();
    }

    public void Fire(string eventName, params object[] args)
    {
        if (!this.hash.ContainsKey(eventName))
        {
            UtilsBase.ddd(eventName, "is not exist!");
            return;
        }

        var vo = (EventVo)this.hash[eventName];
        vo.Fire(args);
    }
    
}