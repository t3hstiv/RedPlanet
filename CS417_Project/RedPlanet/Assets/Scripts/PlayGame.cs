using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
//IT
public class PlayGame : MonoBehaviour {
    //public Player p1;
    //public Player p2;
    public int playerTurn;
    public HexGrid gameGrid;
    public Player player1, player2;
    public HexTile[] HighlightedTiles;
    public Canvas winner;
    public Text winnerName;
    //public Tank tankUnit;
    public HexTile hx;
    //public HQ hq1, hq2;
    
    //IT
    // Use this for initialization
    void Start () {
        Debug.Log(Random.Range(40, 50));
        //player1.p_name = "Player1";
        //player2.p_name = "Player2";
        //Flip a coin to determine which player goes first.
        //player1 = player1.GetComponent<Player>();
        //player2 = player2.GetComponent<Player>();

        //player1 = Instantiate(player1);
        //player2 = Instantiate(player2);

        gameGrid.CreateGrid();
        //Player player1 = Instantiate<Player>(p1);
        //Player player2 = Instantiate<Player>(p2);
        float coin = Random.Range(0.0f, 1.0f);
        Debug.Log("The coin flip is: " + coin);
        player1.SendMessage("SetName", "Earth");
        player2.SendMessage("SetName", "Mars");

        if (coin < .5f)
        {
            playerTurn = 1;
            player1.turn = true;
            player2.turn = false;
            player1.SendMessage("DisplayName");
        }
        else
        {
            playerTurn = 2;
            player1.turn = false;
            player2.turn = true;
            player2.SendMessage("DisplayName");
        }
   
        player1.SendMessage("SetHQ",2);
        player2.SendMessage("SetHQ",5);
        player1.hq.xcoord = 2;
        player1.hq.zcoord = 2;
       // player2.hq.xcoord = 17;
       // player2.hq.zcoord = 17;
        player2.hq.xcoord = 5;
        player2.hq.zcoord = 5;
        gameGrid.tiles[2, 2].obstacle = true;
        //gameGrid.tiles[17, 17].obstacle = false;
        gameGrid.tiles[5, 5].obstacle = true;
        winner.enabled = false;
        
    }

    void Update () {
        if (player1.wins)
        {
            winner.enabled = true;
            winnerName.text = "Earth Wins!!!";
            StartOver();
        }
        if (player2.wins)
        {
            winner.enabled = true;
            winnerName.text = "Mars Wins!!!";
            StartCoroutine("StartOver"); 
                //StartOver();
        }
    }

    IEnumerator StartOver()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }

}
