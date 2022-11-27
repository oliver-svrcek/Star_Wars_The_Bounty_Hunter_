using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovementController : MonoBehaviour
{
	protected Animator Animator { get; set; }
	protected Rigidbody2D Rigidbody2D { get; set; }
	protected CapsuleCollider2D BodyCollider  { get; set; }
	protected LayerMask WhatIsGround  { get; set; }
	protected Vector3 velocity;
	protected bool AirControl  { get; set; }
	public bool IsGrounded { get; protected set; }
	protected float JumpForce  { get; set; }
	protected float MovementSmoothing  { get; set; }
	protected bool IsLookingRight { get; set; } = true;

	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;
	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	protected void Awake()
	{
		if ((Animator = this.gameObject.GetComponent<Animator>()) is null)
		{
			Debug.LogError(
				"ERROR: <CharacterMovementController> - " + this.gameObject.name +
				" game object is missing Animator component."
				);
			Application.Quit(1);
		}
		
		if ((Rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>()) is null)
		{
			Debug.LogError(
				"ERROR: <CharacterMovementController> - " + this.gameObject.name +
				" game object is missing Rigidbody2D component."
				);
			Application.Quit(1);
		}

		if ((BodyCollider = this.gameObject.GetComponent<CapsuleCollider2D>()) is null)
		{
			Debug.LogError(
				"ERROR: <CharacterMovementController> - " + this.gameObject.name +
				" game object is missing CapsuleCollider2D component."
				);
			Application.Quit(1);
		}
		
		WhatIsGround = LayerMask.GetMask("GameWorldSolid");
		velocity = Vector3.zero;

		AirControl = true;
		IsGrounded = false;
		JumpForce = 850f;
		MovementSmoothing = 0.01f;
		IsLookingRight = true;

		if (OnLandEvent == null)
		{
			OnLandEvent = new UnityEvent();	
		}
	}

	protected void FixedUpdate()
	{
		bool wasGrounded = IsGrounded;
		IsGrounded = false;

		Collider2D[] colliders = Physics2D.OverlapCapsuleAll(
			new Vector2(BodyCollider.bounds.center.x, BodyCollider.bounds.center.y - 0.1f), 
			new Vector2(BodyCollider.size.x - 0.4f, BodyCollider.size.y), 
			BodyCollider.direction, 0f, WhatIsGround
			);

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				IsGrounded = true;
				
				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
			}
		}
	}

	public void Move(float move, bool jump)
	{
		if (IsGrounded)
		{
			Animator.SetBool("IsJumping", false);
			Animator.SetBool("IsFalling", false);
			Animator.SetBool("IsGrounded", true);	
		}
		else
		{
			Animator.SetBool("IsGrounded", false);	
		}
		
		if (IsGrounded && move != 0f)
		{
			Animator.SetBool("IsWalking", true);	
		}
		else
		{
			Animator.SetBool("IsWalking", false);
		}
		
		if (!IsGrounded && Rigidbody2D.velocity.y < 0.001f)
		{
			Animator.SetBool("IsFalling", true);
		}
		else if (IsGrounded || Rigidbody2D.velocity.y >= 0.001f)
		{
			Animator.SetBool("IsFalling", false);
		}
		
		if (IsGrounded || AirControl)
		{

			Vector3 targetVelocity = new Vector2(move * 10f, Rigidbody2D.velocity.y);
			Rigidbody2D.velocity = Vector3.SmoothDamp(
				Rigidbody2D.velocity, targetVelocity, ref velocity, MovementSmoothing
			);

			if (move > 0 && !IsLookingRight)
			{
				Flip();
			}
			else if (move < 0 && IsLookingRight)
			{
				Flip();
			}
		}

		if (IsGrounded && jump)
		{
			IsGrounded = false;
			Animator.SetBool("IsJumping", true);
			Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0f);
			Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
		}
	}
	
	protected void Flip()
	{
		IsLookingRight = !IsLookingRight;
		transform.Rotate(0f, 180f, 0f);
	}
}
