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
		GetComponentInChildren<HitCount>().SetCount(hits);
	}

	void FixedUpdate() {
		if(isLocalPlayer) {
			float moveHoriz = Input.GetAxis ("Horizontal");
			float moveVert = Input.GetAxis ("Vertical");
			GetComponent<Rigidbody2D>().velocity = new Vector2(SpeedX * moveHoriz, SpeedY * moveVert);
		}
	}

	[Command]
	void CmdHit() {
		Debug.Log("Player " + Number + " hit. Hits: " + hits);
		hits++;
		GetComponentInChildren<HitCount>().SetCount(hits);
		AudioSource[] audioSources = GetComponents<AudioSource>();
		audioSources[Random.Range(0, audioSources.Length)].Play();
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if(col.gameObject.tag == "Obstacle" && isLocalPlayer) {
			CmdHit();
		}
	}

	public int GetHits() {
		return hits;
	}
	
	public void Reset() {
		GetComponent<Rigidbody2D>().velocity  = Vector2.zero;
		GetComponent<Rigidbody2D>().angularVelocity  = 0;
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		hits = 0;
		GetComponentInChildren<HitCount>().SetCount(hits);
	}
}
