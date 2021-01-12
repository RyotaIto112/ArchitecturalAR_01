using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    Transform objectTransform;

    private Vector3 lookAtPosition;
    private Quaternion rot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject CameraOject = GameObject.Find("AR Camera");
        objectTransform = CameraOject.GetComponent<Transform>();

        rot = Quaternion.Euler(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float positionX = objectTransform.position.x;
        float positionY = this.transform.position.y;
        float positionZ = objectTransform.position.z;

        lookAtPosition = new Vector3(positionX, positionY, positionZ); 

        this.GetComponent<Transform>().LookAt(lookAtPosition);
        this.transform.rotation *= rot;
    }
}
