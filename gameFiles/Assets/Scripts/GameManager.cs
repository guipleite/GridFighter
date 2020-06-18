using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static int NPCChars = 0;
    public static int PlayerChars = 0;
    public static int level = 0;
    public static int totalKills = 0;



    bool rn = false;

    void Awake()
    {
        rn = false;
        NPCChars = 0;
        PlayerChars = 0;
    }

    // Update is called once per frame
    void Update()
    {        // if(units)
        if(!rn){
            Init();
            rn = true;
        }
        checkEndGame();
        checkEndLevel();

    }

    void Init(){
        

        foreach (TacticsMove unit in TurnManager.units["NPC"]){
            if(unit.name!=null )
                if(unit.name!="DEAD")     
                    NPCChars++;
        }
        foreach (TacticsMove unit in TurnManager.units["Player"]){
            if(unit.name!=null )
                if(unit.name!="DEAD")     
                    PlayerChars++;

        }

    }

    void checkEndGame(){
        if(PlayerChars<=0){
           
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene("EndGameScreenLoose");
            
        }
    }

    void checkEndLevel(){
        if(NPCChars<=0){
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene("EndLevel");
            level++;
        }
    }
}

