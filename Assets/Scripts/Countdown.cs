using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections; 
using System;

public class Countdown : NetworkBehaviour {
    public int countdownStart = 120;
    public ScoreController scoreController;
	
    private int timeLeft;
	private int timeStarted;
    private bool restart;
	private bool started = false;

	[SyncVar]
	private string currDisplayTime = "02:00";

    void Start() {
        restart = true;
        timeLeft = countdownStart;
    }

	public bool gameOver(){
		return (int)Time.timeScale == 0;
	}

    void Update () {
		if(isServer && NetworkServer.connections.Count > 1) {
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
	                //scoreController.DisplayWinner();
	                Invoke("ReloadLevel", 10f);
	                restart = false;
	            }
	        }
		}
		GetComponent<Text>().text = currDisplayTime;
	}

	void ReloadLevel() {
		NetworkManager.singleton.ServerChangeScene("Online");
    }

    public float GetPercentMatchDone() {
        return 100.0f - ((float)timeLeft / countdownStart * 100.0f);
    }
}
