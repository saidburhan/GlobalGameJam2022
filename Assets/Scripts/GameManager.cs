using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;
	public List<GameObject> toplananlar = new List<GameObject>();
	public List<Vector3> olusturmaPozisyonlari = new List<Vector3>();
	public List<GameObject> toplananPrefablar = new List<GameObject>();
	public int score;
	public AnimationCurve curve;
	public bool isContinue, isStartPanel, isEndPanel;


	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}

	private void Start()
	{
		StartingEvents();
	}



	// Update is called once per frame
	void Update()
    {
		if (isContinue)
		{
			if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Joystick1Button5))
			{
				if (toplananlar.Count > 0)
				{
					StartCoroutine(ToplananlariGonder(toplananlar[0]));
				}
				else
				{
					SetPlayerAvalibility();
				}
			}
		}
		else if (isStartPanel)
		{
			if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetButtonDown("Jump"))
			{
				GameStartEvents();
			}
		}
		else if (isEndPanel)
		{
			if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetButtonDown("Jump"))
			{
				isContinue = false;
				UiController.instance.StartScreenActive();
				isEndPanel = false;
				isStartPanel = true;
			}
		}
		
    }

	public void GameOver()
	{
		UiController.instance.EndPanelActive();
		UiController.instance.SetEndPanelScore();
		PlayerControllerHot.instance.isAvailable = false;
		PlayerControllerCold.instance.isAvailable = false;	
		isEndPanel = true;
		isContinue = false;
	}

	public void GameStartEvents()
	{
		score = 0;
		for (int i = 0; i < toplananlar.Count; i++)
		{
			toplananlar.Remove(toplananlar[i]);
		}
		PlayerControllerHot.instance.slider.value = 0;
		PlayerControllerCold.instance.slider.value = .5f;
		isStartPanel = false;
		isEndPanel = false;
		isContinue = true;
		UiController.instance.GamePanelActive();
		StartCoroutine(CreateFireOrIce());
		PlayerControllerHot.instance.PlayerHotStartingEvent();
		PlayerControllerCold.instance.transform.position = new Vector3(4,-4.9F,-.5F);
		PlayerControllerHot.instance.transform.position = new Vector3(-4,-4.9F,-.5F);
		
	}


	public void StartingEvents()
	{
		score = 0;
		for (int i = 0; i < toplananlar.Count; i++)
		{
			toplananlar.Remove(toplananlar[i]);
		}
		PlayerControllerHot.instance.slider.value = 0;
		PlayerControllerCold.instance.slider.value = .5f;
		PlayerControllerCold.instance.isAvailable = false;
		PlayerControllerHot.instance.isAvailable = false;
		isContinue = false;
		isStartPanel = true;
		isEndPanel = false;
	}

	public IEnumerator ToplananlariGonder(GameObject obj)
	{

		if (PlayerControllerCold.instance.isAvailable)
		{
			StartCoroutine(ParabolikHareket(obj, PlayerControllerCold.instance.gameObject, PlayerControllerHot.instance.gameObject));
			toplananlar.Remove(obj);
			yield return new WaitForSeconds(.2f);
			if (toplananlar.Count > 0) StartCoroutine(ToplananlariGonder(toplananlar[0]));
			else
			{
				yield return new WaitForSeconds(.3f);
				SetPlayerAvalibility();
			}
		}
		else
		{
			StartCoroutine(ParabolikHareket(obj, PlayerControllerHot.instance.gameObject, PlayerControllerCold.instance.gameObject));
			toplananlar.Remove(obj);
			yield return new WaitForSeconds(.2f);
			if (toplananlar.Count > 0) StartCoroutine(ToplananlariGonder(toplananlar[0]));
			else
			{
				yield return new WaitForSeconds(.3f);
				SetPlayerAvalibility();
			}
		}
	}

	public IEnumerator ParabolikHareket(GameObject obj,GameObject start, GameObject end)
	{
		bool devam = true;
		float time = 0;
		float aci = 0;
		while(devam)
		{
			time += 0.02f;
			Vector3 pos = Vector3.Lerp(start.transform.position+Vector3.up, end.transform.position + Vector3.up, time);
			aci += 3.6f;
			pos.y += Mathf.Sin(Mathf.PI/180*aci);
			obj.transform.position = pos;
			yield return new WaitForSeconds(.01f);
			if (aci > 180) devam = false;			
		}
		obj.GetComponent<Collider>().enabled = true;
	}


	public void SetPlayerAvalibility()
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
		}
	}

	public IEnumerator CreateFireOrIce()
	{	
		while (isContinue)
		{
			if (PlayerControllerHot.instance.isAvailable)
			{
				int rnd = Random.Range(0, 7); // 10 oluþturma yerinin ilk 5 i buna ait olacaðý için
				int rnd2 = Random.Range(0,3); // 6 prefabýn ilk üçü buna ait olacaðý için..
				Instantiate(toplananPrefablar[rnd2], olusturmaPozisyonlari[rnd], Quaternion.identity);
				yield return new WaitForSeconds(2);
			}
			else
			{
				int rnd = Random.Range(7, 14); // 10 oluþturma yerinin ilk 5 i buna ait olacaðý için
				int rnd2 = Random.Range(3, 6); // 6 prefabýn ilk üçü buna ait olacaðý için..
				Instantiate(toplananPrefablar[rnd2], olusturmaPozisyonlari[rnd], Quaternion.identity);
				yield return new WaitForSeconds(2);
			}
		}
	}

}
