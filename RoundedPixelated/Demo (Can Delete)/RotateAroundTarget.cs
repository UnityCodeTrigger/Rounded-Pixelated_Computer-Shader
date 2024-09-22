using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateAroundTarget : MonoBehaviour
{
    
    public Transform target;
    public float offset;
    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        float x = Mathf.Cos(time) * offset;
        float z = Mathf.Sin(time) * offset;
        transform.position = new Vector3(x + target.position.x, transform.position.y, z + target.position.z);
    }

}
