using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBaseController : MonoBehaviour {

    private WeaponItem vo = null;
    public WeaponItem Data { get { return this.vo; } }

    protected WeaponBaseView m_BaseView;
    public WeaponBaseView View { get { return this.m_BaseView; } }

    private RaycastHit m_hit;
    public RaycastHit Hit { get { return this.m_hit; } }

    public virtual void Start ()
    {
        m_BaseView = this.GetComponent<WeaponBaseView>();
    }
	
	public virtual void Update ()
    {
        this.ShootReady();
        if (Input.GetMouseButtonDown(0))
        {
            this.OnLeftMouseBtnDown();
        }
        else if (Input.GetMouseButton(1))
        {
            this.OnRightMouseBtn();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            this.OnRightMouseBtnUp();
        }
    }

    // 射击
    protected abstract void Shoot();

    // 射击准备
    private void ShootReady()
    {
        //Debug.Log("射击准备");
        Ray ray = new Ray(this.View.GunPointTran.position, this.View.GunPointTran.forward);
        //Debug.DrawLine(this.View.GunPointTran.position, this.View.GunPointTran.forward * 10);

        if (Physics.Raycast(ray, out m_hit))
        {
            // update实时更新准星位置
            var v2 = RectTransformUtility.WorldToScreenPoint(View.EnvCamera, m_hit.point);
            View.GunStarTran.position = v2;
        }
        else m_hit.point = Vector3.zero; // 置空hit.point
    }

    protected void PlaySound(string assetNames, Vector3 pos)
    {
        //Debug.Log("播放音效");
        var audio_clip = this.View.GetAudioByDict(assetNames);
        UtilsBase.PlaySound(audio_clip, pos);
    }

    protected void PlayEffect(string assetNames, Vector3 pos, EffectEnum @enum)
    {
        //Debug.Log("播放特效");
        var prefab = this.View.GetPrefabByDict(assetNames);
        switch (@enum)
        {
            case EffectEnum.particalSys:
                ParticleSystem particleSystem = UtilsBase.Clone<ParticleSystem>(prefab, pos, Quaternion.identity);
                particleSystem.Play();
                StartCoroutine(DelayDestoryObj(particleSystem.gameObject));
                break;
            case EffectEnum.obj:
                GameObject obj = GameObject.Instantiate<GameObject>(prefab, pos, Quaternion.identity);
                StartCoroutine(DelayDestoryObj(obj));
                break;
            default: break;
        }
    }

    protected virtual void OnLeftMouseBtnDown()
    {
        //Debug.Log("按下左键");
        this.View.Animator.SetTrigger("Fire");
        this.Shoot();
    }

    protected virtual void OnRightMouseBtn()
    {
        //Debug.Log("按住右键");
        this.View.Animator.SetBool("HoldPose", true);
        this.View.EnterHoldPost();
    }

    protected virtual void OnRightMouseBtnUp()
    {
        //Debug.Log("抬起右键");
        this.View.Animator.SetBool("HoldPose", false);
        this.View.ExitHoldPost();
    }

    protected IEnumerator DelayDestoryObj(GameObject go, float time = 5f)
    {
        yield return new WaitForSeconds(time);
        GameObject.Destroy(go);
    }
}
