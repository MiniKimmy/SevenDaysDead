using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour {

    private Rigidbody m_Rigidbody;
    private RaycastHit m_hit;

    void Awake ()
    {
        m_Rigidbody = this.GetComponent<Rigidbody>();
	}

    public void Shoot(Vector3 dir, float force)
    {
        m_Rigidbody.AddForce(dir * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_Rigidbody.Sleep(); // 停止刚体运动
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.Raycast(ray, out this.m_hit, 100f, LayerMask.NameToLayer("Env")))
        {
            var bullet_mark = this.m_hit.collider.GetComponent<BulletMark>();
            if (bullet_mark != null)
            {
                bullet_mark.CreateMark(this.m_hit);
            }
        }
    }
}
