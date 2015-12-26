using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {
	public float SpeedX = 5f;
	public float SpeedY = 2f;

	void Start() {
	}

	void FixedUpdate() {
		if(isLocalPlayer) {
			float moveHoriz = Input.GetAxis ("Horizontal");
			float moveVert = Input.GetAxis ("Vertical");
			GetComponent<Rigidbody2D>().velocity = new Vector2(SpeedX * moveHoriz, SpeedY * moveVert);
		}
	}
	
	void OnCollisionEnter2D(Collision2D col) {
//		if(col.gameObject.tag == "Finish") {
//			Application.LoadLevel("level2");
//		}
	}
}
