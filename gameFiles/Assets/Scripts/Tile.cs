using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : TacticsMove
{
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool walkable = true;

    public bool processed = false;
    public Tile parent = null;
    public int distance = 0; 

    public List<Tile> adjacencyList = new List<Tile>();

    public float f = 0;
    public float g = 0;
    public float h = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current){GetComponent<Renderer>().material.color = Color.gray;}
        
        else if(target){GetComponent<Renderer>().material.color = Color.green;}
        
        else if(selectable){GetComponent<Renderer>().material.color = Color.cyan;}
        
        else{GetComponent<Renderer>().material.color = Color.white;}
    }

    public void  Reset() {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;
        processed = false;
        parent = null;
        distance = 0; 

        f = g = h = 0;
    }

    public void FindNeighbors(float jumpHeight,Tile target,bool attacking){
        Reset();

        CheckTile(Vector3.forward, jumpHeight, target, attacking);
        CheckTile(-Vector3.forward, jumpHeight, target, attacking);
        CheckTile(Vector3.right, jumpHeight, target, attacking);
        CheckTile(-Vector3.right, jumpHeight, target, attacking);
    }

    public void CheckTile(Vector3 direction, float jumpHeight, Tile target, bool attacking){

        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders){

            Tile tile = item.GetComponent<Tile>();

            if (tile != null && tile.walkable){
                RaycastHit hit;

                if(attacking ){
                    Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1);
                    if(hit.collider){
                        if (hit.collider.tag=="NPC"){
                            adjacencyList.Add(tile);
                        } 
                    }
                }

                else if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target)){
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
