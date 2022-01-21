using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;
	public List<GameObject> toplananlar = new List<GameObject>();
	public int score;
	public AnimationCurve curve;


	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}

	private void Start()
	{
		DOTween.Init();
		curve = new AnimationCurve();
	}



	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.H))
		{
			if(toplananlar.Count > 0)
			{
				StartCoroutine(ToplananlariGonder(toplananlar[0]));
			}
			else
			{
				SetPlayerAvalibility();
			}		
		}
    }


	//public void ToplananlariGonder(GameObject obj)
	//{
	//	if (PlayerControllerCold.instance.isAvailable)
	//	{
	//		obj.transform.DOMove(PlayerControllerHot.instance.transform.position, .3f).OnComplete(() =>
	//		{
	//			toplananlar.Remove(obj);
	//			if (toplananlar.Count > 0) ToplananlariGonder(toplananlar[0]);
	//			else SetPlayerAvalibility();
	//		});
	//	}
	//	else
	//	{
	//		obj.transform.DOMove(PlayerControllerCold.instance.transform.position, .3f).OnComplete(() =>
	//		{
	//			toplananlar.Remove(obj);
	//			if (toplananlar.Count > 0) ToplananlariGonder(toplananlar[0]);
	//			else SetPlayerAvalibility();
	//		});
	//	}
	//}

	public IEnumerator ToplananlariGonder(GameObject obj)
	{

		if (PlayerControllerCold.instance.isAvailable)
		{
			StartCoroutine(ParabolikHareket(obj, PlayerControllerCold.instance.gameObject, PlayerControllerHot.instance.gameObject));
			toplananlar.Remove(obj);
			yield return new WaitForSeconds(.2f);
			if (toplananlar.Count > 0) StartCoroutine(ToplananlariGonder(toplananlar[0]));
			else SetPlayerAvalibility();
		}
		else
		{
			StartCoroutine(ParabolikHareket(obj, PlayerControllerHot.instance.gameObject, PlayerControllerCold.instance.gameObject));
			toplananlar.Remove(obj);
			yield return new WaitForSeconds(.2f);
			if (toplananlar.Count > 0) StartCoroutine(ToplananlariGonder(toplananlar[0]));
			else SetPlayerAvalibility();
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
			yield return new WaitForSeconds(.02f);
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
			//PlayerControllerHot.instance.HotToCold();
		}
	}

}
