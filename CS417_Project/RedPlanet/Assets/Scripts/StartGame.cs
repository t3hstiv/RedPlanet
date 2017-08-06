using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    public Canvas startMenu;
    public Canvas quitMenu;
    public Button yes;
    public Button no;
    public Button start;
    public Button quit;

	// Use this for initialization
	void Start () {
        startMenu = startMenu.GetComponent<Canvas>();
        quitMenu = quitMenu.GetComponent<Canvas>();
        start = start.GetComponent<Button>();
        quit = quit.GetComponent<Button>();
        yes = yes.GetComponent<Button>();
        no = no.GetComponent<Button>();
        startMenu.enabled = true;
        quitMenu.enabled = false;
	}
	
    public void StartPress()
    {
        startMenu.enabled = false;
        //Application.LoadLevel(1);
        SceneManager.LoadScene(1);
    }

    public void QuitPress()
    {
        quitMenu.enabled = true;
        startMenu.enabled = false;
    }

    public void YesPress()
    {
        quitMenu.enabled = false;
        Application.Quit();
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        startMenu.enabled = true;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
