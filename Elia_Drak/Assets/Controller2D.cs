using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour
{
	private bool isGrounded = true;
	[SerializeField]
	private float moveSpeed, jumpHeight, gravity, weight;
	private Rigidbody2D rb2d;
	[SerializeField]
	private bool freezRotation;
	private float moveVelocity;



	public bool IsStopped { get; private set; }
	public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
	public bool FreezRotation { get => freezRotation; set => rb2d.freezeRotation = freezRotation = value; }
	public float Gravity { get => gravity; set => rb2d.gravityScale = gravity = value; }
	public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
	public float Weight { get => weight; set => rb2d.mass = weight = value; }

	[SerializeField]
	[Header("Input")]
	private List<KeyCode> moveRightKeys, moveLeftKeys, jumpKeys;



	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.freezeRotation = freezRotation;
		rb2d.gravityScale = gravity;
		rb2d.mass = weight;

	}

	private void Update()
	{
		GetInput();
		if (Input.GetMouseButton(0)&&!isGrounded)
		{
			rb2d.gravityScale = gravity / 2;
			rb2d.mass = weight / 2;

		}
		else
		{
			rb2d.gravityScale = gravity;
			rb2d.mass = weight;

		}

	}
	private void FixedUpdate()
	{
		Move();
	}
	private void Move()
	{
		rb2d.velocity = new Vector2(moveVelocity, rb2d.velocity.y);
	}

	private void GetInput()
	{

		moveVelocity = 0;
		isGrounded = rb2d.velocity.y == 0;
		if (IsStopped) return;

		if (isGrounded && (Input.GetAxis("Jump") > 0 || (jumpKeys.Count > 0 && jumpKeys.Any(k => k != KeyCode.None && Input.GetKey(k)))))
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, JumpHeight);
			isGrounded = false;
		}
		if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
		{
			moveVelocity = Input.GetAxis("Horizontal") * moveSpeed;
		}
		else
		{
			if (moveLeftKeys.Any(k => Input.GetKey(k)))
			{
				moveVelocity = -MoveSpeed;
				GetComponent<SpriteRenderer>().flipX = true;
			}
			else if (moveRightKeys.Any(k => Input.GetKey(k)))
			{
				moveVelocity = MoveSpeed;
				GetComponent<SpriteRenderer>().flipX = false;

			}
			if (jumpKeys.Any(k => Input.GetKey(k)))
			{
				if (isGrounded)
				{
					rb2d.velocity = new Vector2(rb2d.velocity.x, JumpHeight);
					isGrounded = false;
				}
			}
		}
		
	}
	/// <summary>
	/// locks movement
	/// </summary>
	public void Stop()
	{
		IsStopped = true;
		rb2d.velocity = Vector2.zero;
		moveVelocity = 0;
	}
	/// <summary>
	/// unlocks movement
	/// </summary>
	public void Restore()
	{
		IsStopped = false;
	}
}
