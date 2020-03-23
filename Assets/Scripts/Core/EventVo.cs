using UnityEngine;
using System;

public class EventVo {

    public event MAction eventList1 = null;
    public event Action eventList2 = null;

    private readonly string name = "<UnName Event>";
    public string Name { get { return name; } }

    public EventVo(string name = null)
    {
        if (name != null)
        {
            this.name = name;
        }

    }

    public void Fire(params object[] args)
    {
        if (this.eventList1 == null)
        {
            UtilsBase.ddd("event fire err, eventName = ", this.name, "type : MAction");
            return;
        }

        this.eventList1.Invoke(args);
    }

    public void Fire()
    {
        if (this.eventList2 == null)
        {
            UtilsBase.ddd("event fire err, eventName = ", this.name, "type : Action");
            return;
        }

        this.eventList2.Invoke();
    }

    // myAction
    public void Add(MAction func)
    {
        this.eventList1 += func;
    }

    // myAction
    public void Remove(MAction func)
    {
        if (eventList1 == null)
        {
            UtilsBase.ddd("mul delete event", this.name, "type : MAction");
            return;
        }

        this.eventList1 -= func;
    }

    // action
    public void Add(Action func)
    {
        this.eventList2 += func;
    }

    // action
    public void Remove(Action func)
    {
        if (eventList2 == null)
        {
            UtilsBase.ddd("mul delete event", this.name, "type : Action");
            return;
        }

        eventList2 -= func;
    }

    // 事件列表是否为空
    public bool IsEmpty()
    {
        return this.eventList1 == null && this.eventList2 == null;
    }

    // 事件监听总数量
    public int EventCount()
    {
        var len1 = this.eventList1 == null ? 0 : this.eventList1.GetInvocationList().Length;
        var len2 = this.eventList2 == null ? 0 : this.eventList2.GetInvocationList().Length;
        UtilsBase.ddd("event1.Length", len1, "event2.Length", len2);

        return len1 + len2;
    }

    // 清空所有事件
    public void RemoveAll()
    {
        // MAtion
        if (this.eventList1 != null)
        {
            var list = this.eventList1.GetInvocationList();
            var info = typeof(EventVo).GetEvent("eventList1");
            foreach (Delegate del in list)
            {
                info.RemoveEventHandler(this, del);
            }
        }

        // Action
        if (this.eventList2 != null)
        {
            var list = this.eventList2.GetInvocationList();
            var info = typeof(EventVo).GetEvent("eventList2");
            foreach (Delegate del in list)
            {
                info.RemoveEventHandler(this, del);
            }
        }

    }
}
