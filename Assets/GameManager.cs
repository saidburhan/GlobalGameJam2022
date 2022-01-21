using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    


    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.H))
		{
			if (PlayerControllerCold.instance.isAvailable)
			{
                PlayerControllerCold.instance.isAvailable = false;
                PlayerControllerHot.instance.isAvailable = true;
			}
			else
			{
                PlayerControllerHot.instance.isAvailable = false;
                PlayerControllerCold.instance.isAvailable = true;
			}
		}
    }
}
