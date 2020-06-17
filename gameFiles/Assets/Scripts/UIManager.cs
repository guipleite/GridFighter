using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Level;
    public GameObject Kills;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Level.GetComponent<Text>().text = (""+(GameManager.level+1));
        Kills.GetComponent<Text>().text = ("x " + GameManager.totalKills);
    }
}
