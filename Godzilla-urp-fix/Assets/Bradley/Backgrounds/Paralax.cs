using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float length, startpos;

    public GameObject cam;

    public float parralaxAmmount;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parralaxAmmount);
        transform.position = new Vector3(startpos + dist, transform.position.y,transform.position.z);
    }
}
