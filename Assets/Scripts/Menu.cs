using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

    public Button startGame;
    public Text startText;
    public Text loadingText;

	// Use this for initialization
	void Start () {
        startGame = startGame.GetComponent<Button>();
        startText = startText.GetComponent<Text>();
        loadingText = loadingText.GetComponent<Text>();

        startText.enabled = true;
        loadingText.enabled = false;
	}
	
    public void interact() {
        startText.enabled = false;
        loadingText.enabled = true;

        StartCoroutine(TimerThingy());
    }

    IEnumerator TimerThingy() {
        yield return new WaitForSeconds(4);
        Application.LoadLevel("CAMP_INT_Initiation");
    }

}
