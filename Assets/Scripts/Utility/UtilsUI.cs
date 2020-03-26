using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UtilsUI  {

    // setActive
	public static void SetScaleActive(Transform t, bool isActive)
	{
		if (isActive){
			t.localScale = Vector3.one;
		} else {
			t.localScale = Vector3.zero;
		}
	}

    // setActive
    public static void SetActive(GameObject obj, bool isActive)
    {
        if (isActive != obj.activeSelf)
        {
            obj.SetActive(isActive);
        }
    }

    // setWidth
    public static void SetWidth(RectTransform rect, float width)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    // setHeight
    public static void SetHeight(RectTransform rect, float height)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    // setWidthAndHeight
    public static void SetWidthAndHeight(RectTransform rect, float width, float height)
    {
        rect.sizeDelta = new Vector2(width, height);
    }

    // setWidthAndHeight
    public static void SetWidthAndHeight(Transform t, float width, float height)
    {
        SetWidthAndHeight(t.GetComponent<RectTransform>(), width, height);
    }

    // find
    public static T GetComponent<T>(Transform t, string path = "") where T : class
    {
        return t.Find(path).GetComponent<T>();
    }

    // button监听
    public static void AddButtonListener(Transform t, string path = "", UnityEngine.Events.UnityAction call = null)
    {
        t.Find(path).GetComponent<Button>().onClick.RemoveAllListeners();
        t.Find(path).GetComponent<Button>().onClick.AddListener(call);
    }

    // ui打开/关闭
    public static void SetUIActive(IUIPanelShowAndHide ui, bool active)
    {
        if (active)
        {
            ui.UIPanelShow();
        } else {
            ui.UIPanelHide();
        }
    }
}
