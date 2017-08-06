using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateUnit : MonoBehaviour
{
    public Canvas createMenu;
    public Button troops;
    public Button tank;
    public PlayGame play;
    public Canvas confirm;
    public Button yes;
    public Button no;
    //public bool wantTank, wantTroop;
    //public GameObject hqSelected;
    //public Rigidbody tankUnit;
    //public Player p1;
    //public Player p2;
    //public GameObject newObj;
    //public Troop troopsUnit;
    //private HexCoordinates hex;
    //public GameObject


    void Start()
    {
        //createMenu = createMenu.GetComponent<Canvas>();
        //troops = troops.GetComponent<Button>();
       // tank = tank.GetComponent<Button>();
        //p1 = p1.GetComponent<Player>();
       // p2 = p2.GetComponent<Player>();
        createMenu.enabled = false;
        confirm.enabled = false;
        //wantTank = false; 
        //wantTroop = false;
    }

    public void TankPress()
    {
        //adjacentCell will call a method for the appropriate cell placement location for the new unit. For now, it will just place it close by.
        //Vector3 adjacentCell = current HexCell
        //Vector3 adjacentCell = new Vector3(40, 0, 40);
        //Rigidbody tank = Instantiate(tankUnit, p1.hq.location + adjacentCell, tankUnit.rotation) as Rigidbody;
        //Update the proper location hex for the tank by setting its HexCoordinates
        confirm.enabled = true;
        createMenu.enabled = false;
        if (play.playerTurn == 1)
            play.player1.wantTank = true;
        else
            play.player2.wantTank = true;
    }

    public void TroopPress()
    {
        //adjacentCell will call a method for the appropriate cell placement location for the new unit. For now, it will just place it close by.
        //Vector3 adjacentCell = current HexCell
        //Vector3 adjacentCell = new Vector3(40, 0, 40);
        //Rigidbody tank = Instantiate(tankUnit, p1.hq.location + adjacentCell, tankUnit.rotation) as Rigidbody;
        //Update the proper location hex for the tank by setting its HexCoordinates
        confirm.enabled = true;
        createMenu.enabled = false;
        if (play.playerTurn == 1)
            play.player1.wantTroop = true;
        else
            play.player2.wantTroop = true;
    }

    public void YesPress()
    {
        Player p1 = play.player1;
        Player p2 = play.player2;

        confirm.enabled = false;
        //Create the tank if the player has enough biofuel.
        if (play.player1.wantTank || play.player2.wantTank)
        {
            if (play.playerTurn == 1)
            {
                if (p1.bioFuel >= 1000)
                {
                    p1.SendMessage("CreateTank");
                }
                else
                {
                    Debug.Log("Player 1, not enough biofuel to create tank.");
                    createMenu.enabled = false;
                    //Start the process from scratch and dehighlight the HQ
                    play.player1.hq.selected = false;
                    play.player1.hq.hqMaterial.material = play.player1.hq.initialMaterial;
                    play.player1.wantTank = false;
                }
            }

            if (play.playerTurn == 2)
            {
                if (p2.bioFuel >= 1000)
                {
                    p2.SendMessage("CreateTank");
                }
                else
                {
                    Debug.Log("Player 2, not enough biofuel to create tank.");
                    createMenu.enabled = false;
                    //Start the process from scratch and dehighlight the HQ
                    play.player2.hq.selected = false;
                    play.player2.hq.hqMaterial.material = play.player2.hq.initialMaterial;
                    play.player2.wantTank = false;
                }
            }
            //wantTank = false;
        }

        if (play.player1.wantTroop || play.player2.wantTroop)
        {
            if (play.playerTurn == 1)
            {
                if (p1.bioFuel >= 500)
                {
                    p1.SendMessage("CreateTroop");
                }
                else
                {
                    Debug.Log("Player 1, not enough biofuel to create troop.");
                    createMenu.enabled = false;
                    play.player1.hq.selected = false;
                    play.player1.hq.hqMaterial.material = play.player1.hq.initialMaterial;
                    play.player1.wantTroop = false;
                }
            }

            if (play.playerTurn == 2)
            {
                if (p2.bioFuel >= 500)
                {
                    p2.SendMessage("CreateTroop");
                }
                else
                {
                    Debug.Log("Player 2, not enough biofuel to create troop.");
                    createMenu.enabled = false;
                    play.player2.hq.selected = false;
                    play.player2.hq.hqMaterial.material = play.player2.hq.initialMaterial;
                    play.player2.wantTroop = false;
                }
            }
            // wantTroop = false;
        }

    }

    //IT
    public void NoPress()  //They don't want to create a tank now.
    {
        createMenu.enabled = false;
        confirm.enabled = false;
        if (play.playerTurn == 1)
        {
            play.player1.wantTank = false;
            play.player1.wantTroop = false;
            play.player1.hq.hqMaterial.material = play.player1.hq.initialMaterial;
            play.player1.hq.selected = false;
        }
        else
        {
            play.player2.wantTank = false;
            play.player2.wantTroop = false;
            play.player2.hq.hqMaterial.material = play.player2.hq.initialMaterial;
            play.player2.hq.selected = false;
        }
    }

    void Update()
    {

    }
}