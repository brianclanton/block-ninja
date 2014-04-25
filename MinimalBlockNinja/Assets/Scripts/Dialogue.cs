using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {

	public string nextScene;
	public string[] dialogueElements;

	private int currentIndex = 0;
	private string currentText;

	// Use this for initialization
	void Start () {
		currentText = dialogueElements[currentIndex++];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Advance Dialogue")) {
			if (IsDone())
				Application.LoadLevel(nextScene);
			else
				currentText = dialogueElements[currentIndex++];
		}
	}

	void OnGUI() {
		// Style stuff
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUI.skin.box.padding = new RectOffset(25, 25, 25, 25);
		GUI.skin.box.fontSize = 24;
		GUI.skin.box.wordWrap = true;

		// Actual dialog box
		GUI.Box(new Rect(475, Screen.height - 150, Screen.width - 950, 150), currentText);
		GUI.Label(new Rect(Screen.width - 475 - 175, Screen.height - 25, 200, 30), "Press Space to continue...");
	}

	public bool IsDone() {
		return currentIndex == dialogueElements.Length;
	}
}
