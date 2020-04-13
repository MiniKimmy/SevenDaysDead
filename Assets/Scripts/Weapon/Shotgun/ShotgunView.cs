using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunView : WeaponBaseView {

    protected override void Awake()
    {
        base.Awake();

        this.GunPointTran = this.Transform.Find("Armature/Weapon/Effect_PosA");
        this.ShellPointTran = this.Transform.Find("Armature/Weapon/Effect_PosB");

        // audio
        this.InitAudioDict(new[]
        {
            GAssetName.ShotgunFireAudio,
            GAssetName.ShotgunPumpAudio,
        });

        // 开镜位置
        this.StPos = m_Transform.localPosition;
        this.StRot = m_Transform.eulerAngles;
        this.EdPos = new Vector3(-0.14f, -1.78f, -0.03f);
        this.EdRot = new Vector3(0, 10, 0);
    }

    // obj资源
    protected override System.Func<string[], string[]> InitAssetsFunc()
    {
        return (defaultAssets) =>
        {
            return new[]
            {
                GAssetName.ShotgunFireEffect,
                GAssetName.WeaponShotGunShell,
            };
        };
    }
}
