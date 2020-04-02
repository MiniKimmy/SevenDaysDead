using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public sealed class UtilsBase {

    #region debug相关
    public static bool IsDebug = true; // debug模式

    public static void ddd(object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null)
    {
        if (!IsDebug) return;
        ddd(new object[] { args1, args2, args3, args4, args5 });
    }

    private static void ddd(params object[] args)
    {
        string format = "";
        int cnt = 0;
        int n = args.Length;
        for (int i = 0; i < n; i ++)
        {
            if (args[i] != null) cnt ++;
            else break;
            format += "{" + i + "}, ";
        }

        if (cnt == 0) Debug.Log("");
        else if (cnt == 1) Debug.LogFormat(format, args[0]);
        else if (cnt == 2) Debug.LogFormat(format, args[0], args[1]);
        else if (cnt == 3) Debug.LogFormat(format, args[0], args[1], args[2]);
        else if (cnt == 4) Debug.LogFormat(format, args[0], args[1], args[2], args[3]);
        else if (cnt == 5) Debug.LogFormat(format, args[0], args[1], args[2], args[3], args[4]);
        else Debug.LogWarningFormat("too long ====: parrms_cnt = {0}", cnt);
    }
    #endregion

    #region resources相关
    // fileName -》加载文件夹所有的sprite
    public static Dictionary<string, Sprite> LoadFolderAllSprites(string fileName)
    {
        Dictionary<string, Sprite> res = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
        for (int i = 0; i < sprites.Length; i++)
        {
            res.Add(sprites[i].name, sprites[i]);
        }
        return res;
    }

    // name -》 字典内的资源
    public static T ByNameGetAsset<T>(string name, Dictionary<string, T> dict)
        where T : class
    {
        T res = null;
        dict.TryGetValue(name, out res);
        return res;
    }

    // 创建obj
    public static T Clone<T>(GameObject go, Transform parent = null)
    {
        return GameObject.Instantiate<GameObject>(go, parent).GetComponent<T>();
    }

    // 创建obj
    public static T Clone<T>(GameObject go, Vector3 v3, Quaternion quaternion)
    {
        return GameObject.Instantiate<GameObject>(go, v3, quaternion).GetComponent<T>(); ;
    }

    // 播放音效
    public static void PlaySound(AudioClip audio_clip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(audio_clip, pos);
    }

    // 播放特效
    public static GameObject PlayEffect(GameObject effect, Vector3 pos, Quaternion quaternion)
    {
        return GameObject.Instantiate<GameObject>(effect, pos, quaternion);
    }

    #endregion

    #region event system相关
    // 触发事件:UtilsBase.FireInnerEvent(eventVo, args)
    public static void FireInnerEvent(EventVo evt, params object[] args)
    {
        evt.Fire(args);
    }

    // 触发事件:UtilsBase.FireInnerEvent(eventVo)
    public static void FireInnerEvent(EventVo evt)
    {
        evt.Fire();
    }

    // 注册单个事件:UtilsBase.SetEvent(eventVo, () => methodName())
    public static void SetEvent(ref EventVo evt, Action func)
    {
        if (evt != null) evt.RemoveAll();
        else evt = new EventVo();
        evt.Add((Action)func);
    }

    // 注册单个事件:UtilsBase.SetEvent(eventVo, (args) => methodName(args))
    public static void SetEvent(ref EventVo evt, MAction func)
    {
        if (evt != null) evt.RemoveAll();
        else evt = new EventVo();
        evt.Add(func);
    }
    #endregion

    #region color相关
    // 修改文本颜色
    public static string Color(string color, string str)
    {
        return string.Format("<color=#{0}>{1}</color>", color, T18N(str));
    }

    // 16进制颜色转Color
    public static Color Hex2Color(string color)
    {
        Color res;
        ColorUtility.TryParseHtmlString(string.Format("#{0}", color), out res);
        return res;
    }
    #endregion

    // json文件名 -》 获取List<T>
    public static List<T> GetJsonList<T>(string fileName)
    {
        List<T> res = new List<T>();

        string jsonStr = Resources.Load<TextAsset>("JsonData/" + fileName).text;
        JsonData jsonData = JsonMapper.ToObject(jsonStr);

        for (int i = 0; i < jsonData.Count; i++)
        {
            var item = JsonMapper.ToObject<T>(jsonData[i].ToJson());
            res.Add(item);
        }

        return res;
    }

    // 中文规范化 TODO
    public static string T18N(string str)
    {
        return str;
    }
}
