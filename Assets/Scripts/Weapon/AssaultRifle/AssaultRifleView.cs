using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleView : WeaponBaseView {

    private Transform m_ShellPointTran;  // 弹夹位置
    public Transform ShellPointTran { get { return m_ShellPointTran; } }

    public override void Awake ()
    {
        base.Awake();

        GunPointTran = this.Transform.Find("Assault_Rifle/Effect_PosA");
        m_ShellPointTran = this.Transform.Find("Assault_Rifle/Effect_PosB");

        // obj
        this.InitAssetDict(new[]
        {
            GAssetName.WeaponBullet,
            GAssetName.AssaultRifleFireEfect,
            GAssetName.WeaponShell,
        });

        // audio
        this.InitAudioDict(new[]
        {
            GAssetName.AssaultRifleFireAudio
        });

        // 开镜位置
        this.StPos = m_Transform.localPosition;
        this.StRot = m_Transform.eulerAngles;
        this.EdPos = new Vector3(-0.065f, -1.85f, 0.25f);
        this.EdRot = new Vector3(2.8f, 1.3f, 0.08f);
    }
}
