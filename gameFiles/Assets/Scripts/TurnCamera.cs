using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Cam1;
    public GameObject Cam2;
    bool tic = true;
    void Start()
    {
        Cam1.SetActive(true);
        Cam2.SetActive(false);
        tic = true;
    }
    public void RotateLeft(){
        if(tic){
            Cam1.SetActive(true);
            Cam2.SetActive(false);
            tic = false;
        }
        else{
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            tic = true;
        }
    }

    public void RotateRight(){
        if(tic){
            Cam1.SetActive(true);
            Cam2.SetActive(false);
            tic = false;
        }
        else{
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            tic = true;
        }
    }

}
