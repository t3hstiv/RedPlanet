using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CommandUnit : MonoBehaviour {
    public Canvas commandMenu;
    public Button move;
    public Button attack;
    public Button end;
    public Button capture;
    public GameObject unit;

    void Start()
    {
        //commandMenu = commandMenu.GetComponent<Canvas>();
       // move = move.GetComponent<Button>();
        //attack = attack.GetComponent<Button>();
        //capture = capture.GetComponent<Button>();
        commandMenu.enabled = false;
    }

    public void MovePress()
    {
        //Highlight all of the possible hexes the unit can move to--depending on its movement value.
        unit.SendMessage("Move");
        // unit.transform.position += new Vector3(0, 1, 0);
    }

    public void AttackPress()
    {
        unit.SendMessage("Attack");        
    }

    public void EndPress()
    {
        unit.SendMessage("End");
    }
    
    /*public void CapturePress()
    {
       
       
    }*/

    void Update()
    {

    }

}
