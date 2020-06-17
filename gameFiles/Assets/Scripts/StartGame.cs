using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class StartGame : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private bool GameStart = false;
    public GameObject button;


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

            SceneManager.LoadScene("SampleScene") ; 
        }
    }

    void Update()
    {
        if(!GameStart){

            OnButtonClick();
        }
    }
}
