using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    public float speed = 0.005f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y + speed,transform.position.z);
    }

    public void reset()
    {
        transform.position = new Vector3(0, -50, 0);
    }
}
