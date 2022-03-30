using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

	//bullet prefab
	public GameObject bullet;
	//the time between bullet creations
	public float shootingRate = 1f;
	//Speed of the bullet
	public Vector3 bulletSpeedV;

    private void FixedUpdate()
    {
		Shoot();
    }

	private void Shoot()
    {
		GameObject shotBullet;
		shotBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		//We multiply it with transform.localScale.x because we need to know where is it facing
		shotBullet.GetComponent<Rigidbody2D>().velocity = bulletSpeedV * transform.localScale.x;
	}
}
