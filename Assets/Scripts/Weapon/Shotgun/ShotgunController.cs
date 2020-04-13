using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : WeaponBaseController
{
    private ShotgunView m_ShotgunView;

    public override void Start()
    {
        base.Start();
        this.m_ShotgunView = (ShotgunView)this.View;
    }

    // 射击
    protected override void Shoot()
    {
        for (int i = 0; i < 5; i++)
        {
            var prefab = this.m_ShotgunView.GetPrefabByDict(GAssetName.WeaponBullet);

            //GameObject.Instantiate<GameObject>(prefab, this.Hit.point, Quaternion.identity);
        }
    }

    // 点击左键
    protected override void OnLeftMouseBtnDown()
    {
        base.OnLeftMouseBtnDown();
        this.PlayEffect(GAssetName.ShotgunFireEffect, this.m_ShotgunView.GunPointTran.position, EffectEnum.particalSys);
        this.PlaySound(GAssetName.ShotgunFireAudio, this.m_ShotgunView.GunPointTran.position);
    }

    // 播放弹壳动画
    private void PlayShellTween()
    {
        var prefab = this.m_ShotgunView.GetPrefabByDict(GAssetName.WeaponShell);
        var v3 = this.m_ShotgunView.ShellPointTran.position;
        var rigid = UtilsBase.Clone<Rigidbody>(prefab, v3, Quaternion.identity);
        rigid.AddForce(this.m_ShotgunView.ShellPointTran.up * 50);
        StartCoroutine(DelayDestoryObj(rigid.gameObject));
    }

    // 动画控制音效播放
    private void PlayShellAudioAni()
    {
        this.PlaySound(GAssetName.ShotgunPumpAudio, this.m_ShotgunView.ShellPointTran.position);
    }
}
