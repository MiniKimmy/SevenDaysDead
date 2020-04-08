using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleController : WeaponBaseController {

    private AssaultRifleView m_AssaultRifleView;

    public override void Start ()
    {
        base.Start();
        this.m_AssaultRifleView = (AssaultRifleView)this.View;
        //weaponType = WeaponEnum.AssaultRifle;
    }
    
    // 点击左键
    protected override void OnLeftMouseBtnDown()
    {
        base.OnLeftMouseBtnDown();
        this.PlaySound(GAssetName.AssaultRifleFireAudio, this.m_AssaultRifleView.GunPointTran.position);
        this.PlayEffect(GAssetName.AssaultRifleFireEffect, this.m_AssaultRifleView.GunPointTran.position, EffectEnum.particalSys);
        this.PlayShellTween();
    }

    // 射击
    protected override void Shoot()
    {
        if(this.Hit.point != Vector3.zero)
        {
            var prefab = this.m_AssaultRifleView.GetPrefabByDict(GAssetName.WeaponBullet);
            var get_bullet_mark = this.Hit.collider.GetComponent<BulletMark>();

            if (get_bullet_mark != null)
            {
                get_bullet_mark.CreateMark(this.Hit);
            }
            else
            {
                var obj = GameObject.Instantiate<GameObject>(prefab, this.Hit.point, Quaternion.identity);
                StartCoroutine(DelayDestoryObj(obj)); // tmp
            }
        }
    }

    // 播放弹壳动画
    private void PlayShellTween()
    {
        var prefab = this.m_AssaultRifleView.GetPrefabByDict(GAssetName.WeaponShell);
        var v3 = this.m_AssaultRifleView.ShellPointTran.position;
        var rigid = UtilsBase.Clone<Rigidbody>(prefab, v3, Quaternion.identity);
        rigid.AddForce(this.m_AssaultRifleView.ShellPointTran.up * 50); // 50力度
        StartCoroutine(DelayDestoryObj(rigid.gameObject)); // tmp
    }
}
