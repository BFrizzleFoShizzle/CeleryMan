using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int money = 4000; 
    public int moneypersec = 100;
    public Text moneytext;
    public Text timeText;

    public int currentTimeScore;
    private float startTime;
    
    public Color32 moneyColorNormal;
    public Color32 moneyColorLow;

    public WorldManager worldmanager;
    public PlayerController playercontroller;

    // Start is called before the first frame update
    void Start(){
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(playercontroller.Failed == false){
            money -= (int)(Time.deltaTime*moneypersec);
            currentTimeScore = Mathf.RoundToInt((Time.time - startTime)* 10);
        } else {
            money = 0;
        }

        timeText.text = "" + currentTimeScore;
        moneytext.text = "$"+(money);
        moneytext.color = (money<Constants.MONEY__LOW_THRESHOLD)? moneyColorLow: moneyColorNormal;

        if (worldmanager.CheckPaydayReached()) {
            money += Constants.MONEY_PAYDAY;
        }

        if(money < 0){
            money = 0;
            playercontroller.Failed = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("TESTING");
    }
}
