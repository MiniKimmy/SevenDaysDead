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
        this.PlaySound(GAssetName.ShotgunFireAudio, this.m_ShotgunView.GunPointTran.position);
        this.PlayEffect(GAssetName.ShotgunFireEffect, this.m_ShotgunView.GunPointTran.position, EffectEnum.particalSys);
    }
}
