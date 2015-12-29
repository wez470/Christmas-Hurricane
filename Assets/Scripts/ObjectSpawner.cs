using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : NetworkBehaviour {
	public const float HALF_WIDTH = 12f;
	public const float HALF_HEIGHT = 10f;

	public List<GameObject> objects = new List<GameObject>();

	private float time = 0f;
	private float timeToNextSpawn = 3f;

	void Update () {
		if(isServer) {
			if(NetworkServer.connections.Count < 2) {
				time = Time.timeSinceLevelLoad;
			}
			if(Time.timeSinceLevelLoad - time > timeToNextSpawn) {
				time = Time.timeSinceLevelLoad;
				timeToNextSpawn = Random.Range(0f, 4f);
				
				float xPos = Random.Range(-HALF_WIDTH, HALF_WIDTH);
				Vector3 pos = new Vector3(xPos, -HALF_HEIGHT, 0);
				GameObject obstacle = Instantiate(objects[Random.Range(0, objects.Count)], pos, Quaternion.identity) as GameObject;
				NetworkServer.Spawn(obstacle);
			}
		}
	}
}
