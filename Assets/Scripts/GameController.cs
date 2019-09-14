using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int money; 
    public int moneypersec = 100;
    public Text moneytext;
    public bool FAILED;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Restart(){
        money = 4000;
    }

    // Update is called once per frame
    void Update()
    {
        money -= (int)(Time.deltaTime*moneypersec);
        moneytext.text = "Money "+(money)+"$";
    }
}
