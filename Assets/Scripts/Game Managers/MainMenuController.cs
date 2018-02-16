using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	[SerializeField]
	private Button musicButton;

	[SerializeField]
	private Sprite soundOff, soundOn;

	void Start () {
		if (GameManager.instance.canPlayMusic) {
			musicButton.image.sprite = soundOff;
		} else {
			musicButton.image.sprite = soundOn;
		}
	}

	public void PlayGame () {
		GameManager.instance.gameStartedFromMainMenu = true;
		SceneManager.LoadScene (Tags.GAMEPLAY_SCENE);
	}

	public void ToggleMusic () {
		if (GameManager.instance.canPlayMusic) {
			musicButton.image.sprite = soundOn;
			GameManager.instance.canPlayMusic = false;

		} else {
			musicButton.image.sprite = soundOff;
			GameManager.instance.canPlayMusic = true;
		}
	}

} // MainMenuController