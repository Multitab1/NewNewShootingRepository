using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float spd = 1.0f;
    Vector3 direct = Vector3.down;

    public GameObject target;

    private void Start()
    {
        int rndNum = Random.Range(0, 10);
        if (rndNum % 3 == 0 )
        {
            direct = target.transform.position - target.transform.position;
            direct.Normalize();
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }




}
