using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroyer : MonoBehaviour
{
    
    [SerializeField] private float delayInSeconds = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        Invoke(nameof(DestroyObject), delayInSeconds);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void DestroyObject() 
    {
        Destroy(gameObject);
    }
}
