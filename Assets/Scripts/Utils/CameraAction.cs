using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 pos2D = new Vector3(0.03f, 0.5f, 0.0f);
    int rotate2DX = 90;
    int rotate2DY = 90;
    int rotate2DZ = 0;

    Vector3 pos3D = new Vector3(-0.285f, 0.5f, 0.0f);
    int rotate3DX = 60;
    int rotate3DY = 90;
    int rotate3DZ = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void onChangeCamera2D() {
        transform.position = pos2D;
        transform.rotation = Quaternion.Euler(rotate2DX, rotate2DY, rotate2DZ);
    }

    public void onChangeCamera3D() {
        transform.position = pos3D;
        transform.rotation = Quaternion.Euler(rotate3DX, rotate3DY, rotate3DZ);
    }

}
