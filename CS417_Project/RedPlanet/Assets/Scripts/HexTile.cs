using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HexTile : MonoBehaviour {

    private Vector3[] v;
    private int[] t;
    private Vector2[] uv;
    public GameObject explosion;
    //orad and irad are the outer and inner radiuses
    //I decided 10 was too big and 1 was too small, so I went with 5
    public const float orad = 5f;
    public const float irad = orad * 0.866025404f;

    public int xcoord;
    public int zcoord;

    private MeshFilter mf;
    private MeshRenderer mr;
    private Mesh mesh;
    private MeshCollider mc;
    public Texture tex;

    public Texture highlightHex;
    public PlayGame pgame;
    private bool isHighlighted;

    public bool obstacle = false;
    // Use this for initialization
    void Start () {
        GameObject pGameObj = GameObject.FindGameObjectWithTag("PlayGameObj");
        pgame = pGameObj.GetComponent<PlayGame>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
  
    public void CreateMesh()
    {
        //Vertices for the mesh
        v = new Vector3[]
        {
            new Vector3(0f, 0f, orad),
            new Vector3(irad, 0f, 0.5f * orad),
            new Vector3(irad, 0f, -0.5f * orad),
            new Vector3(0f, 0f, -orad),
            new Vector3(-irad, 0f, -0.5f * orad),
            new Vector3(-irad, 0f, 0.5f * orad),
            new Vector3(0f, 0f, orad)
        };

        //Each number correlates to a vertex in the above array. Makes triangles for the mesh
        t = new int[]
        {
            4, 5, 3,
            5, 0, 3,
            0, 2, 3,
            0, 1, 2
        };

        //For UV mapping of the mesh. Each point is related to the Vector3 array above.
        uv = new Vector2[]
        {
            new Vector2(0.5f, 1f),
            new Vector2(0.866025404f, 0.75f),
            new Vector2(0.866025404f, 0.25f),
            new Vector2(0.5f, 0f),
            new Vector2(1-0.866025404f, 0.25f),
            new Vector2(0.133971596f, 0.75f),
            new Vector2(0.5f, 1f)
        };

        //Add necessary componenets, then attribute the arrays made above to the appropriate mesh properties.
        mf = gameObject.AddComponent<MeshFilter>();
        mr = gameObject.AddComponent<MeshRenderer>();
        mc = gameObject.AddComponent<MeshCollider>();
        //pgame = gameObject.GetComponent<PlayGame>();
        mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = v;
        mesh.uv = uv;
        mesh.triangles = t;
        mesh.name = "HexTile";
        mesh.RecalculateNormals();
        mf.mesh = mesh;
        mc.sharedMesh = mesh;
        mr.material.mainTexture = tex;
        isHighlighted = false;
    }

    public bool isTextureHighlighted()
    {
        return isHighlighted;
    }

    //Shows coordinates when mouse enters a new tile
    void OnMouseEnter()
    {
        //Create an Image

        //highlight.enabled = true;
        //highlight.GetComponent<Image>().gameObject.transform.position = new Vector3(xcoord, 0, zcoord);
        //mr.material.mainTexture = highlightHex;
        //Debug.Log("Coordinates are " + xcoord + ", " + zcoord);
    }

    void OnMouseExit()
    {
        //mr.material.mainTexture = tex;
    }

    public void OnMouseDown()
    {
        /* if (mr.material.mainTexture == highlightHex)
         {
             Vector3 position = transform.position;
             position.y = 2;
             pgame.player1.SpawnTank(position);
         }*/

        //pgame.player1.SendMessage("Who");
        //pgame.player2.SendMessage("Who");
        Debug.Log("Mouse Down on tile.");


        Vector3 position = transform.position;
        //Vector3 position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        position.y = 2;
        Debug.Log("before highlight troop position = " + position);

        if (isHighlighted == true)
        {
            if (pgame != null)
            {
                if (pgame.playerTurn == 1)
                {
                    Debug.Log("selectedTank = " + pgame.player1.selectedTank);

                    if (pgame.player1.selectedTank >= 0 && !pgame.player1.attackMode)    //IT   move. a player can still attack after a move. so, (move) or (move and then attack) is what is HERE>
                    {
                        //Get the current tanks' coordinates to reset the obstacle flag.
                        Tank t = pgame.player1.tanks[pgame.player1.selectedTank];
                        int x = t.xcoord;
                        int z = t.zcoord;
                        pgame.gameGrid.tiles[x, z].obstacle = false;
                        pgame.player1.MoveTank(xcoord, zcoord, pgame.player1.selectedTank);
                        //Check to set the selectedTank to -1 and moved to true, if no attack is available HERE_____________________________________________________________________
                        if (pgame.player1.CheckForEnemyUnits(xcoord, zcoord))   //check to see if the Attack button should be activated or not to allow for a proximity attack before moving. This sets up the next if statement.
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = true;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("Found enemies found near troop at Earth!");
                        }
                        else
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = false;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("No enemies found near troop at Earth!");
                        }
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;

                    }
                    
                    if (pgame.player1.selectedTank >= 0 && pgame.player1.attackMode)    //IT  attack. NO moving after an attack. tiles have already been highlighted.
                    {
                        Tank t1 = pgame.player1.tanks[pgame.player1.selectedTank];
                        foreach (int t in pgame.player1.attackableHQ)
                        {
                            if (pgame.player2.hq.xcoord == xcoord && pgame.player2.hq.zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //This tank will attack the HQ now.
                                Tank p1 = pgame.player1.tanks[pgame.player1.selectedTank];

                                //Highlight the attacked tank as yellow.
                                pgame.player2.hq.hqMaterial.material = pgame.player2.hq.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p1.healthPoints -= (int)(0.75 * pgame.player2.hq.hqAttack);   //HQ only does 10 damage.
                                else
                                    p1.healthPoints -= (int)(pgame.player2.hq.hqAttack);
                                //HQ is on a mountain, so no defense bonus. We can incorporate a special HQ defense bouns later.
                                pgame.player2.hq.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));

                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    //p1.gameObject.active = false;
                                    pgame.gameGrid.tiles[p1.xcoord, p1.zcoord].obstacle = false;
                                }
                                if (pgame.player2.hq.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, pgame.player2.hq.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    //pgame.player2.hq.gameObject.active = false;
                                    obstacle = false;
                                }

                                break;
                            }
                        }
                        foreach (int t in pgame.player1.attackableTanks)
                        {
                            if(pgame.player2.tanks[t].xcoord == xcoord && pgame.player2.tanks[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            { 
                                //These tanks will attack eachother now.
                                Tank p1 = pgame.player1.tanks[pgame.player1.selectedTank];
                                Tank p2 = pgame.player2.tanks[t];
                                //Highlight the attacked tank as yellow.
                                p2.tankMaterial.material = p2.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p1.healthPoints -= (int)(0.75 * Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));

                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                   GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    //p1.gameObject.active = false;
                                    pgame.gameGrid.tiles[p1.xcoord, p1.zcoord].obstacle = false;
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    //p2.gameObject.active = false;
                                    obstacle = false;
                                }

                                break;
                            }
                        }
                        foreach (int t in pgame.player1.attackableTroops)
                        {
                            if (pgame.player2.troops[t].xcoord == xcoord && pgame.player2.troops[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //These units will attack eachother now.
                                Tank p1 = pgame.player1.tanks[pgame.player1.selectedTank];
                                Troop p2 = pgame.player2.troops[t];
                                p2.troopMaterial.material = p2.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p1.healthPoints -= (int)(0.75 * Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));

                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    //p1.gameObject.active = false;
                                    pgame.gameGrid.tiles[p1.xcoord, p1.zcoord].obstacle = false;
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    //p2.gameObject.active = false;
                                    obstacle = false;
                                }
                                break;
                            }
                        }
                        t1.commandMenu.enabled = true;   //enable the move and the attack options.
                        t1.commandUnit.attack.interactable = false;
                        t1.commandUnit.move.interactable = false;
                        t1.commandUnit.end.interactable = true;
                        pgame.player1.attackMode = false;
                    }
                    if (pgame.player1.wantTank)    //IT spawn
                    {
                        pgame.player1.SpawnTank(position, xcoord, zcoord);
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;
                        pgame.player1.hq.hqMaterial.material = pgame.player1.hq.initialMaterial;   //IT
                        pgame.player1.hq.selected = false;   //IT
                    }

                    Debug.Log("selectedTroop = " + pgame.player1.selectedTroop);
                    if (pgame.player1.selectedTroop >= 0 && !pgame.player1.attackMode)   //IT
                    {
                        //Get the current tanks' coordinates to reset the obstacle flag.
                        Troop t = pgame.player1.troops[pgame.player1.selectedTroop];
                        int x = t.xcoord;
                        int z = t.zcoord;
                        pgame.gameGrid.tiles[x, z].obstacle = false;
                        pgame.player1.MoveTroop(xcoord, zcoord, pgame.player1.selectedTroop);
                        if (pgame.player1.CheckForEnemyUnits(xcoord, zcoord))   //check to see if the Attack button should be activated or not to allow for a proximity attack before moving. This sets up the next if statement.
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = true;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("Found enemies found near troop at Earth!");
                        }
                        else
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = false;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("No enemies found near troop at Earth!");
                        }
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;
                    }
                    if (pgame.player1.selectedTroop >= 0 && pgame.player1.attackMode)    //IT  attack. NO moving after an attack. tiles have already been highlighted.
                    {
                        Troop t1 = pgame.player1.troops[pgame.player1.selectedTroop];
                        foreach (int t in pgame.player1.attackableHQ)
                        {
                            if (pgame.player2.hq.xcoord == xcoord && pgame.player2.hq.zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //This tank will attack the HQ now.
                                Troop p1 = pgame.player1.troops[pgame.player1.selectedTroop];

                                //Highlight the attacked tank as yellow.
                                pgame.player2.hq.hqMaterial.material = pgame.player2.hq.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p1.healthPoints -= (int)(0.75 * pgame.player2.hq.hqAttack);   //HQ only does 10 damage.
                                else
                                    p1.healthPoints -= (int)(pgame.player2.hq.hqAttack);
                                //HQ is on a mountain, so no defense bonus. We can incorporate a special HQ defense bouns later.
                                pgame.player2.hq.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));

                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    //p1.gameObject.active = false;
                                    pgame.gameGrid.tiles[p1.xcoord, p1.zcoord].obstacle = false;
                                }
                                if (pgame.player2.hq.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, pgame.player2.hq.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    //pgame.player2.hq.gameObject.active = false;
                                    obstacle = false;
                                }

                                break;
                            }
                        }
                        foreach (int t in pgame.player1.attackableTanks)
                        {
                            if (pgame.player2.tanks[t].xcoord == xcoord && pgame.player2.tanks[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //These tanks will attack eachother now.
                                Troop p1 = pgame.player1.troops[pgame.player1.selectedTroop];
                                Tank p2 = pgame.player2.tanks[t];
                                p2.tankMaterial.material = p2.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p1.healthPoints -= (int)(0.75 * Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));
                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    // Destroy(p1, 3f);
                                    //p1.gameObject.active = false;
                                    pgame.gameGrid.tiles[p1.xcoord, p1.zcoord].obstacle = false;
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                   // Destroy(p2, 3f);
                                    //p2.gameObject.active = false;
                                    obstacle = false;
                                }
                                break;
                            }
                        }
                        foreach (int t in pgame.player1.attackableTroops)
                        {
                            if (pgame.player2.troops[t].xcoord == xcoord && pgame.player2.troops[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //These units will attack eachother now.
                                Troop p1 = pgame.player1.troops[pgame.player1.selectedTroop];
                                Troop p2 = pgame.player2.troops[t];
                                p2.troopMaterial.material = p2.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p1.healthPoints -= (int)(0.75 * Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));
                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    //p1.gameObject.active = false;
                                    pgame.gameGrid.tiles[p1.xcoord, p1.zcoord].obstacle = false;
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                   // Destroy(p2, 3f);
                                    //p2.gameObject.active = false;
                                    obstacle = false;
                                }
                                break;
                            }
                        }
                        t1.commandMenu.enabled = true;   //enable the move and the attack options.
                        t1.commandUnit.attack.interactable = false;
                        t1.commandUnit.move.interactable = false;
                        t1.commandUnit.end.interactable = true;
                        pgame.player1.attackMode = false;
                    }
                    if (pgame.player1.wantTroop)
                    {
                        Debug.Log("troop position = " + position);

                        pgame.player1.SpawnTroop(position, xcoord, zcoord);
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;
                        pgame.player1.hq.hqMaterial.material = pgame.player1.hq.initialMaterial;   //IT
                        pgame.player1.hq.selected = false;   //IT
                    }
                }
                else
                {
                    Debug.Log("selectedTank = " + pgame.player2.selectedTank);
                    if (pgame.player2.selectedTank >= 0 && !pgame.player2.attackMode)    //IT
                    {
                        //Get the current tanks' coordinates to reset the obstacle flag.
                        Tank t = pgame.player2.tanks[pgame.player2.selectedTank];
                        int x = t.xcoord;
                        int z = t.zcoord;
                        pgame.gameGrid.tiles[x, z].obstacle = false;
                        pgame.player2.MoveTank(xcoord, zcoord, pgame.player2.selectedTank);
                        if (pgame.player2.CheckForEnemyUnits(xcoord, zcoord))   //check to see if the Attack button should be activated or not to allow for a proximity attack before moving. This sets up the next if statement.
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = true;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("Found enemies found near troop at Earth!");
                        }
                        else
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = false;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("No enemies found near troop at Earth!");
                        }
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;
                    }
                    if (pgame.player2.selectedTank >= 0 && pgame.player2.attackMode)    //IT  attack. NO moving after an attack. tiles have already been highlighted.
                    {
                        Tank t1 = pgame.player2.tanks[pgame.player2.selectedTank];
                        foreach (int t in pgame.player2.attackableHQ)
                        {
                            if (pgame.player1.hq.xcoord == xcoord && pgame.player1.hq.zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //This tank will attack the HQ now.
                                Tank p2 = pgame.player2.tanks[pgame.player2.selectedTank];

                                //Highlight the attacked tank as yellow.
                                pgame.player1.hq.hqMaterial.material = pgame.player1.hq.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p2.healthPoints -= (int)(0.75 * pgame.player1.hq.hqAttack);   
                                else
                                    p2.healthPoints -= (int)(pgame.player1.hq.hqAttack);
                                //HQ is on a mountain, so no defense bonus. We can incorporate a special HQ defense bouns later.
                                pgame.player1.hq.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));

                                //Check for destroyed!
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    // p2.gameObject.active = false;
                                    pgame.gameGrid.tiles[p2.xcoord, p2.zcoord].obstacle = false;
                                }
                                if (pgame.player1.hq.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, pgame.player1.hq.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    //pgame.player1.hq.gameObject.active = false;
                                    obstacle = false;
                                }

                                break;
                            }
                        }
                        foreach (int t in pgame.player2.attackableTanks)
                        {
                            if (pgame.player1.tanks[t].xcoord == xcoord && pgame.player1.tanks[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //These tanks will attack eachother now.
                                Tank p2 = pgame.player2.tanks[pgame.player2.selectedTank];
                                Tank p1 = pgame.player1.tanks[t];
                                p1.tankMaterial.material = p1.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p1.healthPoints -= (int)(0.75*Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    // Destroy(p1, 3f);
                                    //p1.gameObject.active = false;
                                    obstacle = false;
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                    // Destroy(p2, 3f);
                                    // p2.gameObject.active = false;
                                    pgame.gameGrid.tiles[p2.xcoord, p2.zcoord].obstacle = false;
                                }
                                break;
                            }
                        }
                        foreach (int t in pgame.player2.attackableTroops)
                        {
                            if (pgame.player1.troops[t].xcoord == xcoord && pgame.player1.troops[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //These units will attack eachother now.
                                Tank p2 = pgame.player2.tanks[pgame.player2.selectedTank];
                                Troop p1 = pgame.player1.troops[t];
                                p1.troopMaterial.material = p1.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p1.healthPoints -= (int)(0.75 * Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    // p1.gameObject.active = false;
                                    obstacle = false;
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    // p2.gameObject.active = false;
                                    pgame.gameGrid.tiles[p2.xcoord, p2.zcoord].obstacle = false;
                                }
                                break;
                            }
                        }
                        t1.commandMenu.enabled = true;   //enable the move and the attack options.
                        t1.commandUnit.attack.interactable = false;
                        t1.commandUnit.move.interactable = false;
                        t1.commandUnit.end.interactable = true;
                        pgame.player2.attackMode = false;
                    }
                    if (pgame.player2.wantTank)
                    {
                        pgame.player2.SpawnTank(position, xcoord, zcoord);
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;
                        pgame.player2.hq.hqMaterial.material = pgame.player2.hq.initialMaterial;   //IT
                        pgame.player2.hq.selected = false;   //IT
                    }
                    
                    Debug.Log("selectedTroop = " + pgame.player2.selectedTroop);

                    if (pgame.player2.selectedTroop >= 0 && !pgame.player2.attackMode)    //IT
                    {
                        //Get the current tanks' coordinates to reset the obstacle flag.
                        Troop t = pgame.player2.troops[pgame.player2.selectedTroop];
                        int x = t.xcoord;
                        int z = t.zcoord;
                        pgame.gameGrid.tiles[x, z].obstacle = false;
                        pgame.player2.MoveTroop(xcoord, zcoord, pgame.player2.selectedTroop);
                        if (pgame.player2.CheckForEnemyUnits(xcoord, zcoord))   //check to see if the Attack button should be activated or not to allow for a proximity attack before moving. This sets up the next if statement.
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = true;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("Found enemies found near troop at Earth!");
                        }
                        else
                        {
                            t.commandMenu.enabled = true;   //enable the move and the attack options.
                            t.commandUnit.attack.interactable = false;
                            t.commandUnit.move.interactable = false;
                            t.commandUnit.end.interactable = true;
                            Debug.Log("No enemies found near troop at Earth!");
                        }
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;
                    }
                    if (pgame.player2.selectedTroop >= 0 && pgame.player2.attackMode)    //IT  attack. NO moving after an attack. tiles have already been highlighted.
                    {
                        Troop t1 = pgame.player2.troops[pgame.player2.selectedTroop];
                        foreach (int t in pgame.player2.attackableHQ)
                        {
                            if (pgame.player1.hq.xcoord == xcoord && pgame.player1.hq.zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //This tank will attack the HQ now.
                                Troop p2 = pgame.player2.troops[pgame.player2.selectedTroop];

                                //Highlight the attacked tank as yellow.
                                pgame.player1.hq.hqMaterial.material = pgame.player1.hq.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p2.healthPoints -= (int)(0.75 * pgame.player1.hq.hqAttack);
                                else
                                    p2.healthPoints -= (int)(pgame.player1.hq.hqAttack);
                                //HQ is on a mountain, so no defense bonus. We can incorporate a special HQ defense bouns later.
                                pgame.player1.hq.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));

                                //Check for destroyed!
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                    // p2.gameObject.active = false;
                                    pgame.gameGrid.tiles[p2.xcoord, p2.zcoord].obstacle = false;
                                }
                                if (pgame.player1.hq.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, pgame.player1.hq.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    //pgame.player1.hq.gameObject.active = false;
                                    obstacle = false;
                                }

                                break;
                            }
                        }
                        foreach (int t in pgame.player2.attackableTanks)
                        {
                            if (pgame.player1.tanks[t].xcoord == xcoord && pgame.player1.tanks[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //These tanks will attack eachother now.
                                Troop p2 = pgame.player2.troops[pgame.player2.selectedTroop];
                                Tank p1 = pgame.player1.tanks[t];
                                p1.tankMaterial.material = p1.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p1.healthPoints -= (int)(0.75 * Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                    //Destroy(p1, 3f);
                                   // p1.gameObject.active = false;
                                    obstacle = false;
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy(p2, 3f);
                                    // p2.gameObject.active = false;
                                    pgame.gameGrid.tiles[p2.xcoord, p2.zcoord].obstacle = false;
                                }
                                break;
                            }
                        }
                        foreach (int t in pgame.player2.attackableTroops)
                        {
                            if (pgame.player1.troops[t].xcoord == xcoord && pgame.player1.troops[t].zcoord == zcoord)   //this is the clicked on opponents tank
                            {
                                //These units will attack eachother now.
                                Troop p2 = pgame.player2.troops[pgame.player2.selectedTroop];
                                Troop p1 = pgame.player1.troops[t];
                                p1.troopMaterial.material = p1.yellowHighlight;
                                if (pgame.gameGrid.tiles[xcoord, zcoord].name == "Hill")
                                    p2.healthPoints -= (int)(0.75 * Random.Range(p1.attack[0], p1.attack[1]));
                                else
                                    p2.healthPoints -= (int)(Random.Range(p1.attack[0], p1.attack[1]));
                                if (name == "Hill") //25% reduction in attack.
                                    p1.healthPoints -= (int)(0.75 * Random.Range(p2.attack[0], p2.attack[1]));
                                else
                                    p1.healthPoints -= (int)(Random.Range(p2.attack[0], p2.attack[1]));
                                //Check for destroyed!
                                if (p1.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p1.transform.position, Quaternion.identity) as GameObject;
                                    //Destroy Tank
                                   // Destroy(p1, 3f);
                                   // p1.gameObject.active = false;
                                    obstacle = false;   //this is the current click at xcoord, zcoord
                                }
                                if (p2.healthPoints <= 0)
                                {
                                    GameObject ex = Instantiate(explosion, p2.transform.position, Quaternion.identity) as GameObject;
                                   // Destroy(p2, 3f);
                                    //p2.gameObject.active = false;
                                    pgame.gameGrid.tiles[p2.xcoord, p2.zcoord].obstacle = false;
                                }
                                break;
                            }
                        }
                        t1.commandMenu.enabled = true;   //enable the move and the attack options.
                        t1.commandUnit.attack.interactable = false;
                        t1.commandUnit.move.interactable = false;
                        t1.commandUnit.end.interactable = true;
                        pgame.player2.attackMode = false;
                    }
                    if (pgame.player2.wantTroop)
                    {
                        Debug.Log("troop position = " + position);

                        pgame.player2.SpawnTroop(position, xcoord, zcoord);
                        pgame.gameGrid.tiles[xcoord, zcoord].obstacle = true;
                        pgame.player2.hq.hqMaterial.material = pgame.player2.hq.initialMaterial;   //IT
                        pgame.player2.hq.selected = false;   //IT
                    }
                }
            }
        }
        Debug.Log("troop after position = " + position);

        isHighlighted = false;
    }
        
        
    public void HighlightAttackTile()
    {
        mr.material.mainTexture = highlightHex;
        isHighlighted = true;
        Debug.Log(xcoord + " " + zcoord);
    }

    public void HighlightTile()
    {
        //if xcoord, zcoord are marked as an obstacle, don't highlight that hex
        if (!obstacle)
        {
            mr.material.mainTexture = highlightHex;
            isHighlighted = true;
            Debug.Log(xcoord + " " + zcoord);
        }
    }

    public void DehighlightTile()
    {
        mr.material.mainTexture = tex;
        isHighlighted = false;
    }

}