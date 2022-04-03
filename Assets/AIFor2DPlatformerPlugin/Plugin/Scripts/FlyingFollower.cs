using UnityEngine;
using System.Collections;

//Ez a kód tökéletes arra, hogy valami kis repülő cucc kövessen (isKinematic = true)
namespace AI
{
	public class FlyingFollower : MonoBehaviour
	{

		public Transform whoToFollow;
		public float velocity = 2f;
		public float distanceFromTargetX = 1.0f;
		public float distanceFromTargetY = 1.0f;

		public bool willItTurn = false;

		private Transform thisTransform;

		private bool facingRight = true;
		private float distance;

		void Start()
		{
			thisTransform = transform;
		}

		//You only have to change the "PlayerController" and the "facingRight" parts if it's turning.

		void LateUpdate()
		{

			float positionX;
			float positionY;

			if (willItTurn)
			{

				if (GameObject.Find(whoToFollow.name).GetComponent<PlayerController>().facingRight)
				{
					positionX = Mathf.Lerp(thisTransform.position.x, whoToFollow.position.x - distanceFromTargetX, Time.deltaTime * velocity);
				}
				else
				{
					positionX = Mathf.Lerp(thisTransform.position.x, whoToFollow.position.x + distanceFromTargetX, Time.deltaTime * velocity);
				}
				positionY = Mathf.Lerp(thisTransform.position.y, whoToFollow.position.y + distanceFromTargetY, Time.deltaTime * velocity);

				thisTransform.position = new Vector3(positionX, positionY, 0);

				if (GameObject.Find(whoToFollow.name).GetComponent<PlayerController>().facingRight == true && !facingRight)
				{
					Flip();
				}
				else if (GameObject.Find(whoToFollow.name).GetComponent<PlayerController>().facingRight == false && facingRight)
				{
					Flip();
				}
			}
			else if (!willItTurn)
			{

				positionX = Mathf.Lerp(thisTransform.position.x, whoToFollow.position.x - distanceFromTargetX, Time.deltaTime * velocity);
				positionY = Mathf.Lerp(thisTransform.position.y, whoToFollow.position.y + distanceFromTargetY, Time.deltaTime * velocity);
				thisTransform.position = new Vector3(positionX, positionY, 0);

			}
		}

		void Flip()
		{
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

	}
}