using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuug : MonoBehaviour
{
    public Transform sun;

    void Start()
    {
        Debug.Log("pos: " + sun.position + " rot: " + sun.rotation.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("pos: " + sun.position + " rot: " + sun.rotation.eulerAngles);
    }
}
