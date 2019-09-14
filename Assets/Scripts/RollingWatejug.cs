using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingWatejug : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(-5, rb.velocity.y, 0);
    }

    // Update is called once per frame
    void Update()
    {

	}
}
