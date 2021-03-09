using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripter : MonoBehaviour
{
   public static bool menuCalled = false;
   public GameObject menu;
   public GameObject xyz;
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
	if(xyz != null)
	{
		xyz.SetActive(false);
	}
	menuCalled = true;
    }
    public void DisappearMenu()
    {
	menu.SetActive(false);
	if(xyz != null)
	{
		xyz.SetActive(true);
	}
	menuCalled = false;
    }
    public void LoadHUD()
    {		
   	SceneManager.LoadScene("finalScene");
	SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
    public void LoadControllerTraining()
    {

	SceneManager.LoadScene("ControllerTraining");
	SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
    public void LoadHUDTraining()
    {
	
	SceneManager.LoadScene("TrainingFinalScene");
	SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
    public void Quit()
    {
    	 Application.Quit();
    }		
}
