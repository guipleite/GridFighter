using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    GameObject target;

    public AudioSource audio;
    public AudioClip ataque;


    // Start is called before the first frame update
    public bool isAttacking = false;

    public bool attacked;

    bool dying = false;

    private Animator animator;

    public static PlayerMove instance;

    void Start()
    {
        instance = this;
        Init();
        animator = GetComponentInChildren<Animator>();
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
        animator.SetBool("isattacking", isAttacking);
        animator.SetBool("move", moving);
    }

        IEnumerator kill(GameObject x)
    {
        //attacked = true;
        // animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.6f);
        audio.PlayOneShot(ataque);
        GameObject.Destroy(x);
        GameManager.NPCChars--;
        animator.SetBool("attack", false);


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
                            
                            if (hit.collider.tag=="NPC"){
                                transform.LookAt(hit.collider.transform);
                                animator.SetBool("attack", true);

                                StartCoroutine(kill(hit.collider.gameObject)); 
                               
                                hit.collider.gameObject.name = "DEAD";
                                attackDone=true;
                                
                                GameManager.totalKills++;
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