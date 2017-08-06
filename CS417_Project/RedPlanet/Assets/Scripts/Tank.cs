using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {
    private Rigidbody rb;
    //private HexCoordinates position;
    public float healthPoints = 100f;
    public CommandUnit commandUnit;
    public Canvas commandMenu;
    public Canvas confirmAttack;
    public Particle explosion;
    //public Canvas createMenu;
    public bool selected = false;
    public Material initialMaterial;
    public int movementTank = 3;
    public float[] attack = { 0, 0 };
    public Material highlightMaterial, attackMaterial, yellowHighlight; //attackMaterial is used for attackable units and highlight material is for a player's selected units
    public MeshRenderer tankMaterial;
    //public Vector3 location;
    public bool moved = false;
    //public Player p1;
    //public Player p2;
    public PlayGame pgame;
    public int tag;
    public int xcoord;//SB
    public int zcoord;//SB
    public int index;
    //public int tanknumber;SB


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        initialMaterial  = tankMaterial.material;
        GameObject pGameObj = GameObject.FindGameObjectWithTag("PlayGameObj");
        pgame = pGameObj.GetComponent<PlayGame>();
        GameObject commandMenuObj = GameObject.FindGameObjectWithTag("CommandUnitCanvas");
        commandMenu = commandMenuObj.GetComponent<Canvas>();
        commandUnit = commandMenu.GetComponent<CommandUnit>();
    }

    // Update is called once per frame. This will be used when the tank is moved.
    void Update () {
       // isClicked();
	}

    private void OnMouseDown()
    {
        Debug.Log("Player " + tag);
        if (!moved)
        {
            if (tag == 1 && pgame.player1.turn)
            {
                if (selected)
                {
                    commandMenu.enabled = false;
                    selected = false;
                    tankMaterial.material = initialMaterial;
                    pgame.player1.selectedTank = -1;
                    foreach (int t in pgame.player1.attackableHQ)
                    {
                        pgame.player2.hq.hqMaterial.material = pgame.player2.hq.initialMaterial;
                        //Dehighlight the hex
                        //pgame.gameGrid.tiles[17, 17].DehighlightTile();
                        pgame.gameGrid.tiles[5, 5].DehighlightTile();
                    }
                    foreach (int t in pgame.player1.attackableTanks)
                    {
                        pgame.player2.tanks[t].tankMaterial.material = pgame.player2.tanks[t].initialMaterial;
                        //Dehighlight the hex
                        pgame.gameGrid.tiles[pgame.player2.tanks[t].xcoord, pgame.player2.tanks[t].zcoord].DehighlightTile();
                    }
                    foreach (int t in pgame.player1.attackableTroops)
                    {
                        pgame.player2.troops[t].troopMaterial.material = pgame.player2.troops[t].initialMaterial;
                        //Dehighlight the hex
                        pgame.gameGrid.tiles[pgame.player2.troops[t].xcoord, pgame.player2.troops[t].zcoord].DehighlightTile();
                    }
                    pgame.player1.attackableTanks.Clear();
                    pgame.player1.attackableTroops.Clear();
                    pgame.player1.attackableHQ.Clear();
                }
                else
                {
                    //make sure we can't select more than one tank at a time.
                    if ((index == pgame.player1.selectedTank || pgame.player1.selectedTank == -1) && pgame.player1.selectedTroop == -1 && !pgame.player1.hq.selected)
                    {
                        if (pgame.player1.CheckForEnemyUnits(xcoord, zcoord))   //check to see if the Attack button should be activated or not to allow for a proximity attack before moving.
                        {
                            commandMenu.enabled = true;   //enable the move and the attack options.
                            commandUnit.attack.interactable = true;
                            commandUnit.move.interactable = true;
                            commandUnit.end.interactable = true;
                            Debug.Log("Found enemies found near troop at Earth!");
                        }
                        else
                        {
                            Debug.Log("No enemies found near troop at Earth!");
                            commandMenu.enabled = true;
                            commandUnit.attack.interactable = false;
                            commandUnit.move.interactable = true;
                            commandUnit.end.interactable = true;
                        }
                        selected = true;
                        tankMaterial.material = highlightMaterial;
                        //Assign the CommandMenu unit to "this" Tank.
                        commandUnit.unit = rb.gameObject;
                        Debug.Log("Set command unit to a tank.");

                        pgame.player1.selectedTank = index - 1;
                    }
                }
            }
            if (tag == 2 && pgame.player2.turn)
            {
                if (selected)
                {
                    commandMenu.enabled = false;
                    selected = false;
                    tankMaterial.material = initialMaterial;
                    pgame.player2.selectedTank = -1;
                    foreach (int t in pgame.player2.attackableHQ)
                    {
                        pgame.player1.hq.hqMaterial.material = pgame.player1.hq.initialMaterial;
                        //Dehighlight the hex
                        pgame.gameGrid.tiles[2, 2].DehighlightTile();
                    }
                    foreach (int t in pgame.player2.attackableTanks)
                    {
                        pgame.player1.tanks[t].tankMaterial.material = pgame.player1.tanks[t].initialMaterial;  
                        pgame.gameGrid.tiles[pgame.player1.tanks[t].xcoord, pgame.player1.tanks[t].zcoord].DehighlightTile();
                    }
                    foreach (int t in pgame.player2.attackableTroops)
                    {
                        pgame.player1.troops[t].troopMaterial.material = pgame.player1.troops[t].initialMaterial;  //highlight the attackable tanks
                        pgame.gameGrid.tiles[pgame.player1.troops[t].xcoord, pgame.player1.troops[t].zcoord].DehighlightTile();
                    }
                    pgame.player2.attackableTanks.Clear();
                    pgame.player2.attackableTroops.Clear();
                    pgame.player2.attackableHQ.Clear();
                }
                else
                {
                    //make sure we can't select more than one tank at a time.
                    if ((index == pgame.player2.selectedTank || pgame.player2.selectedTank == -1)  && pgame.player2.selectedTroop == -1 && !pgame.player2.hq.selected)
                    {
                        if (pgame.player2.CheckForEnemyUnits(xcoord, zcoord))   //check to see if the Attack button should be activated or not to allow for a proximity attack before moving.
                        {
                            commandMenu.enabled = true;   //enable the move and the attack options.
                            commandUnit.attack.interactable = true;
                            commandUnit.move.interactable = true;
                            commandUnit.end.interactable = true;
                            Debug.Log("Found enemies found near troop at Earth!");
                        }
                        else
                        {
                            Debug.Log("No enemies found near troop at Earth!");
                            commandMenu.enabled = true;
                            commandUnit.attack.interactable = false;
                            commandUnit.move.interactable = true;
                            commandUnit.end.interactable = true;
                        }
                        //commandMenu.enabled = true;
                        selected = true;
                        tankMaterial.material = highlightMaterial;
                        //Assign the CommandMenu unit to "this" Tank.
                        commandUnit.unit = rb.gameObject;
                        pgame.player2.selectedTank = index - 1;
                    }
                }
            }
        }
    }
    //SB
    public void Move()
    {

        if (pgame.playerTurn == 1)
        {
            pgame.player1.HighlightTilesMovement(xcoord, zcoord, movementTank);
        }
        else
        {
            pgame.player2.HighlightTilesMovement(xcoord, zcoord, movementTank);
        }
        commandMenu.enabled = false;
    }

    public void Attack()
    {
        if (pgame.playerTurn == 1)
        {
            pgame.player1.HighlightTilesAttack();
            pgame.player1.attackMode = true;
        }
        else
        {
            pgame.player2.HighlightTilesAttack();
            pgame.player2.attackMode = true;
        }
       
        moved = true;
        commandUnit.attack.interactable = false;
    }

    public void End()
    {
        int savet = -1;
        if (pgame.playerTurn == 1)
        {
            pgame.player1.tanks[pgame.player1.selectedTank].selected = false;
            pgame.player1.tanks[pgame.player1.selectedTank].tankMaterial.material = pgame.player1.tanks[pgame.player1.selectedTank].initialMaterial;
            Debug.Log("End this units turn for player1.");
            foreach (int t in pgame.player1.attackableHQ)
            {
                pgame.player2.hq.hqMaterial.material = pgame.player2.hq.initialMaterial;
                //Dehighlight the hex
                pgame.gameGrid.tiles[5, 5].DehighlightTile();

                //pgame.gameGrid.tiles[17, 17].DehighlightTile();
            }
            //Separately dehighlight and set the initial material back.
            foreach (int t in pgame.player1.attackableTanks)
            {
                pgame.player2.tanks[t].tankMaterial.material = pgame.player2.tanks[t].initialMaterial;
                //Dehighlight the hex
                pgame.gameGrid.tiles[pgame.player2.tanks[t].xcoord, pgame.player2.tanks[t].zcoord].DehighlightTile();
            }
            foreach (int t in pgame.player1.attackableTanks)
            {
                if (pgame.player2.tanks[t].healthPoints <= 0)
                {
                    pgame.player2.tanks[t].gameObject.active = false;
                    pgame.gameGrid.tiles[pgame.player2.tanks[t].xcoord, pgame.player2.tanks[t].zcoord].obstacle = false;
                    pgame.player2.tanks.Remove(pgame.player2.tanks[t]);
                    pgame.player2.numTanks--;
                    savet = t;
                    break;
                }
            }
            if (savet != -1)
                for (int i = savet; i < pgame.player2.tanks.Count; i++)
                {
                    pgame.player2.tanks[i].index--;
                }
            savet = -1;
            foreach (int t in pgame.player1.attackableTroops)
            {
                pgame.player2.troops[t].troopMaterial.material = pgame.player2.troops[t].initialMaterial;
                //Dehighlight the hex
                pgame.gameGrid.tiles[pgame.player2.troops[t].xcoord, pgame.player2.troops[t].zcoord].DehighlightTile();
            }
            foreach (int t in pgame.player1.attackableTroops)
            {
                if (pgame.player2.troops[t].healthPoints <= 0)
                {
                    pgame.player2.troops[t].gameObject.active = false;
                    pgame.gameGrid.tiles[pgame.player2.troops[t].xcoord, pgame.player2.troops[t].zcoord].obstacle = false;
                    pgame.player2.troops.Remove(pgame.player2.troops[t]);
                    pgame.player2.numTroops--;
                    savet = t;
                    break;
                }
            }
            if (savet != -1)
                for (int i = savet; i < pgame.player2.troops.Count; i++)
                {
                    pgame.player2.troops[i].index--;
                }
            savet = -1;
            //Tank is destroyed.
            if (pgame.player1.tanks[pgame.player1.selectedTank].healthPoints <= 0)
            {
                pgame.player1.tanks[pgame.player1.selectedTank].gameObject.active = false;
                pgame.player1.tanks.Remove(pgame.player1.tanks[pgame.player1.selectedTank]);
                pgame.player1.numTanks--;
                savet = pgame.player1.selectedTank;
            }
            if (savet != -1)
                for (int i = savet; i < pgame.player1.tanks.Count; i++)
                {
                    pgame.player1.tanks[i].index--;
                }
            savet = -1;
            if (pgame.player2.hq.healthPoints <= 0)
            {
                pgame.player1.wins = true;
                pgame.player2.hq.gameObject.active = false;
            }
            pgame.player1.attackableTanks.Clear();
            pgame.player1.attackableTroops.Clear();
            pgame.player1.attackableHQ.Clear();
            pgame.player1.selectedTank = -1;
        }
        else
        {
            pgame.player2.tanks[pgame.player2.selectedTank].selected = false;
            pgame.player2.tanks[pgame.player2.selectedTank].tankMaterial.material = pgame.player2.tanks[pgame.player2.selectedTank].initialMaterial;
            Debug.Log("End this units turn for player2.");
            foreach (int t in pgame.player2.attackableHQ)
            {
                pgame.player1.hq.hqMaterial.material = pgame.player1.hq.initialMaterial;
                //Dehighlight the hex
                pgame.gameGrid.tiles[2, 2].DehighlightTile();
            }
            foreach (int t in pgame.player2.attackableTanks)
            {
                pgame.player1.tanks[t].tankMaterial.material = pgame.player1.tanks[t].initialMaterial;
                //Dehighlight the hex
                pgame.gameGrid.tiles[pgame.player1.tanks[t].xcoord, pgame.player1.tanks[t].zcoord].DehighlightTile();
            }
            foreach (int t in pgame.player2.attackableTanks)
            {
                if (pgame.player1.tanks[t].healthPoints <= 0)
                {
                    pgame.player1.tanks[t].gameObject.active = false;
                    pgame.gameGrid.tiles[pgame.player1.tanks[t].xcoord, pgame.player1.tanks[t].zcoord].obstacle = false;
                    pgame.player1.tanks.Remove(pgame.player1.tanks[t]);
                    pgame.player1.numTanks--;
                    savet = t;
                    break;
                }
            }
            if (savet != -1)
                for (int i = savet; i < pgame.player1.tanks.Count; i++)
                {
                    pgame.player1.tanks[i].index--;
                }
            savet = -1;
            foreach (int t in pgame.player2.attackableTroops)
            {
                pgame.player1.troops[t].troopMaterial.material = pgame.player1.troops[t].initialMaterial;
                //Dehighlight the hex
                pgame.gameGrid.tiles[pgame.player1.troops[t].xcoord, pgame.player1.troops[t].zcoord].DehighlightTile();
            }
            foreach (int t in pgame.player2.attackableTroops)
            {
                if (pgame.player1.troops[t].healthPoints <= 0)
                {
                    pgame.player1.troops[t].gameObject.active = false;
                    pgame.gameGrid.tiles[pgame.player1.troops[t].xcoord, pgame.player1.troops[t].zcoord].obstacle = false;
                    pgame.player1.troops.Remove(pgame.player1.troops[t]);
                    pgame.player1.numTroops--;
                    savet = t;
                    break;
                }
            }
            if (savet != -1)
                for (int i = savet; i < pgame.player1.troops.Count; i++)
                {
                    pgame.player1.troops[i].index--;
                }
            savet = -1;
            //Tank is destroyed.
            if (pgame.player2.tanks[pgame.player2.selectedTank].healthPoints <= 0)
            {
                pgame.player2.tanks[pgame.player2.selectedTank].gameObject.active = false;
                pgame.player2.tanks.Remove(pgame.player2.tanks[pgame.player2.selectedTank]);
                pgame.player2.numTanks--;
                savet = pgame.player2.selectedTank;
            }
            if (savet != -1)
                for (int i = savet; i < pgame.player2.tanks.Count; i++)
                {
                    pgame.player2.tanks[i].index--;
                }
            savet = -1;
            if (pgame.player1.hq.healthPoints <= 0)
            {
                pgame.player2.wins = true;
                pgame.player1.hq.gameObject.active = false;
            }
            pgame.player2.attackableTanks.Clear();
            pgame.player2.attackableTroops.Clear();
            pgame.player2.attackableHQ.Clear();
            pgame.player2.selectedTank = -1;
        }
        commandMenu.enabled = false;
    }
}
