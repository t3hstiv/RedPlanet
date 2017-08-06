using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndTurn : MonoBehaviour {
    public PlayGame pg;
    public Text playerName;
    private bool clicked;
    public Canvas endTurn;
    public Canvas confirmTurn;
    public Button yes;
    public Button no;
    public Button end;
    // Use this for initialization
    void Start () {
        endTurn.enabled = true;
        confirmTurn.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void EndTurnPress()
    {
        endTurn.enabled = false;
        confirmTurn.enabled = true;
        //Debug.Log("Player1 turn ending.");
        Debug.Log("Current turn is " + pg.playerTurn);

    }

    public void YesPress()  
    {
        if (pg.playerTurn == 1)
        {
            Debug.Log("Earth turn ending.");
            pg.player1.turn = false;
            pg.player2.turn = true;
            pg.playerTurn = 2;
            pg.player2.SendMessage("BeginTurn");//SB
            //p1.allMoved = false;   
            //p2.allMoved = false;
            pg.player1.bioFuel += pg.player1.turnBonusFuel;
            pg.player2.SendMessage("DisplayName");
            //pg.player1.playerName.text = pg.player2.nameP;
            //end.enabled = false;

            //end.OnSelect
            //re-enable all of the move options for each players' units
        }
        else
        {
            Debug.Log("Mars turn ending.");
            pg.player2.turn = false;
            pg.player1.turn = true;
            pg.playerTurn = 1;
            pg.player1.SendMessage("BeginTurn");//SB
            pg.player2.bioFuel += pg.player2.turnBonusFuel;
            pg.player1.SendMessage("DisplayName");
            //end.enabled = false;
            //re-enable all of the move options for each players' units
        }
        endTurn.enabled = true;
        confirmTurn.enabled = false;
        Debug.Log("Current turn is " + pg.playerTurn);

    }

    public void NoPress()
    {
        confirmTurn.enabled = false;
        endTurn.enabled = true;
        Debug.Log("Current turn is " + pg.playerTurn);

    }
}
