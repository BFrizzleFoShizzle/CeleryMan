using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public WorldManager worldmanager;
    public float tilesize = Constants.TILE_SIZE;
    private bool moving = false;

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

    public int x = 5;
    public int z = 0;

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
                transform.position = new Vector3((x-5)*tilesize+0.5f,transform.position.y,z*tilesize);
				startpos = transform.position;
				moving = false;
            }
        }else{
			Vector3 playerPos = transform.position + new Vector3(0.0f,0.1f,0.0f);
             if (Input.GetKey(KeyCode.A)) {
                if((x != 0) && (worldmanager.PlayerCanEnter(playerPos, Vector3.left, tilesize * 1.0f))){
                    targettime = time + stepTime;
					x -= 1;
					targetpos = transform.position + Vector3.left * tilesize;
					moving = true;
				}
            }else if (Input.GetKey(KeyCode.D)) {
                if((x != 9) && (worldmanager.PlayerCanEnter(playerPos, Vector3.right, tilesize * 1.0f))){
					targettime = time + stepTime;
					x += 1;
					targetpos = transform.position + Vector3.right * tilesize;
					moving = true;
                }
            }else if (Input.GetKey(KeyCode.W))
			{
                if(worldmanager.PlayerCanEnter(playerPos, Vector3.forward, tilesize * 1.0f))
				{
                    z += 1;
                    if(worldmanager != null){
                        worldmanager.PlayerAdvanceToRow(z);
                    }
			        targettime = time + stepTime;
              
			        targetpos = transform.position + Vector3.forward * tilesize;
                    moving = true;
                }
            }else if (Input.GetKey(KeyCode.M)) { //Input.GetButtonDown
                if(worldmanager.PlayerCanEnter(playerPos, Vector3.back, tilesize * 1.0f)){
                    z -= 1;
                    if(worldmanager != null){
                        worldmanager.PlayerAdvanceToRow(z);
                    }
			        targettime = time + stepTime;
              
			        targetpos = transform.position + Vector3.back * tilesize;
                    moving = true;
                }
			}
        }
        
    }
}
