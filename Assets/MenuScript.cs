using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
   public static bool menuCalled = false;
   public GameObject menu;
   //public GameObject blocking;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
	{
		if(!menuCalled)
		{
			AppearMenu();
		} else
		{
			DisappearMenu();
		}	

	}
    }

    public void AppearMenu()
    {
	menu.SetActive(true);
	menuCalled = true;
    }
    public void DisappearMenu()
    {
	menu.SetActive(false);
	menuCalled = false;
    }
    public void LoadHUD()
    {
	 SceneManager.LoadScene("finalScene");
    }
    public void LoadControllerTraining()
    {
	 SceneManager.LoadScene("ControllerTraining");
    }
    public void LoadHUDTraining()
    {
	 SceneManager.LoadScene("TrainingFinalScene");
    }
    public void Quit()
    {
    	 Application.Quit();
    }		
}
