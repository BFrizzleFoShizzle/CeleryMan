using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu: MonoBehaviour
{
    // Update is called once per frame
    public void Begin()
    {
        SceneManager.LoadScene(1);
    }

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

    public void Quit()
    {
        Application.Quit();
    }
}
