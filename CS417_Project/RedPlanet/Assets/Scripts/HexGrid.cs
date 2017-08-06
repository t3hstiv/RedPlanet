using UnityEngine;
using System.Collections;

public class HexGrid : MonoBehaviour {

    public HexTile tileGrassPrefab;
    public HexTile tileRoadPrefab;
    public HexTile tileHillPrefab;
    public HexTile tileMountainPrefab;
    public PlayGame pgame;

    public HexTile[,] tiles;
    HexTile tile;

	// Use this for initialization
	void Start () {
        
	}
	
    public void CreateGrid()
    {
        tiles = new HexTile[20, 20];

        for (int z = 0; z < 20; z++)
        {
            for (int x = 0; x < 20; x++)
            {
                CreateTile(x, z);
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}

    void CreateTile(int x, int z)
    {
        Vector3 pos;

        //Sets position for x axis using inner radius. Offset depending on the row
        if (z % 2 == 0)
        {
            pos.x = x * HexTile.irad * 2f;
        }
        else
        {
            pos.x = (x * HexTile.irad * 2f) + HexTile.irad;
        }

        pos.y = 0f;

        //Since each row is offset, z only needs to move up 1.5 to fit
        pos.z = z * HexTile.orad * 1.5f;

        //Create tile with different prefabs
        if (isRoadTile(x, z))
        {
            tile = Instantiate<HexTile>(tileRoadPrefab);
            tile.name = "Road";
        }
        else if (isMountainTile(x, z))
        {
            tile = Instantiate<HexTile>(tileMountainPrefab);
            tile.obstacle = true;
            tile.name = "Mountain";
        }
        else if (isHillTile(x, z))
        {
            tile = Instantiate<HexTile>(tileHillPrefab);
            //tile.gameObject.transform.tag = "Hill";   //IT
            tile.name = "Hill";
        }
        else
        {
            tile = Instantiate<HexTile>(tileGrassPrefab);
            tile.name = "Grass";
        }

        //Places tile at the position derived above
        tile.transform.localPosition = pos;

        //Attaches tile transform to parent transform (HexGrid object)
        //tile.transform.SetParent(transform, false);

        tiles[x, z] = tile;
        tile.xcoord = x;
        tile.zcoord = z;
        //tile.name = "HexTile[" + x + ", " + z + "]";
       // tile.pgame = pgame;
        tile.CreateMesh();
    }

    //Places road prefab at specific coordinates
    bool isRoadTile(int x, int z)
    {
        if ((z == 3 || z == 16) && (x > 2 && x < 17))
        {
            return true;
        }
        else if ((z > 3 && z < 17) && ((x == 9 && z % 2 == 1) || (x == 10 && z % 2 == 0)))
        {
            return true;
        }
        return false;
    }

    //Randomizes mountain tile placement. Also being used for HQ position (2,2 and 17,17)
    bool isMountainTile(int x, int z)
    {
        float rn = Random.value * 100f;
        if (x == 1 && z == 2 || x == 3 && z == 2 || x == 1 && z == 3 || x == 2 && z == 3 || x == 1 && z == 1 || x == 2 && z == 1)
        {
            return false;
        }
        //else if (x == 16 && z == 17 || x == 18 && z == 17 || x == 17 && z == 18 || x == 18 && z == 18 || x == 17 && z == 16 || x == 18 && z == 16)
        else if (x == 4 && z == 5 || x == 6 && z == 5 || x == 5 && z == 6 || x == 6 && z == 6 || x == 5 && z == 4 || x == 6 && z == 4)
        {
            return false;
        }
        //else if (x == 2 && z == 2 || x == 17 && z == 17)
        else if (x == 2 && z == 2 || x == 5 && z == 5)
        {
            return true;
        }
        else if (rn < 15)
        {
            return true;
        }
        return false;
    }

    //Randomizes hill tile placement
    bool isHillTile(int x, int z)
    {
        float rn = Random.value * 100f;
        if (rn < 25)
        {
            return true;
        }
        return false;
    }
}
