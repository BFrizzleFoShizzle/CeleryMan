using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public WorldManager worldmanager;
    public GameObject ghostmarker;
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
    public bool ChargingSmash = false;

    private Vector3 StartScale;
    private float SmashBlocks = 0.0f;
    private GameObject ghostmarkerinst;

    // Start is called before the first frame update
    void Start()
    {
        targetpos = transform.position;
        transform.position = startpos;
        StartScale = transform.localScale;
        ghostmarkerinst = Instantiate(ghostmarker, transform.position, Quaternion.identity);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "HitPlayer"){
            Failed = true;
        }
    }

    // Update is called once per frame
    void Update(){
        time += Time.deltaTime;

        //print("GETBUTTON, AXISRAW "+Input.GetButton("Vertical")+","+Input.GetAxisRaw("Vertical"));

        if(Input.GetButton("Cancel"))
        {
            SceneManager.LoadScene(0);
        }

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
            }else{
                float stepDelta = (targettime - time) / stepTime;
			    float step = moveCurve.Evaluate(stepDelta);
			    transform.position = (startpos * stepDelta) + (targetpos * (1.0f - stepDelta));
                transform.localScale = StartScale;
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

            }else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0) {
                float starttime = 0;
                SmashBlocks = 0.0f;
                
                if(ChargingSmash){
                    
                    SmashBlocks = (time-starttime)/0.5f;
                    ghostmarkerinst.transform.position = transform.position + Vector3.forward*((int)(SmashBlocks));
                    if(SmashBlocks > 5){
                        SmashBlocks = 5.0f;
                    }else{
                        float scaleMo = Time.deltaTime*0.3f;
                        transform.localScale = new Vector3(transform.localScale.x+transform.localScale.x*scaleMo,transform.localScale.y+transform.localScale.y*scaleMo,transform.localScale.z+transform.localScale.z*scaleMo);
                    }
                    
                }else{
                    ChargingSmash = true;
                    starttime = time;
                    ghostmarkerinst.transform.position = transform.position;
                }
            }else if (ChargingSmash && (Input.GetButton("Vertical") == false)){
                int WholeBlocks = (int)(SmashBlocks);
                z += WholeBlocks;
                if(worldmanager != null){
                    worldmanager.PlayerAdvanceToRow(z);
                }
			    targettime = time + stepTime;
              
			    targetpos = transform.position + Vector3.forward * tilesize*WholeBlocks;
                moving = true;
                ChargingSmash = false;

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
