using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Player : NetworkBehaviour {
	public float SpeedX = 5f;
	public float SpeedY = 2f;
	public int Number { get; set; }

	[SyncVar]
	private int hits = 0;

	void Start() {
	}

	void Update() {
		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			GetComponentInChildren<HitCount>().SetCount(player.GetComponent<Player>().GetHits());
		}
	}

	void FixedUpdate() {
		if(isLocalPlayer) {
			float moveHoriz = Input.GetAxis ("Horizontal");
			float moveVert = Input.GetAxis ("Vertical");
			GetComponent<Rigidbody2D>().velocity = new Vector2(SpeedX * moveHoriz, SpeedY * moveVert);
		}
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if(col.gameObject.tag == "Obstacle" && isServer) {
			hits++;
			GetComponentInChildren<HitCount>().SetCount(hits);
			AudioSource[] audioSources = GetComponents<AudioSource>();
			audioSources[Random.Range(0, audioSources.Length)].Play();
		}
	}

	public int GetHits() {
		return hits;
	}
}
