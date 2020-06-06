using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove 
{
    GameObject target;

	void Start () 
	{
        Init();
	}
	
	void Update () 
	{
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn){
            return;
        }

        else if (!moving){
            FindNearestTarget();
            CalculatePath();
            FindSelectableTiles();
            actualTargetTile.target = true;
        }
        else{
            Move();
        }
	}

    void CalculatePath(){
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    void FindNearestTarget(){
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets) {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance) {
                distance = d;
                nearest = obj;
            }
        }
        target = nearest;
    }
}
