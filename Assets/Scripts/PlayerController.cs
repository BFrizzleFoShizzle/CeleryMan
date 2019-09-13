using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int tilesize = 5;
    private bool moving = false;
    private Vector3 targetpos;
    private float targettime;
    public float snapspeed = 1.5f;
    private int leftrightIndex = 0;
    public Vector3 Startpos = new Vector3(0.5f,0.0f,0.0f);

    private float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        targetpos = transform.position;
        transform.position = Startpos;
    }

    // Update is called once per frame
    void Update(){
        time += Time.deltaTime;
 
        if(moving){
            float DistanceToTarget = Vector3.Distance(transform.position, targetpos);
            float ModSnapSpeed = snapspeed*(60-(18*Mathf.Abs(DistanceToTarget-(tilesize/2))));      
            float step =  ModSnapSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetpos, step);
            if (DistanceToTarget < 0.001f){
                moving = false;
            }
        }else{
             if (Input.GetKey(KeyCode.A)) {
                if(leftrightIndex != 5){
                    targetpos = transform.position + Vector3.left * tilesize;
                    moving = true;
                    targettime = time + 0.5f;
                    leftrightIndex += 1;
                }
            }else if (Input.GetKey(KeyCode.D)) {
                if(leftrightIndex != -5){
                    leftrightIndex -= 1;
                    targetpos = transform.position + Vector3.right * tilesize;
                    targettime = time + 0.5f;
                    moving = true;
                }
            }else if (Input.GetKeyDown(KeyCode.W)) {
                targetpos = transform.position + Vector3.forward * tilesize;
                targettime = time + 0.5f;
                moving = true;
            }else if (Input.GetKeyDown(KeyCode.S)) { //Input.GetButtonDown
                transform.position += Vector3.back * tilesize;
            }
        }
        
    }
}
