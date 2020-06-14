using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove 
{
    GameObject target;

    public bool isAttacking = false;
    public bool canKill = false;

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

        else if (!moving && !attacking){
            FindNearestTarget();
            CalculatePath();
            FindSelectableTiles();
            actualTargetTile.target = true;
        }

        else if (attacking){
            if(!isAttacking){
                isAttacking = true;
                FindNearestTarget();
                CalculatePath();
                FindSelectableTiles();
                actualTargetTile.target = true;
            }
            Attack();
        }

        else{
            isAttacking = false;
            Move();
            actualTargetTile.target = false;
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
                if(d==1 && attacking){
                    Debug.Log("can kill");
                    canKill = true;
                }
            }
        }
        target = nearest;

        if(canKill){
            GameObject.Destroy(target);
            canKill=false;
        }
    }
}
