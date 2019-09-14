using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public WorldManager worldmanager;
    public float tilesize = Constants.TILE_SIZE;
    public bool moving = false;

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

    //Ints for position, playpos is sent to the worldmannager from the playerconotller 
    public int x = 5;
    public int z = 0;

    public bool Failed = false;

    //Have you hit payday, 
    public bool HitPayday = false;

    // Start is called before the first frame update
    void Start()
    {
        targetpos = transform.position;
        transform.position = startpos;
    }

    // Update is called once per frame
    void Update(){
        time += Time.deltaTime;

        if(Failed){
            transform.position += Vector3.up * Time.deltaTime*1;
            transform.Rotate(0.0f, 50.0f* Time.deltaTime, 0.0f, Space.Self);
            float scaleM = Time.deltaTime*0.6f;
            Vector3 targetDir = Vector3.back;
            float step = 2.5f * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            
            transform.rotation = Quaternion.LookRotation(newDir);
            if (transform.localScale.x < 1.75){
                transform.localScale = new Vector3(transform.localScale.x+transform.localScale.x*scaleM,transform.localScale.y+transform.localScale.y*scaleM,transform.localScale.z+transform.localScale.z*scaleM);
            }
            return;
        }
        
        if(moving){
			if (time > targettime){ //End of movement       
				transform.position = targetpos;
				startpos = transform.position;
                moving = false;
                if(worldmanager.IsPaydayRow(z)){
                    HitPayday = true;
                }
            }else{
                float stepDelta = (targettime - time) / stepTime;
			    float step = moveCurve.Evaluate(stepDelta);
			    transform.position = (startpos * stepDelta) + (targetpos * (1.0f - stepDelta));
            }
        }else{
			Vector3 playerPos = transform.position + new Vector3(0.0f,0.1f,0.0f);
            //LEFT 
            //if (Input.GetKey(KeyCode.A)) {
             if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) { 
                if((x != 0) && (worldmanager.PlayerCanEnter(playerPos, Vector3.left, tilesize * 1.0f))){
                    targettime = time + stepTime;
					x -= 1;
                    
					targetpos = transform.position + Vector3.left * tilesize;
					moving = true;
				}
            }
             //RIGHT
             //else if (Input.GetKey(KeyCode.D)) {
             else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
            {
                if((x != 9) && (worldmanager.PlayerCanEnter(playerPos, Vector3.right, tilesize * 1.0f))){
					targettime = time + stepTime;
					x += 1;

					targetpos = transform.position + Vector3.right * tilesize;
					moving = true;
                }
            }
            //F 
            //else if (Input.GetKey(KeyCode.W)){
			else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0) {
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
