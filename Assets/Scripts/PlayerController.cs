using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int tilesize = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            transform.position += Vector3.forward * tilesize;
        }
        else if (Input.GetKeyDown(KeyCode.S)) { //Input.GetButtonDown
            transform.position += Vector3.back * tilesize;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            transform.position += Vector3.left * tilesize;
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            transform.position += Vector3.right * tilesize;
        }
    }
}
