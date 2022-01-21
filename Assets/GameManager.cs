using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;
	public int score;


	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}



	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.H))
		{
			if (PlayerControllerCold.instance.isAvailable)
			{
                PlayerControllerCold.instance.isAvailable = false;
                PlayerControllerHot.instance.isAvailable = true;
				StartCoroutine(PlayerControllerHot.instance.SliderActive());
				
			}
			else
			{
                PlayerControllerHot.instance.isAvailable = false;
                PlayerControllerCold.instance.isAvailable = true;
				StartCoroutine(PlayerControllerCold.instance.SliderActive());
				PlayerControllerHot.instance.HotToCold();
			}
		}
    }
}
