using UnityEngine;
using System.Collections;

/*
 * Just a simple player contoller script
 * for the test scene.
 * */
namespace AI
{

	public class PlayerController : MonoBehaviour
	{

		public float maxSpeed = 3f;
		public float jumpForce = 400f;
		[HideInInspector]
		public bool facingRight = true;

		private bool grounded = false;
		public Transform groundCheck;
		public LayerMask whatIsGround;


		// Use this for initialization
		void Start()
		{
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);


			float move = Input.GetAxis("Horizontal");
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

			if (move > 0 && !facingRight)
				Flip();
			else if (move < 0 && facingRight)
				Flip();
		}

		void Update()
		{
			if (grounded && Input.GetButtonDown("Jump"))
			{
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
			}
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();

		}

		void Flip()
		{
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		void OnCollisionEnter2D(Collision2D coll)
		{
			if (coll.gameObject.tag == "Enemy")
				Application.LoadLevel("SampleScene");
		}
	}
}
