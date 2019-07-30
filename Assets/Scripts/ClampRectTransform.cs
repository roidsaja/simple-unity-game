using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampRectTransform : MonoBehaviour {

    public float padding = 10.0f;
    public float elementSize = 128.0f;
    public float viewSize = 250.0f;

    private RectTransform rt;
    private int amountelements;
    private float contentsize;
    
    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //clamp our rect transform
        amountelements = rt.childCount;
        contentsize = ((amountelements * (elementSize + padding)) - viewSize) * rt.localScale.x;

        if(rt.localPosition.x > padding)
        {
            rt.localPosition = new Vector3(padding, rt.localPosition.y, rt.localPosition.z);
        }
        else if (rt.localPosition.x < - contentsize)
        {
            rt.localPosition = new Vector3(-contentsize, rt.localPosition.y, rt.localPosition.z);
        }
    }
}
