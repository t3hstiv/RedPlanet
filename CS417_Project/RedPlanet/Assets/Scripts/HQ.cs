using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HQ : MonoBehaviour {
    private Rigidbody rb;
    //private HexCoordinates[] openPositions;
    public float healthPoints = 300f;
    public Canvas createMenu;
    public bool selected = false;
    public Material initialMaterial;
    public Material highlightMaterial, attackMaterial, yellowHighlight;
    public MeshRenderer hqMaterial;
    public int hqAttack = 10;
    public int xcoord, zcoord;

    public Vector3 location = Vector3.zero;
    //public HexGrid theGrid;
    public PlayGame play;
    public int id;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialMaterial = hqMaterial.material;
        //createMenu = createMenu.GetComponent<Canvas>();
        //theGrid = GetComponent<HexGrid>();
        //openPositions gets initialized here.--------------------------------
    }

    // Update is called once per frame. This will be used when the tank is moved.
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("player is: " + play.playerTurn + " and hq id is: " + id);
        //Make sure player1 can't click on player2's hq and vice-versa.
        if(play.playerTurn == 1 && id == 1)
            if (selected)
            {
                createMenu.enabled = false;
                selected = false;
                hqMaterial.material = initialMaterial;
                //if there are any highlighted tiles around the base and the player backed out of placing the unit, dehighlight them.
                play.player1.DehighlightHQTiles(1);
                play.player1.wantTroop = false;
                play.player1.wantTank = false;
            }
            else
            {
                if (play.player1.selectedTank == -1 && play.player1.selectedTroop == -1)
                {
                    createMenu.enabled = true;
                    selected = true;
                    hqMaterial.material = highlightMaterial;
                    //Assign the CommandMenu unit to "this" Tank.
                    //createMenu.GetComponent<CreateUnit>().hqSelected = rb.gameObject;
                }
            }
        if (play.playerTurn == 2 && id == 2)
            if (selected)
            {
                createMenu.enabled = false;
                selected = false;
                hqMaterial.material = initialMaterial;
                //if there are any highlighted tiles around the base and the player backed out of placing the unit, dehighlight them.
                play.player2.DehighlightHQTiles(2);
                play.player2.wantTroop = false;
                play.player2.wantTank = false;
            }
            else
            {
                if (play.player2.selectedTank == -1 && play.player2.selectedTroop == -1)
                {
                    createMenu.enabled = true;
                    selected = true;
                    hqMaterial.material = highlightMaterial;
                    //Assign the CommandMenu unit to "this" Tank.
                    //createMenu.GetComponent<CreateUnit>().hqSelected = rb.gameObject;
                }
            }

    }

    /*public void CreateTank()
    {
        Debug.Log("Tank Button pressed");
    }

    public void CreateTroops()
    {
        Debug.Log("Tank Button pressed");
    }*/


    /*
    void isClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Tank")
                {
                    Debug.Log(hit.collider.name);
                    //Vector3 pointClicked = hit.point;
                    // do whatever you want with the newly selected
                    // object
                    //hit.transform.gameObject.transform.position += new Vector3(0, 1, 0);

                    //Display the CommandMenu
                    displayCommandMenu();

                }
            }
        }
    }

    */
}

