using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections; 
using System.Collections.Generic; 
using System;

public class Countdown : NetworkBehaviour {
    public int countdownStart = 120;
	public GameObject winner;
	
    private int timeLeft;
	private int timeStarted; //This variable will overflow eventually
    private bool restart;
	private bool started = false;

	[SyncVar]
	private string currDisplayTime = "02:00";
	[SyncVar]
	private string gameWinner = "";

    void Start() {
		winner.GetComponent<Text>().text = "";
        restart = true;
        timeLeft = countdownStart;
    }

	public bool gameOver(){
		return (int)Time.timeScale == 0;
	}

    void Update () {
		if(isServer && NetworkManager.singleton.numPlayers > 1) {
			if(!started) {
				timeStarted = (int)Time.timeSinceLevelLoad;
				started = true;
			}
	        if(countdownStart + timeStarted - (int)Time.timeSinceLevelLoad >= 0) {
	            timeLeft = countdownStart - (int)Time.timeSinceLevelLoad + timeStarted;
	    		int minutesLeft = timeLeft / 60;
	    		int secondsLeft = timeLeft % 60;
	    		currDisplayTime = minutesLeft.ToString ("00") + ":" + secondsLeft.ToString ("00");
	        }
	        else {
	            if(restart) {
					SetWinner();
	                Invoke("ReloadLevel", 10f);
	                restart = false;
	            }
	        }
		}
		if(isClient) {
			GetComponent<Text>().text = currDisplayTime;
			winner.GetComponent<Text>().text = gameWinner;
		}
	}

	private void SetWinner() {
		gameWinner = GetWinningScore();
	}

	private String GetWinningScore() {
		MyNetworkManager networkManager = NetworkManager.singleton as MyNetworkManager;
		List<GameObject> players = networkManager.GetPlayers();
		int topScore = players[0].GetComponent<Player>().GetHits();
		int topPlayer = 1;
		for(int i = 0; i < players.Count; i++) {
			if(players[i].GetComponent<Player>().GetHits() < topScore) {
				topScore = players[i].GetComponent<Player>().GetHits();
				topPlayer = i + 1;
			}
		}
		return "Player " + topPlayer + " Wins!";
	}

	void ReloadLevel() {
		timeStarted = (int)Time.timeSinceLevelLoad;
		MyNetworkManager networkManager = NetworkManager.singleton as MyNetworkManager;
		List<GameObject> players = networkManager.GetPlayers();
		foreach(GameObject player in players) {
			player.GetComponent<Player>().RpcReset();
		}
		foreach(GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle")) {
			Destroy(obstacle);
			NetworkServer.Destroy(obstacle);
		}
		gameWinner = "";
		restart = true;
    }

    public float GetPercentMatchDone() {
        return 100.0f - ((float)timeLeft / countdownStart * 100.0f);
    }
}
