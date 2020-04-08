using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleView : WeaponBaseView {

    public override void Awake ()
    {
        base.Awake();

        this.GunPointTran = this.Transform.Find("Assault_Rifle/Effect_PosA");
        this.ShellPointTran = this.Transform.Find("Assault_Rifle/Effect_PosB");

        // obj
        this.InitAssetDict(new[]
        {
            GAssetName.AssaultRifleFireEffect,
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
