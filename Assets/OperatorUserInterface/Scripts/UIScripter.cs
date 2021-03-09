using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScripter : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    public Sprite sprite6;

    // Start is called before the first frame update
    void Start(){
        gameObject.GetComponent<Image>().sprite=sprite1;
    }
    void Update()
    {
	
	if(Input.GetKeyDown("a") && gameObject.GetComponent<Image>().sprite==sprite1 ){
	gameObject.GetComponent<Image>().sprite=sprite2;
	}
	if(Input.GetKeyDown("b") && gameObject.GetComponent<Image>().sprite==sprite2){
	gameObject.GetComponent<Image>().sprite=sprite3;
	}
	if(Input.GetKeyDown("c")&& gameObject.GetComponent<Image>().sprite==sprite3){
	gameObject.GetComponent<Image>().sprite=sprite4;
	}
	if(Input.GetKeyDown("d")&& gameObject.GetComponent<Image>().sprite==sprite4){
	gameObject.GetComponent<Image>().sprite=sprite5;
	}
	if(Input.GetKeyDown("e")&& gameObject.GetComponent<Image>().sprite==sprite5){
	gameObject.GetComponent<Image>().sprite=sprite6;
	}

	
    }

   
}
