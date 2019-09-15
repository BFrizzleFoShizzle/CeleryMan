using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPrinter : MonoBehaviour
{
	Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		Debug.Assert(rb != null);

		ResetPosition();
		Debug.Log("Printer spawned");
	}

    // Update is called once per frame
    void Update()
    {
		rb.velocity = new Vector3(20, 0, 0);
		if (transform.position.x > Constants.ROW_WIDTH * Constants.TILE_SIZE)
			ResetPosition();
    }

	private void ResetPosition()
	{
		transform.position = new Vector3(transform.localPosition.x - (30 + Random.Range(10, 30)), transform.position.y, transform.position.z);
	}
}
