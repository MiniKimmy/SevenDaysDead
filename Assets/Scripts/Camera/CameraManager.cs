using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private static CameraManager instance = null;
    public static CameraManager Instance
    {
        get
        {
            if (instance == null) instance = new CameraManager();
            return instance;
        }
    }

    private Transform canvasTran;

    public Transform CanvasTran
    {
        get { return canvasTran; }
        set { canvasTran = value; }
    }

    void Awake()
    {
        instance = this;
        canvasTran = GameObject.Find("Canvas").transform;
    }
}
