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

    private Transform canvasUITran = null;
    public Transform CanvasUITran
    {
        get {
            if (canvasUITran == null) canvasUITran = GameObject.Find("Canvas").transform;
            return canvasUITran;
        }
    }

    void Awake()
    {
        instance = this;
        canvasUITran = GameObject.Find("Canvas").transform;
    }
}
