using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private bool GameStart = false;
    public GameObject button;

    private List<string> LevelList = new List<string>(new string[] { "SampleScene", "level2", "level3","level4","level5","EndGameVic" });

    public GameObject Level;
    public GameObject Kills;
    // private AudioSource audioSource;
    // public AudioClip sound;
    
    // Start is called before the first frame update
    void Start()
    {
        GameStart = false;

        // audioSource = GetComponent<AudioSource>();
        // audioSource.clip = sound;
        // audioSource.loop = true;
        // audioSource.Play();
    }

    public void OnButtonClick(){

        var go = EventSystem.current.currentSelectedGameObject;
        if (go ==button){
            GameStart = true;

            if(SceneManager.GetActiveScene().name=="EndGameScreenLoose"){
                GameManager.level = 0;
                GameManager.totalKills = 0;
            }
            SceneManager.LoadScene(LevelList[GameManager.level]) ; 
        }
    }

    void Update()
    {
        if(!GameStart){

            OnButtonClick();
        }
        if(SceneManager.GetActiveScene().name!="MainMenu" && !GameStart){
            Level.GetComponent<Text>().text = (""+GameManager.totalKills);
            Kills.GetComponent<Text>().text = (""+GameManager.level);
        }
       
    }
}
