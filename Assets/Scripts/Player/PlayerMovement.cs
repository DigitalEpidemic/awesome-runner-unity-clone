using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 5f;
	public float jumpPower = 10f;
	public float secondJumpPower = 10f;
	public Transform groundCheckPosition;
	public float radius = 0.3f;
	public LayerMask layerGround;

	private Rigidbody myBody;
	private bool isGrounded;
	private bool playerJumped;
	private bool canDoubleJump;

	private PlayerAnimation playerAnim;

	public GameObject smokePosition;
	private bool gameStarted;

	private BGScroller bgScroller;

	private PlayerHealthDamageShoot playerShoot;

	void Awake () {
		myBody = GetComponent<Rigidbody> ();
		playerAnim = GetComponent<PlayerAnimation> ();
		bgScroller = GameObject.Find (Tags.BACKGROUND_GAME_OBJ).GetComponent<BGScroller> ();
		playerShoot = GetComponent<PlayerHealthDamageShoot> ();
	}

	void Start () {
		StartCoroutine (StartGame ());
	}

	// Is called every 4 frames
	void FixedUpdate () {
		if (gameStarted) {
			PlayerMove ();
			PlayerGrounded ();
			PlayerJump ();
		}
	}

	void PlayerMove () {
		myBody.velocity = new Vector3 (movementSpeed, myBody.velocity.y, 0f);
	}

	void PlayerGrounded () {
		isGrounded = Physics.OverlapSphere (groundCheckPosition.position, radius, layerGround).Length > 0;

		if (isGrounded && playerJumped) {
			playerJumped = false;
			playerAnim.DidLand ();
		}
	}

	void PlayerJump () {

		if (Input.GetKeyDown (KeyCode.Space) && !isGrounded && canDoubleJump) {
			canDoubleJump = false;
			myBody.AddForce (new Vector3 (0, secondJumpPower, 0));

		} else if (Input.GetKeyUp(KeyCode.Space) && isGrounded) {
			playerAnim.DidJump ();
			myBody.AddForce (new Vector3 (0, jumpPower, 0));
			playerJumped = true;
			canDoubleJump = true;
		}
	}

	IEnumerator StartGame () {
		yield return new WaitForSeconds (2f);
		gameStarted = true;
		bgScroller.canScroll = true;
		playerShoot.canShoot = true;
		GameplayController.instance.canCountScore = true;
		smokePosition.SetActive (true);
		playerAnim.PlayerRun ();
	}

} // PlayerMovement