using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public Button startGame;
    public Text startText;
    public Text loadingText;
    public Text tipsText;
    public RawImage BLLogo;
    public RawImage ZLogo;

	// Use this for initialization
	void Start () {
        startGame = startGame.GetComponent<Button>();
        startText = startText.GetComponent<Text>();
        loadingText = loadingText.GetComponent<Text>();
        tipsText = tipsText.GetComponent<Text>();
        BLLogo = BLLogo.GetComponent<RawImage>();
        ZLogo = ZLogo.GetComponent<RawImage>();

        startText.enabled = true;
        loadingText.enabled = false;
        tipsText.enabled = false;
        BLLogo.enabled = true;
        ZLogo.enabled = false;
	}
	
    public void interact() {
        startText.enabled = false;
        loadingText.enabled = true;
        BLLogo.enabled = false;
        ZLogo.enabled = true;
        tipsText.enabled = true;

        StartCoroutine(TimerThingy());
    }

    IEnumerator TimerThingy() {
        //yield return new WaitForSeconds(4);
        AsyncOperation async = SceneManager.LoadSceneAsync("CAMP_INT_Initiation");
        yield return async;
        Debug.Log("Loading done");
    }

}
