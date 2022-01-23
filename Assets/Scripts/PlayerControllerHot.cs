using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerHot : MonoBehaviour
{
	public static PlayerControllerHot instance;
	[SerializeField] LayerMask groundLayers;
	private float gravity = -30f;
	private CharacterController characterController;
	private Vector3 velocity;
	private bool isGrounded;
	private float horizontalInput;
	private float speed = 7;
	private float jumpHeight = 2;
	public bool isAvailable;
	public Slider slider;
	public int toplanan;
	public GameObject playerModel;
	public Animator playerAnim;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		isGrounded = Physics.CheckSphere(transform.position, .2f, groundLayers, QueryTriggerInteraction.Ignore);
		if (isAvailable)
		{
			horizontalInput = Input.GetAxis("Horizontal"); ;

			// face forward
			playerModel.transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);		
			if (horizontalInput == 0)
			{
				playerAnim.ResetTrigger("run");
				playerAnim.SetTrigger("idle");

			}
			else
			{
				playerAnim.ResetTrigger("idle");
				playerAnim.SetTrigger("run");
			}

			characterController.Move(new Vector3(horizontalInput * speed, 0, 0) * Time.deltaTime);

			if (isGrounded )
			{
				if(Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Joystick1Button0))
					velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
			}

			
		}
		if (isGrounded && velocity.y < 0)
		{
			velocity.y = 0;
		}
		else
		{
			velocity.y += gravity * Time.deltaTime;
		}
		characterController.Move(velocity * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("a5") && isAvailable)
		{
			toplanan += 5;
			UiController.instance.SetEldeki1Text();
			GameManager.instance.toplananlar.Add(other.gameObject);
			other.GetComponent<Collider>().enabled = false;
			other.transform.parent = transform;
			other.transform.localPosition = new Vector3(0,0.66f,-.2f);
			other.transform.GetChild(0).gameObject.SetActive(false);
			other.transform.GetChild(1).gameObject.SetActive(false);
		}
		else if (other.CompareTag("a10") && isAvailable)
		{
			toplanan += 10;
			UiController.instance.SetEldeki1Text();
			GameManager.instance.toplananlar.Add(other.gameObject);
			other.GetComponent<Collider>().enabled = false;
			other.transform.parent = transform;
			other.transform.localPosition = new Vector3(0, 0.66f, -.2f);
			other.transform.GetChild(0).gameObject.SetActive(false);
			other.transform.GetChild(1).gameObject.SetActive(false);
		}
		else if (other.CompareTag("a20") && isAvailable)
		{
			toplanan += 20;
			UiController.instance.SetEldeki1Text();
			GameManager.instance.toplananlar.Add(other.gameObject);
			other.GetComponent<Collider>().enabled = false;
			other.transform.parent = transform;
			other.transform.localPosition = new Vector3(0, 0.66f, -.2f);
			other.transform.GetChild(0).gameObject.SetActive(false);
			other.transform.GetChild(1).gameObject.SetActive(false);
		}
		else if (other.CompareTag("b5"))
		{
			DecreaseSliderValue(5);
			Destroy(other.gameObject);
		}
		else if (other.CompareTag("b10"))
		{
			DecreaseSliderValue(10);
			Destroy(other.gameObject);
		}
		else if (other.CompareTag("b20"))
		{
			DecreaseSliderValue(20);
			Destroy(other.gameObject);
		}

	}

	public void DecreaseSliderValue(int gelen)
	{
		float value = (float)gelen / 100;
		if (value <= slider.value)
		{
			
			slider.value = slider.value - value;
		}
		else
		{
			slider.value = 0;
		}
		GameManager.instance.score += gelen;
		UiController.instance.SetGPScoreText();
		toplanan = 0;
	}

	public void PlayerHotStartingEvent()
	{
		isAvailable = true;
		StartCoroutine(SliderActive());
	}

	public IEnumerator SliderActive()
	{
		while (isAvailable)
		{
			slider.value +=  0.0018f;
			yield return new WaitForSeconds(0.035f);
			if (slider.value >= 1)
			{
				// Oyun sonu iþlemleri...
				GameManager.instance.GameOver();
				isAvailable = false;
				yield return new WaitForSeconds(3);
				GameManager.instance.isEndPanel = true;
				GameManager.instance.isContinue = false;
			}
		}
	}

}
