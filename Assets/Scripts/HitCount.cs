using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitCount : MonoBehaviour {
	public void SetCount(int count) {
		GetComponent<Text>().text = count.ToString();
	}
}
