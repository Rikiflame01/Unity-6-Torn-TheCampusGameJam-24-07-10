using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float randomX = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 360f);
        float randomZ = Random.Range(0f, 360f);

        // Apply the random rotation to the object
        transform.rotation = Quaternion.Euler(0, randomY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
