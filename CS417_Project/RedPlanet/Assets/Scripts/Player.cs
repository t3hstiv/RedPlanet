using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Generic;

public class Player : MonoBehaviour {
    public List<Tank> tanks;
    public List<int> attackableTanks;
    public List<int> attackableTroops;
    public List<int> attackableHQ;
    //public List<Tank> tanks;
    public int numTanks = 0;
    public int numUnits = 0;
    public int numTroops = 0;
    public bool allMoved = false;
    public HQ hq;
    public HexTile[] placementTiles = new HexTile[6];
    public HexTile[] movementTiles = new HexTile[100];
    public List<Troop> troops;
    public bool turn;

    public bool wins = false;
    private const int initialBioFuel = 6000;
    public int turnBonusFuel = 1000;
    public int bioFuel; 
    private GameObject selectedUnit;
    public Tank tankUnit;
    public Troop troopUnit;
    private Canvas displayPlayerName;
    public Text playerName;
    public PlayGame pgame;
    public HexGrid gameGrid;
    public string nameP;
    public int tankxcoord;
    public int tankzcoord;
    public int selectedTank, selectedTroop, selectedHQ;
    public Tank activeTankObject;
    public bool wantTank, wantTroop;
    public bool attackMode;

    //public string p_name;

    // Use this for initialization
    void Start () {
        bioFuel = initialBioFuel;
        playerName.enabled = true;
        tanks = new List<Tank>();
        troops = new List<Troop>();
        attackableTanks = new List<int>();
        attackableTroops = new List<int>();
        attackableHQ = new List<int>();

        selectedTank = -1;
        selectedTroop = -1;
        attackMode = false;
    }
	
    void DisplayName()
    {
        playerName.text = nameP;
    }


    public void SetName(string n)
    {
        nameP = n;
    }

	// Update is called once per frame
	void Update () {
        
    }

    public void MoveTank(int xc, int zc, int indexOfSelected)
    {
        Vector3 position = pgame.gameGrid.tiles[xc, zc].transform.position;
        position.y = 2;
        Debug.Log("tanks size is" + pgame.player1.tanks.Count);

        tanks[indexOfSelected].transform.position = position;
        tanks[indexOfSelected].moved = true;

        //Dehighlihght movement tiles here.
        HighlightTilesMovement(tanks[indexOfSelected].xcoord, tanks[indexOfSelected].zcoord, tanks[indexOfSelected].movementTank);
        tanks[indexOfSelected].xcoord = xc;
        tanks[indexOfSelected].zcoord = zc;
    }

    public void MoveTroop(int xc, int zc, int indexOfSelected)
    {
        Vector3 position = pgame.gameGrid.tiles[xc, zc].transform.position;
        position.y = 2;
        Debug.Log("troops size is" + pgame.player1.troops.Count);

        troops[indexOfSelected].transform.position = position;
        troops[indexOfSelected].moved = true;

        HighlightTilesMovement(troops[indexOfSelected].xcoord, troops[indexOfSelected].zcoord, troops[indexOfSelected].movementTroop);
        troops[indexOfSelected].xcoord = xc;
        troops[indexOfSelected].zcoord = zc;
    }

    void BeginTurn()
    {
        foreach(Tank tank in tanks)
        {
            tank.moved = false;
        }
        foreach (Troop troop in troops)
        {
            troop.moved = false;
        }
    }

    public void SetHQ(int pos)
    {
        Debug.Log(pos);
        Debug.Log(hq.name);
        Debug.Log(hq.location);
        Debug.Log(pgame.name);
        Debug.Log(pgame.gameGrid);
        Debug.Log(pgame.gameGrid.tiles[pos, pos].xcoord);
        Debug.Log(pgame.gameGrid.tiles[pos, pos].zcoord);
        hq.transform.position = pgame.gameGrid.tiles[pos, pos].transform.position;
        if (pos == 2)
            hq.id = 1;
        else
            hq.id = 2;

    }

    public void CreateTank()
    {
        if (pgame.playerTurn == 1)
        {
            HighlightHQTiles(1); 
        }
        else
        {
            HighlightHQTiles(2);
        }
    }

    public void CreateTroop()
    {
        if (pgame.playerTurn == 1)
        {
            HighlightHQTiles(1); 
        }
        else
        {
            HighlightHQTiles(2);
        }
    }

    public void SpawnTank(Vector3 position, int xcoord, int zcoord)
    {
        numTanks++;
        bioFuel -= 1000;
        Debug.Log(position);

        if (nameP == "Earth")
        {
            Tank tank = Instantiate(tankUnit, position, tankUnit.GetComponent<Rigidbody>().rotation) as Tank;
            tank.xcoord = xcoord;//SB
            tank.zcoord = zcoord;//SB
            tank.healthPoints = 100;
            tank.attack[0] = 40f;
            tank.attack[1] = 50f;
            tank.tag = 1;
            tank.index = tanks.Count + 1;  //index 1 will be tank1, index 2 will be tank2..........IT
            //pgame.player1.tanks.Add(tank);
            //Debug.Log("Count after adding");
            tank.moved = true;
            tanks.Add(tank);
            Debug.Log("SpawnTank Earth, tank count = " + tanks.Count + ", and tank index = " + tank.index);

            tank = null;

        }
        else
        {
            Tank tank = Instantiate(tankUnit, position, tankUnit.GetComponent<Rigidbody>().rotation) as Tank;
            tank.xcoord = xcoord;//SB
            tank.zcoord = zcoord;//SB
            tank.healthPoints = 80;
            tank.attack[0] = 30f;
            tank.attack[1] = 70f;
            tank.tag = 2;
            tank.index = tanks.Count + 1;  //index 1 will be tank1, index 2 will be tank2..........IT

            //pgame.player2.tanks.Add(tank);
            tank.moved = true;
            tanks.Add(tank);
            Debug.Log("SpawnTank Mars, tank count = " + tanks.Count + ", and tank index = " + tank.index);

            tank = null;
        }


        if (pgame.playerTurn == 1)
        {
            DehighlightHQTiles(1); //Place a EventListener on these hexes.
        }
        else
        {
            DehighlightHQTiles(2);
        }
        wantTank = false;
    }

    public void SpawnTroop(Vector3 position, int xcoord, int zcoord)
    {
        numTroops++;
        bioFuel -= 500;
        Debug.Log("inside spawn troop: " + position);

        if (pgame.playerTurn == 1)
        {
            Troop troop = Instantiate(troopUnit, position, troopUnit.GetComponent<Rigidbody>().rotation) as Troop;
            troop.xcoord = xcoord;//SB
            troop.zcoord = zcoord;//SB
            troop.healthPoints = 40;
            troop.attack[0] = 14f;
            troop.attack[1] = 19f;
            troop.tag = 1;
            troop.index = troops.Count + 1;  //index 1 will be tank1, index 2 will be tank2..........IT
            //pgame.player1.tanks.Add(tank);
            //Debug.Log("Count after adding");
            troop.moved = true;
            troops.Add(troop);
            Debug.Log("SpawnTroop Earth, troop count = " + troops.Count + ", and troop index = " + troop.index + " at: " + troop.transform.position);

            //troop = null;

        }
        else
        {
            //troopUnit.GetComponent<Rigidbody>().rotation
            Troop troop = Instantiate(troopUnit, position, troopUnit.GetComponent<Rigidbody>().rotation) as Troop;
            troop.xcoord = xcoord;//SB
            troop.zcoord = zcoord;//SB
            troop.healthPoints = 35;
            troop.attack[0] = 18f;
            troop.attack[1] = 26f;
            troop.tag = 2;
            troop.index = troops.Count + 1;  //index 1 will be tank1, index 2 will be tank2..........IT

            //pgame.player2.tanks.Add(tank);
            troop.moved = true;
            troops.Add(troop);
            Debug.Log("SpawnTroop Mars, troop count = " + troops.Count + ", and troop index = " + troop.index);

            //troop = null;
        }


        if (pgame.playerTurn == 1)
        {
            DehighlightHQTiles(1); //Place a EventListener on these hexes.
        }
        else
        {
            DehighlightHQTiles(2);
        }
        wantTroop = false;
    }


    public void HighlightTilesMovement(int xc, int zc, int i)
    {
        Debug.Log("Coords passed are: " + xc + " " + zc);
        //CheckHighlightable(xc, zc, 0, 0);
        //pgame.gameGrid.tiles[xc, zc].HighlightTile();
        Debug.Log("movement range is: " + i);
        if (zc % 2 == 0)
            switch (i)
            {
                case 1:
                    //first circle around 0, 0   6 hexes
                    CheckHighlightable(xc, zc, -1, 0);
                    CheckHighlightable(xc, zc, -1, 1);
                    CheckHighlightable(xc, zc, -1, -1);
                    CheckHighlightable(xc, zc, 0, 1);
                    CheckHighlightable(xc, zc, 1, 0);
                    CheckHighlightable(xc, zc, 0, -1);
                    break;
                case 2:
                    //first circle around 0, 0   6 hexes
                    CheckHighlightable(xc, zc, -1, 0);
                    CheckHighlightable(xc, zc, -1, 1);
                    CheckHighlightable(xc, zc, 0, 1);
                    CheckHighlightable(xc, zc, 1, 0);
                    CheckHighlightable(xc, zc, 0, -1);
                    CheckHighlightable(xc, zc, -1, -1);
                    //second circles around 0, 0   12 hexes
                    CheckHighlightable(xc, zc, -1, -2);
                    CheckHighlightable(xc, zc, -2, -1);
                    CheckHighlightable(xc, zc, -2, 0);
                    CheckHighlightable(xc, zc, -2, 1);
                    CheckHighlightable(xc, zc, -1, 2);
                    CheckHighlightable(xc, zc, 0, 2);
                    CheckHighlightable(xc, zc, 1, 2);
                    CheckHighlightable(xc, zc, 1, 1);
                    CheckHighlightable(xc, zc, 2, 0);
                    CheckHighlightable(xc, zc, 1, -1);
                    CheckHighlightable(xc, zc, 1, -2);
                    CheckHighlightable(xc, zc, 0, -2);
                    break;
                case 3:
                    //first circle around 0, 0   6 hexes
                    CheckHighlightable(xc, zc, -1, 0);
                    CheckHighlightable(xc, zc, -1, 1);
                    CheckHighlightable(xc, zc, 0, 1);
                    CheckHighlightable(xc, zc, 1, 0);
                    CheckHighlightable(xc, zc, 0, -1);
                    CheckHighlightable(xc, zc, -1, -1);
                    //second circles around 0, 0   12 hexes
                    CheckHighlightable(xc, zc, -1, -2);
                    CheckHighlightable(xc, zc, -2, -1);
                    CheckHighlightable(xc, zc, -2, 0);
                    CheckHighlightable(xc, zc, -2, 1);
                    CheckHighlightable(xc, zc, -1, 2);
                    CheckHighlightable(xc, zc, 0, 2);
                    CheckHighlightable(xc, zc, 1, 2);
                    CheckHighlightable(xc, zc, 1, 1);
                    CheckHighlightable(xc, zc, 2, 0);
                    CheckHighlightable(xc, zc, 1, -1);
                    CheckHighlightable(xc, zc, 1, -2);
                    CheckHighlightable(xc, zc, 0, -2);
                    //third circle around 0, 0  18 hexes
                    CheckHighlightable(xc, zc, -2, -3);
                    CheckHighlightable(xc, zc, -2, -2);
                    CheckHighlightable(xc, zc, -3, -1);
                    CheckHighlightable(xc, zc, -3, 0);
                    CheckHighlightable(xc, zc, -3, 1);
                    CheckHighlightable(xc, zc, -2, 2);
                    CheckHighlightable(xc, zc, -2, 3);
                    CheckHighlightable(xc, zc, -1, 3);
                    CheckHighlightable(xc, zc, 0, 3);
                    CheckHighlightable(xc, zc, 1, 3);
                    CheckHighlightable(xc, zc, 2, 2);
                    CheckHighlightable(xc, zc, 2, 1);
                    CheckHighlightable(xc, zc, 3, 0);
                    CheckHighlightable(xc, zc, 2, -1);
                    CheckHighlightable(xc, zc, 2, -2);
                    CheckHighlightable(xc, zc, 1, -3);
                    CheckHighlightable(xc, zc, 0, -3);
                    CheckHighlightable(xc, zc, -1, -3);
                    break;
            }
        else
            switch (i)
            {
                case 1:
                    //first circle around 0, 0   6 hexes
                    CheckHighlightable(xc, zc, -1, 0);
                    CheckHighlightable(xc, zc, 0, 1);
                    CheckHighlightable(xc, zc, 1, 1);
                    CheckHighlightable(xc, zc, 1, 0);
                    CheckHighlightable(xc, zc, 1, -1);
                    CheckHighlightable(xc, zc, 0, -1);
                    break;
                case 2:
                    //first circle around 0, 0   6 hexes
                    CheckHighlightable(xc, zc, -1, 0);
                    CheckHighlightable(xc, zc, 0, 1);
                    CheckHighlightable(xc, zc, 1, 1);
                    CheckHighlightable(xc, zc, 1, 0);
                    CheckHighlightable(xc, zc, 1, -1);
                    CheckHighlightable(xc, zc, 0, -1);
                    //second circles around 0, 0   12 hexes
                    CheckHighlightable(xc, zc, -1, -1);
                    CheckHighlightable(xc, zc, -2, 0);
                    CheckHighlightable(xc, zc, -1, 1);
                    CheckHighlightable(xc, zc, -1, 2);
                    CheckHighlightable(xc, zc, 0, 2);
                    CheckHighlightable(xc, zc, 1, 2);
                    CheckHighlightable(xc, zc, 2, 1);
                    CheckHighlightable(xc, zc, 2, 0);
                    CheckHighlightable(xc, zc, 2, -1);
                    CheckHighlightable(xc, zc, 1, -2);
                    CheckHighlightable(xc, zc, 0, -2);
                    CheckHighlightable(xc, zc, -1, -2);
                    break;
                case 3:
                    //first circle around 0, 0   6 hexes
                    CheckHighlightable(xc, zc, -1, 0);
                    CheckHighlightable(xc, zc, 0, 1);
                    CheckHighlightable(xc, zc, 1, 1);
                    CheckHighlightable(xc, zc, 1, 0);
                    CheckHighlightable(xc, zc, 1, -1);
                    CheckHighlightable(xc, zc, 0, -1);
                    //second circles around 0, 0   12 hexes
                    CheckHighlightable(xc, zc, -1, -1);
                    CheckHighlightable(xc, zc, -2, 0);
                    CheckHighlightable(xc, zc, -1, 1);
                    CheckHighlightable(xc, zc, -1, 2);
                    CheckHighlightable(xc, zc, 0, 2);
                    CheckHighlightable(xc, zc, 1, 2);
                    CheckHighlightable(xc, zc, 2, 1);
                    CheckHighlightable(xc, zc, 2, 0);
                    CheckHighlightable(xc, zc, 2, -1);
                    CheckHighlightable(xc, zc, 1, -2);
                    CheckHighlightable(xc, zc, 0, -2);
                    CheckHighlightable(xc, zc, -1, -2);
                    //third circle around 0, 0  18 hexes
                    CheckHighlightable(xc, zc, -1, -3);
                    CheckHighlightable(xc, zc, -2, -2);
                    CheckHighlightable(xc, zc, -2, -1);
                    CheckHighlightable(xc, zc, -3, 0);
                    CheckHighlightable(xc, zc, -2, 1);
                    CheckHighlightable(xc, zc, -2, 2);
                    CheckHighlightable(xc, zc, -1, 3);
                    CheckHighlightable(xc, zc, 0, 3);
                    CheckHighlightable(xc, zc, 1, 3);
                    CheckHighlightable(xc, zc, 2, 3);
                    CheckHighlightable(xc, zc, 2, 2);
                    CheckHighlightable(xc, zc, 3, 1);
                    CheckHighlightable(xc, zc, 3, 0);
                    CheckHighlightable(xc, zc, 3, -1);
                    CheckHighlightable(xc, zc, 2, -2);
                    CheckHighlightable(xc, zc, 2, -3);
                    CheckHighlightable(xc, zc, 1, -3);
                    CheckHighlightable(xc, zc, 0, -3);
                    break;
            }

    }

    public void HighlightTilesAttack()
    {
        Debug.Log("Highlight Attack Tiles.");
        //CheckHighlightable(xc, zc, 0, 0);
        //pgame.gameGrid.tiles[xc, zc].HighlightTile();
        //Debug.Log(i);
        int x, z;
   
        if(pgame.playerTurn == 1) {   //Highlight the opposing players' attackable units' tiles.
            foreach (int t in pgame.player1.attackableHQ)
            {
                x = pgame.player2.hq.xcoord;
                z = pgame.player2.hq.zcoord;
                pgame.gameGrid.tiles[x, z].HighlightAttackTile();
            }
            foreach (int t in pgame.player1.attackableTanks)
            {
                x = pgame.player2.tanks[t].xcoord;
                z = pgame.player2.tanks[t].zcoord;
                pgame.gameGrid.tiles[x, z].HighlightAttackTile();
            }
            foreach (int t in pgame.player1.attackableTroops)
            {
                Debug.Log("The index of the attackable troop is: " + t);
                //Debug.Log()
                x = pgame.player2.troops[t].xcoord;
                z = pgame.player2.troops[t].zcoord;
                Debug.Log("The coordinates of the attackable troop are: " + x + " " + z);
                pgame.gameGrid.tiles[x, z].HighlightAttackTile();
            }
        }
        else
        {
            foreach (int t in pgame.player2.attackableHQ)
            {
                x = pgame.player1.hq.xcoord;
                z = pgame.player1.hq.zcoord;
                pgame.gameGrid.tiles[x, z].HighlightAttackTile();
            }
            foreach (int t in pgame.player2.attackableTanks)
            {
                x = pgame.player1.tanks[t].xcoord;
                z = pgame.player1.tanks[t].zcoord;
                pgame.gameGrid.tiles[x, z].HighlightAttackTile();
            }
            foreach (int t in pgame.player2.attackableTroops)
            {
                x = pgame.player1.troops[t].xcoord;
                z = pgame.player1.troops[t].zcoord;
                pgame.gameGrid.tiles[x, z].HighlightAttackTile();
            }
        }

    }
    void CheckHighlightable(int xc, int zc, int xchange, int zchange)
    {
        /*int newx = xc + xchange;
        int newz = zc + zchange;
        if ((newx >= 0 && newx <= 19) && (newz >= 0 && newz <= 19))
        {
            Debug.Log(newx + " " + newz);
            pgame.gameGrid.tiles[newx, newz].HighlightTile();
        }*/

        int newx = xc + xchange;
        int newz = zc + zchange;

        if ((newx >= 0 && newx <= 19) && (newz >= 0 && newz <= 19))
        {
            if (pgame.gameGrid.tiles[newx, newz].isTextureHighlighted())
            {
                pgame.gameGrid.tiles[newx, newz].DehighlightTile();
                Debug.Log("Dehighlight tile: " + newx + " " + newz);
            }
            else
            {
                pgame.gameGrid.tiles[newx, newz].HighlightTile();
            }
        }
    }

    public int GetTankIndexForAttack(int x, int z)
    {
        int index = -1;
        foreach(Tank t in tanks)
        {
            if (t.xcoord == x && t.zcoord == z)
                index = t.index;
        }
        return index;
    }

    public bool CheckForEnemyUnits(int xc, int zc)   //looks for enemy units around the current unit at xc, zc.
    {
        int index = 0;
        bool foundEnemy = false;
        if (pgame.playerTurn == 1)
        {
            //HQ
            if (zc % 2 == 0)
            {
                if ((pgame.player2.hq.xcoord == xc - 1 && pgame.player2.hq.zcoord == zc) ||
                    (pgame.player2.hq.xcoord == xc - 1 && pgame.player2.hq.zcoord == zc + 1) ||
                    (pgame.player2.hq.xcoord == xc - 1 && pgame.player2.hq.zcoord == zc - 1) ||
                    (pgame.player2.hq.xcoord == xc && pgame.player2.hq.zcoord == zc + 1) ||
                    (pgame.player2.hq.xcoord == xc + 1 && pgame.player2.hq.zcoord == zc) ||
                    (pgame.player2.hq.xcoord == xc && pgame.player2.hq.zcoord == zc - 1))
                {
                    foundEnemy = true;
                    //Get HQ 
                    pgame.player2.hq.hqMaterial.material = pgame.player2.hq.attackMaterial;  //highlight the attackable hq
                    pgame.player1.attackableHQ.Add(0);  

                }
            }
            else
            {
                if ((pgame.player2.hq.xcoord == xc - 1 && pgame.player2.hq.zcoord == zc) || 
                    (pgame.player2.hq.xcoord == xc && pgame.player2.hq.zcoord == zc + 1) || 
                    (pgame.player2.hq.xcoord == xc + 1 && pgame.player2.hq.zcoord == zc + 1) ||
                    (pgame.player2.hq.xcoord == xc + 1 && pgame.player2.hq.zcoord == zc) ||
                    (pgame.player2.hq.xcoord == xc + 1 && pgame.player2.hq.zcoord == zc - 1) ||
                    (pgame.player2.hq.xcoord == xc && pgame.player2.hq.zcoord == zc - 1))
                {
                    foundEnemy = true;
                    //Get HQ 
                    pgame.player2.hq.hqMaterial.material = pgame.player2.hq.attackMaterial;  //highlight the attackable tanks
                    pgame.player1.attackableHQ.Add(0);
                }
            }

            //Tank
            foreach (Tank t in pgame.player2.tanks)
            {
                if (zc % 2 == 0)
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc - 1 && t.zcoord == zc + 1) || (t.xcoord == xc - 1 && t.zcoord == zc - 1)
                                                                || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        foundEnemy = true;
                        //Get Tank index of Player List of Tanks
                        pgame.player2.tanks[t.index-1].tankMaterial.material = pgame.player2.tankUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player1.attackableTanks.Add(t.index-1);  //store the index of an attackable tank from the list of player2's tanks
                    }
                }
                else
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc + 1)
                                                                                    || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc + 1 && t.zcoord == zc - 1) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        foundEnemy = true;
                        pgame.player2.tanks[t.index-1].tankMaterial.material = pgame.player2.tankUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player1.attackableTanks.Add(t.index-1);
                    }
                }
               
            }
            //Troop
            foreach (Troop t in pgame.player2.troops)
            {
                Debug.Log("index = " + t.index);
                if (zc % 2 == 0)
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc - 1 && t.zcoord == zc + 1) || (t.xcoord == xc - 1 && t.zcoord == zc - 1)
                                                            || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        Debug.Log("found enemy in CheckForEnemyUnits");
                        foundEnemy = true;
                        pgame.player2.troops[t.index-1].troopMaterial.material = pgame.player2.troopUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player1.attackableTroops.Add(t.index-1);
                    }
                }
                else
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc + 1)
                                                                                    || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc + 1 && t.zcoord == zc - 1) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        Debug.Log("found enemy in CheckForEnemyUnits");

                        foundEnemy = true;
                        pgame.player2.troops[t.index-1].troopMaterial.material = pgame.player2.troopUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player1.attackableTroops.Add(t.index-1);
                    }

                }
            }
        }
        if (pgame.playerTurn == 2)
        {
            if (zc % 2 == 0)
            {
                if ((pgame.player1.hq.xcoord == xc - 1 && pgame.player1.hq.zcoord == zc) ||
                    (pgame.player1.hq.xcoord == xc - 1 && pgame.player1.hq.zcoord == zc + 1) ||
                    (pgame.player1.hq.xcoord == xc - 1 && pgame.player1.hq.zcoord == zc - 1) ||
                    (pgame.player1.hq.xcoord == xc && pgame.player1.hq.zcoord == zc + 1) ||
                    (pgame.player1.hq.xcoord == xc + 1 && pgame.player1.hq.zcoord == zc) ||
                    (pgame.player1.hq.xcoord == xc && pgame.player1.hq.zcoord == zc - 1))
                {
                    foundEnemy = true;
                    //Get HQ 
                    pgame.player1.hq.hqMaterial.material = pgame.player1.hq.attackMaterial;  //highlight the attackable hq
                    pgame.player2.attackableHQ.Add(0);
                }
            }
            else
            {
                if ((pgame.player1.hq.xcoord == xc - 1 && pgame.player1.hq.zcoord == zc) ||
                    (pgame.player1.hq.xcoord == xc && pgame.player1.hq.zcoord == zc + 1) ||
                    (pgame.player1.hq.xcoord == xc + 1 && pgame.player1.hq.zcoord == zc + 1) ||
                    (pgame.player1.hq.xcoord == xc + 1 && pgame.player1.hq.zcoord == zc) ||
                    (pgame.player1.hq.xcoord == xc + 1 && pgame.player1.hq.zcoord == zc - 1) ||
                    (pgame.player1.hq.xcoord == xc && pgame.player1.hq.zcoord == zc - 1))
                {
                    foundEnemy = true;
                    //Get HQ 
                    pgame.player1.hq.hqMaterial.material = pgame.player1.hq.attackMaterial;  //highlight the attackable tanks
                    pgame.player2.attackableHQ.Add(0);

                }
            }
            foreach (Tank t in pgame.player1.tanks)
            {
                if (zc % 2 == 0)
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc - 1 && t.zcoord == zc + 1) || (t.xcoord == xc - 1 && t.zcoord == zc - 1)
                                                                || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        foundEnemy = true;
                        pgame.player1.tanks[t.index-1].tankMaterial.material = pgame.player1.tankUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player2.attackableTanks.Add(t.index-1);
                    }
                }
                else
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc + 1)
                                                                                    || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc + 1 && t.zcoord == zc - 1) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        foundEnemy = true;
                        pgame.player1.tanks[t.index-1].tankMaterial.material = pgame.player1.tankUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player2.attackableTanks.Add(t.index-1);
                    }
                }

            }
            foreach (Troop t in pgame.player1.troops)
            {
                if (zc % 2 == 0)
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc - 1 && t.zcoord == zc + 1) || (t.xcoord == xc - 1 && t.zcoord == zc - 1)
                                                            || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        foundEnemy = true;
                        pgame.player1.troops[t.index-1].troopMaterial.material = pgame.player1.troopUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player2.attackableTroops.Add(t.index-1);
                    }
                }
                else
                {
                    if ((t.xcoord == xc - 1 && t.zcoord == zc) || (t.xcoord == xc && t.zcoord == zc + 1) || (t.xcoord == xc + 1 && t.zcoord == zc + 1)
                                                                                    || (t.xcoord == xc + 1 && t.zcoord == zc) || (t.xcoord == xc + 1 && t.zcoord == zc - 1) || (t.xcoord == xc && t.zcoord == zc - 1))
                    {
                        foundEnemy = true;
                        pgame.player1.troops[t.index-1].troopMaterial.material = pgame.player1.troopUnit.attackMaterial;  //highlight the attackable tanks
                        pgame.player2.attackableTroops.Add(t.index-1);
                    }

                }
            }
        }
        return foundEnemy;

    }

    //IT
    public void Who()
    {
        Debug.Log(nameP);
        //return playerTurn+"";
    }

    public void PlacementTileSelected()
    {
       // if(placementTiles[0].)
        
    }

    void HighlightHQTiles(int p)
    {
        if (p == 1)
        {
            pgame.gameGrid.tiles[3, 2].HighlightTile();
            pgame.gameGrid.tiles[1, 2].HighlightTile();
            pgame.gameGrid.tiles[1, 3].HighlightTile();
            pgame.gameGrid.tiles[2, 3].HighlightTile();
            pgame.gameGrid.tiles[1, 1].HighlightTile();
            pgame.gameGrid.tiles[2, 1].HighlightTile();
        }
        else
        {
            /* pgame.gameGrid.tiles[18, 17].HighlightTile();
             pgame.gameGrid.tiles[16, 17].HighlightTile();
             pgame.gameGrid.tiles[18, 16].HighlightTile();
             pgame.gameGrid.tiles[17, 18].HighlightTile();
             pgame.gameGrid.tiles[18, 18].HighlightTile();
             pgame.gameGrid.tiles[17, 16].HighlightTile();
             */
            Debug.Log("Entered HQ Highlight Tiles");
            pgame.gameGrid.tiles[6, 5].HighlightTile();
            pgame.gameGrid.tiles[4, 5].HighlightTile();
            pgame.gameGrid.tiles[6, 4].HighlightTile();
            pgame.gameGrid.tiles[5, 6].HighlightTile();
            pgame.gameGrid.tiles[6, 6].HighlightTile();
            pgame.gameGrid.tiles[5, 4].HighlightTile();
        }
    }

    public void DehighlightHQTiles(int p)
    {
        if (p == 1)
        {
            pgame.gameGrid.tiles[3, 2].DehighlightTile();
            pgame.gameGrid.tiles[1, 2].DehighlightTile();
            pgame.gameGrid.tiles[1, 3].DehighlightTile();
            pgame.gameGrid.tiles[2, 3].DehighlightTile();
            pgame.gameGrid.tiles[1, 1].DehighlightTile();
            pgame.gameGrid.tiles[2, 1].DehighlightTile();
        }
        else
        {
            /* pgame.gameGrid.tiles[18, 17].DehighlightTile();
             pgame.gameGrid.tiles[16, 17].DehighlightTile();
             pgame.gameGrid.tiles[18, 16].DehighlightTile();
             pgame.gameGrid.tiles[17, 18].DehighlightTile();
             pgame.gameGrid.tiles[18, 18].DehighlightTile();
             pgame.gameGrid.tiles[17, 16].DehighlightTile();
             */
            pgame.gameGrid.tiles[6, 5].DehighlightTile();
            pgame.gameGrid.tiles[4, 5].DehighlightTile();
            pgame.gameGrid.tiles[6, 4].DehighlightTile();
            pgame.gameGrid.tiles[5, 6].DehighlightTile();
            pgame.gameGrid.tiles[6, 6].DehighlightTile();
            pgame.gameGrid.tiles[5, 4].DehighlightTile();

        }
    }
}
