using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour
{

	public float speed = 5;
	private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		Debug.Assert(rb != null);
		rb.velocity = new Vector3(speed, 0, 0);
	}

    // Update is called once per frame
    void Update()
	{
		rb.velocity = new Vector3(speed, rb.velocity.y, 0);
		//if(transform.position > 5)

	}

	public void FlipDirection()
	{
		if(rb == null)
			rb = GetComponent<Rigidbody>();

		Debug.Assert(rb != null);
		speed = -speed;
		rb.velocity = new Vector3(speed, 0, 0);
	}
}
