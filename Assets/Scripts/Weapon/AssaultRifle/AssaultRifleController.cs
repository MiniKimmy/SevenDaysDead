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
    
    protected override void OnLeftMouseBtnDown()
    {
        base.OnLeftMouseBtnDown();
        this.PlaySound(GAssetName.AssaultRifleFireAudio, this.m_AssaultRifleView.GunPointTran.position);
        this.PlayEffect(GAssetName.AssaultRifleFireEfect, this.m_AssaultRifleView.GunPointTran.position, EffectEnum.particalSys);

        var prefab = this.m_AssaultRifleView.GetPrefabByDict(GAssetName.WeaponShell);
        var v3 = this.m_AssaultRifleView.ShellPointTran.position;
        UtilsBase.Clone<Rigidbody>(prefab, v3, Quaternion.identity).AddForce(this.m_AssaultRifleView.ShellPointTran.up * 50); // 50力度
    }

    protected override void Shoot()
    {
        //Debug.Log("开始射击");
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
                GameObject.Instantiate<GameObject>(prefab, this.Hit.point, Quaternion.identity);
            }
        }
    }
}
