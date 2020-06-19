using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove 
{
    GameObject target;

    public AudioSource audio;
    public AudioClip ataque;

    private bool isAttacking = false;
    private bool canKill = false;
    public static NPCMove instance;

    public bool attacked;

    private Animator animator;

    int i ;


	void Start () 
	{
        instance = this;
        Init();
        animator = GetComponentInChildren<Animator>();
	}
	
	void Update () 
	{

        if (!turn){
            i = 0;
            return;
        }

        else if (!moving && !attacking){
            isAttacking = false;
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

        animator.SetBool("isattacking", isAttacking);
        animator.SetBool("move", moving);
	}

        IEnumerator kill(GameObject x)
    {
        animator.SetBool("attack",true);
        yield return new WaitForSeconds(1.4f);
        audio.PlayOneShot(ataque);
        yield return new WaitForSeconds(0.2f);
        GameObject.Destroy(x);
        GameManager.PlayerChars--;
        animator.SetBool("attack",false);
    }

    void CalculatePath(){
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile); 


        if(canKill){
        //     // attacked = true;
        //     audio.PlayOneShot(ataque);
        //     transform.LookAt(target.transform);
        //     target.name = "DEAD";
        //     //GameObject.Destroy(target);
        //     canKill=false;
        //     attackDone=true;
        //     StartCoroutine(kill(target));
        //     //GameManager.PlayerChars--;
        //     //attacked = false;
            transform.LookAt(target.transform);
            StartCoroutine(kill(target));
            target.name = "DEAD";
            canKill=false;
            attackDone=true;    
        }
  
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
                if(d==1 && isAttacking){
                
                if(i==0){
                    i++;
                    canKill = true;
                }
                attacking=false;
                }
            }
        }
        target = nearest;
    }
}
