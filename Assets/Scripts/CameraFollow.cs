using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        cameraZ = transform.position.z;
    }

    float cameraZ;


    void Update()
    {
        transform.position += new Vector3(Time.deltaTime * 1, 0, 0);
    }
    public Transform Player;
}
