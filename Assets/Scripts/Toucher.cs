using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toucher : MonoBehaviour
{
    private float actualTime;
    private float animTime = 0.3f;
    private Material mat;
    public float matEmisive = 0.5f;
    private bool bip;
    private bool touch;
    public int toucherIndex;

    private float zStartPos;
    private float zFinalPos;

    private AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        mat = GetComponent<MeshRenderer>().material;
        mat.SetFloat("_EmColor", 0);
        zStartPos = transform.localPosition.z;
        zFinalPos = zStartPos + 0.01f;
    }

    private void Update()
    {
        if (bip)
            LightEffect();
    }

    void LightEffect()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= 0 && actualTime <= animTime / 2)
        {
            mat.SetFloat("_EmColor", Mathf.Lerp(0, matEmisive, actualTime / (animTime / 2)));
            if (touch)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(zStartPos, zFinalPos, actualTime / (animTime / 2)));

        }
        if (actualTime > animTime / 2 && actualTime <= animTime)
        {
            mat.SetFloat("_EmColor", Mathf.Lerp(matEmisive, 0, actualTime / (animTime / 2) - 1));
            if (touch)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(zFinalPos, zStartPos, actualTime / (animTime / 2) - 1));
        }
        if(actualTime >= animTime)
        {
            mat.SetFloat("_EmColor", 0);
            actualTime = 0;
            bip = false;
            touch = false;
        }
    }

    public void Bip()
    {
        actualTime = 0;
        bip = true;
        audioS.PlayOneShot(audioS.clip);
    }

    public int Touch()
    {
        touch = true;
        Bip();
        return toucherIndex;
    }
}
