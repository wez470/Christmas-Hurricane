using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Obstacle : NetworkBehaviour {
	void Start() {
		if(isServer) {
			float speedX = Random.Range(-2f, 2f);
			float speedY = Random.Range(3f, 10f);
		    GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);
			GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-200f, 200f);
		}
	}

	void OnBecameInvisible() {
		if(isServer) {
			Destroy(this.gameObject);
		}
	}
}

