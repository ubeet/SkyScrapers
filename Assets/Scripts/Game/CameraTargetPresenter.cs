using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetPresenter : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    void Start()
    {
        mainCamera ??= Camera.main;
    }

    
    void Update()
    {
        //if(mainCamera == null)
        //    mainCamera = Camera.main;
        //Vector3 eulerAngles = mainCamera.transform.eulerAngles;
        //Quaternion newRotation = Quaternion.Euler(90, eulerAngles.y, eulerAngles.z);
        //transform.rotation = newRotation;
    }
}