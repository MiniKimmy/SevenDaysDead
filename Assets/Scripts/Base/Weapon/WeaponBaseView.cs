using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class WeaponBaseView : BaseView {

    private Camera evn_Camera; // tmp
    public Camera EnvCamera { get { return evn_Camera; } }

    // 准星
    private Transform m_GunStarTran;
    public Transform GunStarTran { get { return m_GunStarTran; } }

    // 枪口位置
    public Transform GunPointTran { get; protected set; }

    // 动作
    private Animator m_Animator;
    public Animator Animator { get { return m_Animator; } }

    // 开镜动作
    public virtual Vector3 StPos { get; protected set; }
    public virtual Vector3 EdPos { get; protected set; }
    public virtual Vector3 StRot { get; protected set; }
    public virtual Vector3 EdRot { get; protected set; }

    protected Dictionary<string, AudioClip> audios_clip = null; // audio

    public override void Awake()
    {
        base.Awake();

        evn_Camera = GameObject.Find("EnvCamera").GetComponent<Camera>();
        m_GunStarTran = CameraManager.Instance.CanvasUITran.Find("MainPanel/GunStar");
        m_Animator = this.Transform.GetComponent<Animator>();
    }

    protected void InitAudioDict(string[] assetNames)
    {
        if (audios_clip != null)
        {
            UtilsBase.ddd("[InitAudioDict]方法仅允许调用1次初始化");
            return;
        }

        this.audios_clip = new Dictionary<string, AudioClip>();
        for (int i = 0; i < assetNames.Length; i ++)
        {
            string name = assetNames[i];
            this.audios_clip.Add(name, Resources.Load<AudioClip>(name));
        }
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
