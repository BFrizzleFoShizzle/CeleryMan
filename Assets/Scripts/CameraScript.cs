using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float followdistance = 40.0f;
    public GameObject player;
	private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 idealpos = new Vector3(transform.position.x,transform.position.y,player.transform.position.z - followdistance);
		transform.position = Vector3.SmoothDamp(transform.position, idealpos, ref velocity, 0.05f);
	}
}
