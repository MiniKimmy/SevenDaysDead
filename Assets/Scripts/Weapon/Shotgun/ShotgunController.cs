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
        StartCoroutine("CreateBullet");
    }

    IEnumerator CreateBullet()
    {
        for (int i = 0; i < 5; i++)
        {
            var prefab = this.m_ShotgunView.GetPrefabByDict(GAssetName.WeaponShotGunBullet);
            var bullet = GameObject.Instantiate<GameObject>(prefab, this.m_ShotgunView.GunPointTran.position, Quaternion.identity).GetComponent<ShotgunBullet>();

            var offset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
            bullet.Shoot(this.m_ShotgunView.GunPointTran.forward + offset, 3000f); // 发射方向随机范围±0.05之间
            DelayDestoryObj(bullet.gameObject);
            yield return new WaitForSeconds(0.03f);
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
