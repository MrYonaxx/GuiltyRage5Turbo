using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private void OnEnable()
    {
        this.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
    }
}
