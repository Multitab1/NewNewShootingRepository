using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutlineWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
