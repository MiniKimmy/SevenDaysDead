using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class WeaponBaseView : BaseView {

    private Camera evn_Camera; // tmp
    public Camera EnvCamera { get { return evn_Camera; } }

    // 准星
    private Transform m_GunStarTran;
    public Transform GunStarTran { get { return m_GunStarTran; } }

    // 枪口位置
    public Transform GunPointTran { get; protected set; }

    // 弹壳位置
    public Transform ShellPointTran { get; protected set; }

    // 动作ani
    private Animator m_Animator;
    public Animator Animator { get { return m_Animator; } }

    // 开镜动作
    public virtual Vector3 StPos { get; protected set; }
    public virtual Vector3 EdPos { get; protected set; }
    public virtual Vector3 StRot { get; protected set; }
    public virtual Vector3 EdRot { get; protected set; }

    protected Dictionary<string, AudioClip> audios_clip = null; // audio

    protected override void Awake()
    {
        base.Awake();
        evn_Camera = GameObject.Find("EnvCamera").GetComponent<Camera>();
        m_GunStarTran = CameraManager.Instance.CanvasUITran.Find("MainPanel/GunStar");
        m_Animator = this.Transform.GetComponent<Animator>();
        InitAssetDict();
    }

    // obj资源
    protected void InitAssetDict()
    {
        // 默认资源
        var defaultAssets = new[]
        {
            GAssetName.WeaponBullet,
            GAssetName.WeaponShell,
        };
 
        var func = this.InitAssetsFunc();
        if (func != null)
        {
            defaultAssets = func(defaultAssets);
        }
 
        base.InitAssetDict(defaultAssets);
    }

    // audio资源
    protected void InitAudioDict(string[] assetNames)
    {
        if (this.audios_clip == null)
        {
            this.audios_clip = new Dictionary<string, AudioClip>();
        }

        for (int i = 0; i < assetNames.Length; i ++)
        {
            string name = assetNames[i];
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogErrorFormat("【InitAudioDict】传入name是空字符串!!");
                return;
            }

            if (this.audios_clip.ContainsKey(name))
            {
                UtilsBase.ddd("【InitAudioDict】重复添加资源", name, "检查是否使用重复的GAssetName");
                continue;
            }

            this.audios_clip.Add(name, Resources.Load<AudioClip>(name));
        }
    }

    // obj资源(自定义资源)
    protected virtual Func<string[], string[]> InitAssetsFunc() {
        return null; // 第一个参数会传入defaultAssets, 自行取舍, TOutput是最终的obj资源
    }

    // assetName:定义在 GAssetName.cs
    public AudioClip GetAudioByDict(string assetName)
    {
        AudioClip res = UtilsBase.ByNameGetAsset<AudioClip>(assetName, audios_clip);
        if (res == null) UtilsBase.ddd("请检查是否添加过该资源到【InitAudioDict】方法中", assetName);
        return res;
    }

    // 进入开镜动作(动作资源优化)
    public void EnterHoldPost(float duration = 0.2f, float edfov = 40f)
    {
        this.Transform.DOLocalMove(EdPos, duration);
        this.Transform.DOLocalRotate(EdRot, duration);
        this.evn_Camera.DOFieldOfView(edfov, duration);
        UtilsUI.SetScaleActive(this.m_GunStarTran, false);
    }

    // 退出开镜动作(动作资源优化)
    public void ExitHoldPost(float duration = 0.2f, float edfov = 60f)
    {
        m_Transform.DOLocalMove(StPos, duration);
        m_Transform.DOLocalRotate(StRot, duration);
        this.evn_Camera.DOFieldOfView(edfov, duration);
        UtilsUI.SetScaleActive(this.m_GunStarTran, true);
    }
}