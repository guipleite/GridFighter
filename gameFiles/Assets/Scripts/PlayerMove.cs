using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    GameObject target;

    // Start is called before the first frame update
    public bool isAttacking = false;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(!turn){
            return;
        }

        else if (!moving && !attacking){
            FindSelectableTiles();
            CheckClick();
        }
        
        else if (attacking){
            if(!isAttacking){
                isAttacking = true;     
                c=0;       
                FindSelectableTiles();
            }

            else{
                CheckClick();
                Attack();
            }
        }
        else{
            isAttacking = false;
            Move();
        }
    }

     void CheckClick(){

        if (Input.GetMouseButtonUp(0)){

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            
            if (Physics.Raycast(ray,out hit)){

                if (hit.collider.tag == "Tile"){

                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable){
                        if(attacking){
                            Physics.Raycast(t.transform.position, Vector3.up, out hit, 1);
                            if(hit.collider.tag=="NPC"){                            
                                GameObject.Destroy(hit.collider.gameObject);
                                attackDone=true;
                            }
                            else{                        
                                MoveToTile(t);
                            }
                        }
                        else{                        
                            MoveToTile(t);
                        }
                    }
                }
            }
        }
    }
   
    void CalculatePath(){
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }
}
