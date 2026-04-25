using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{

    public GameObject bulletObject;
    public GameObject bulletFireObject;
    // Update is called once per frame
    void Update()
    {
        bool isFire = Input.GetButtonDown("Jump");
        if (isFire)
        {
            GameObject bullet = Instantiate(bulletObject);
            bullet.transform.position = bulletFireObject.transform.position;
        }
    }
}
