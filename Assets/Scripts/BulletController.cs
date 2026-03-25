using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float spd = 3.5f;
 
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * spd * Time.deltaTime);
    }
}
