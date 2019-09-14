using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int money = 4000; 
    public int moneypersec = 100;
    public Text moneytext;
    public bool Failed;

    public WorldManager worldmanager;
    public PlayerController playercontroller;

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update()
    {
        if(Failed == false){
            money -= (int)(Time.deltaTime*moneypersec);
        }

                    moneytext.text = "Money "+(money)+"$";

        if(money < 0){
            money = 0;
            Failed = true;
            playercontroller.Failed = true;
        }
    }
}
