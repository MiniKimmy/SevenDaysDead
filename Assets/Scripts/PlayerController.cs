using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Transform m_Transform;

    // 预加载所有武器.
    private GameObject m_Building_Plan;
    private GameObject m_WoodenSpear;

    [SerializeField] private GameObject currentWeapon;

    void Start ()
    {
        m_Transform = this.GetComponent<Transform>();
        m_Building_Plan = m_Transform.Find("PlayerCamera/Building Plan").gameObject;
        m_WoodenSpear = m_Transform.Find("PlayerCamera/Wooden Spear").gameObject;

        m_WoodenSpear.SetActive(false);
  
        currentWeapon = m_Building_Plan;
    }
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.M))
        {
            Change(m_Building_Plan);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Change(m_WoodenSpear);
        }
    }

    private void Change(GameObject target)
    {
        currentWeapon.GetComponent<Animator>().SetTrigger("Holster");
        StartCoroutine("Delay", target);
    }

    IEnumerator Delay(GameObject targetWeapon)
    {
        yield return new WaitForSeconds(1);
        currentWeapon.SetActive(false);
        targetWeapon.SetActive(true);

        currentWeapon = targetWeapon;
    }
}
