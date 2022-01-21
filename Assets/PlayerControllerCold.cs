using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerCold : MonoBehaviour
{
	public static PlayerControllerCold instance;
	[SerializeField] LayerMask groundLayers;
	private float gravity = -30f;
	private CharacterController characterController;
	private Vector3 velocity;
	private bool isGrounded;
	private float horizontalInput;
	private float speed = 5;
	private float jumpHeight = 2;
	public bool isAvailable;
	public Slider slider;
	public int toplanan;
	public GameObject playerHot;

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
		if (isAvailable)
		{
			horizontalInput = Input.GetAxis("Horizontal"); ;

			// face forward
			transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);


			isGrounded = Physics.CheckSphere(transform.position, .1f, groundLayers, QueryTriggerInteraction.Ignore);

			if (isGrounded && velocity.y < 0)
			{
				velocity.y = 0;
			}
			else
			{
				velocity.y += gravity * Time.deltaTime;
			}

			characterController.Move(new Vector3(horizontalInput * 5, 0, 0) * Time.deltaTime);

			if (isGrounded && Input.GetButtonDown("Jump"))
			{
				velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
			}

			characterController.Move(velocity * Time.deltaTime);
		}
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("b5"))
		{
			toplanan += 5;
			Destroy(other.gameObject);
		}
		else if (other.CompareTag("b10"))
		{
			toplanan += 10;
		}
		else if (other.CompareTag("b20"))
		{
			toplanan += 20;
		}

	}

	public IEnumerator SliderActive()
	{
		while (isAvailable)
		{
			slider.value = slider.value + 0.0018f;
			yield return new WaitForSeconds(0.035f);
		}
	}
}
