using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour
{

	public float speed = 5;
	private Rigidbody rb;
	private bool hasDoneMeanStunts = false;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		Debug.Assert(rb != null);

		if (Random.Range(0, 2) == 1)
			speed = -speed;

		rb.velocity = new Vector3(speed, 0, 0);
	}

    // Update is called once per frame
    void Update()
	{
		rb.velocity = new Vector3(speed, rb.velocity.y, 0);

		// randomly jump objects
		if (Random.Range(0, 100) == 1)
			rb.velocity = rb.velocity + (new Vector3(0, Random.Range(1, 3), 0));

	}

	public bool HasDoneMeanStunts()
	{
		return hasDoneMeanStunts;
	}

	public void DoMeanStunts()
	{
		hasDoneMeanStunts = true;
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
