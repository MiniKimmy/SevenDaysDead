using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleView : WeaponBaseView {

    protected override void Awake ()
    {
        base.Awake();

        this.GunPointTran = this.Transform.Find("Assault_Rifle/Effect_PosA");
        this.ShellPointTran = this.Transform.Find("Assault_Rifle/Effect_PosB");

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

    // obj资源
    protected override Func<string[], string[]> InitAssetsFunc()
    {
        return (defaultAssets) =>
        {
            // 自定义obj资源
            var assets = new[]
            {
                GAssetName.AssaultRifleFireEffect,
            };

            // 自定义+default资源
            ArrayList res = new ArrayList(defaultAssets.Length + assets.Length);
            res.AddRange(defaultAssets);
            res.AddRange(assets);
            return Array.ConvertAll<object, string>(res.ToArray(), (a) => { return a.ToString(); });
        };
    }
}
