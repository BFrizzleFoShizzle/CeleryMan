using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int tilesize = 5;
    private bool moving = false;
    private int leftrightIndex = 0;

	// curve for movement position interpolation
	public AnimationCurve moveCurve;
	// start/end positions for movement
	private Vector3 targetpos;
	public Vector3 startpos = new Vector3(0.5f, 0.0f, 0.0f);
	// time values for movement animation
	private float time = 0.0f;
	private float targettime;
	// time to move 1 tile
	public float stepTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        targetpos = transform.position;
        transform.position = startpos;
    }

    // Update is called once per frame
    void Update(){
        time += Time.deltaTime;
 
        if(moving){
			// 0 when at dest, 1 when at origin
			float stepDelta = (targettime - time) / stepTime;

			//float step = Mathf.SmoothStep(0, 1, stepDelta);
			float step = moveCurve.Evaluate(stepDelta);

			transform.position = (startpos * stepDelta) + (targetpos * (1.0f - stepDelta));

			if (step < 0.001f){
				startpos = transform.position;
				moving = false;
            }
        }else{
             if (Input.GetKey(KeyCode.A)) {
                if(leftrightIndex != 5){
                    targettime = time + stepTime;
					leftrightIndex += 1;
					targetpos = transform.position + Vector3.left * tilesize;
					moving = true;
				}
            }else if (Input.GetKey(KeyCode.D)) {
                if(leftrightIndex != -5)
				{
					targettime = time + stepTime;
					leftrightIndex -= 1;
					targetpos = transform.position + Vector3.right * tilesize;
					moving = true;
                }
            }else if (Input.GetKeyDown(KeyCode.W))
			{
				targettime = time + stepTime;
				targetpos = transform.position + Vector3.forward * tilesize;
                moving = true;
            }else if (Input.GetKeyDown(KeyCode.S)) { //Input.GetButtonDown
                transform.position += Vector3.back * tilesize;
			}
        }
        
    }
}
