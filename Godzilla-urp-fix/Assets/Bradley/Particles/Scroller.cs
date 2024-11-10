using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float moveSpeed = 0.5f; 

    private void Update()
    {
   
        Vector3 newPosition = transform.position + Vector3.left * moveSpeed * Time.deltaTime;


        transform.position = newPosition;
    }
}
