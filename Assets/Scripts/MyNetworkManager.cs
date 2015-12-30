using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MyNetworkManager : NetworkManager
{
	private List<GameObject> players = new List<GameObject>();

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		player.GetComponent<Player>().Number = numPlayers + 1;
		players.Add(player);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public List<GameObject> GetPlayers() {
		return players;
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player) {
		base.OnServerRemovePlayer(conn, player);
		players.Remove(player.gameObject);
	}

	public override void OnServerSceneChanged(string name) {
		base.OnServerSceneChanged(name);
		players.Clear();
	}
}

