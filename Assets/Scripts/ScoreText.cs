using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
	private Text text;
    // Start is called before the first frame update
    void Start()
    {
		text = GetComponent<Text>();
		text.text = "Final Score: " + GameController.score;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
