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
	public Transform toplananlarParent;


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
					SetPlayerAvalibility();
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
		isContinue = false;
		PlayerControllerHot.instance.isAvailable = false;
		PlayerControllerCold.instance.isAvailable = false;
		PlayerControllerHot.instance.playerAnim.ResetTrigger("run");
		PlayerControllerCold.instance.playerAnim.ResetTrigger("run");
		PlayerControllerHot.instance.playerAnim.SetTrigger("idle");
		PlayerControllerCold.instance.playerAnim.SetTrigger("idle");
		UiController.instance.EndPanelActive();
		UiController.instance.SetEndPanelScore();
		PlayerControllerHot.instance.toplanan = 0;
		PlayerControllerCold.instance.toplanan = 0;
		UiController.instance.SetEldeki1Text();
		UiController.instance.SetEldeki2Text();
		for (int i = 0; i < toplananlar.Count; i++)
		{
			Destroy(toplananlar[i].gameObject);
		}
		toplananlar.Clear();
		int count = toplananlarParent.childCount;
		for (int i = 0; i < count; i++)
		{
			Destroy(toplananlarParent.GetChild(i).gameObject);
			count = toplananlarParent.childCount;
		}
	}

	public void GameStartEvents()
	{
		score = 0;
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
		PlayerControllerCold.instance.isAvailable = false;
		AllIdleAnim();
	}


	public void StartingEvents()
	{
		AllIdleAnim();
		score = 0;
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

		if (!PlayerControllerCold.instance.isAvailable)
		{
			if (obj.CompareTag("b5") || obj.CompareTag("b10") || obj.CompareTag("b20"))
			{
				StartCoroutine(ParabolikHareket(obj, PlayerControllerCold.instance.gameObject, PlayerControllerHot.instance.gameObject));
				toplananlar.Remove(obj);
				yield return new WaitForSeconds(.2f);
				if (toplananlar.Count > 0) StartCoroutine(ToplananlariGonder(toplananlar[0]));
				PlayerControllerCold.instance.toplanan = 0;
				UiController.instance.SetEldeki2Text();
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
		else
		{
			if (obj.CompareTag("a5") || obj.CompareTag("a10") || obj.CompareTag("a20"))
			{
				StartCoroutine(ParabolikHareket(obj, PlayerControllerHot.instance.gameObject, PlayerControllerCold.instance.gameObject));
				toplananlar.Remove(obj);
				yield return new WaitForSeconds(.2f);
				if (toplananlar.Count > 0) StartCoroutine(ToplananlariGonder(toplananlar[0]));
				PlayerControllerHot.instance.toplanan = 0;
				UiController.instance.SetEldeki1Text();
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}

	public IEnumerator ParabolikHareket(GameObject obj,GameObject start, GameObject end)
	{
		obj.transform.parent = null;
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
			AllIdleAnim();
			StartCoroutine(PlayerControllerHot.instance.SliderActive());

		}
		else
		{
			PlayerControllerHot.instance.isAvailable = false;
			PlayerControllerCold.instance.isAvailable = true;
			AllIdleAnim();
			StartCoroutine(PlayerControllerCold.instance.SliderActive());
		}

	}

	public void ResetAllAnim()
	{
		PlayerControllerCold.instance.playerAnim.ResetTrigger("run");
		PlayerControllerCold.instance.playerAnim.ResetTrigger("idle");
		PlayerControllerHot.instance.playerAnim.ResetTrigger("run");
		PlayerControllerHot.instance.playerAnim.ResetTrigger("idle");
	}


	public void AllIdleAnim()
	{
		PlayerControllerCold.instance.playerAnim.ResetTrigger("run");
		PlayerControllerCold.instance.playerAnim.SetTrigger("idle");
		PlayerControllerHot.instance.playerAnim.ResetTrigger("run");
		PlayerControllerHot.instance.playerAnim.SetTrigger("idle");
	}

	public IEnumerator CreateFireOrIce()
	{	
		while (isContinue)
		{
			if (PlayerControllerHot.instance.isAvailable)
			{
				int rnd = Random.Range(0, 7); // 14 oluþturma yerinin ilk 5 i buna ait olacaðý için
				int rnd2 = Random.Range(0,3); // 6 prefabýn ilk üçü buna ait olacaðý için..
				GameObject obj = Instantiate(toplananPrefablar[rnd2], olusturmaPozisyonlari[rnd], Quaternion.identity);
				obj.transform.parent = toplananlarParent;
				yield return new WaitForSeconds(2.1f);
			}
			else
			{
				int rnd = Random.Range(7, 14); // 14 oluþturma yerinin ilk 5 i buna ait olacaðý için
				int rnd2 = Random.Range(3, 6); // 6 prefabýn ilk üçü buna ait olacaðý için..
				GameObject obj = Instantiate(toplananPrefablar[rnd2], olusturmaPozisyonlari[rnd], Quaternion.identity);
				obj.transform.parent = toplananlarParent;
				yield return new WaitForSeconds(2.1f);
			}
		}

	
	}

}
