using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{   
   
    public int moveRange = 4;
    public float jumpHeight = 0;
    public float moveSpeed = 2;
    public int c = 0;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    float halfHeight = 0;
    public bool moving = false;
    public bool turn = false;
    public bool attacking = false;
    public int attackRange = 1;
    public bool attackDone = false;


    public Tile actualTargetTile;

    // Start is called before the first frame update
    protected void Init(){

        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }
     
    public void GetCurrentTile(){

        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target){
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position,-Vector3.up, out hit,1)){

            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight,Tile target){

        foreach (GameObject tile in tiles){
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight,target,attacking);
        }
    }
    
    public void FindSelectableTiles(){

        ComputeAdjacencyLists(jumpHeight,null);
        GetCurrentTile();

        int range;
        if(attacking) {range = attackRange;}
        else {range = moveRange;}

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.processed = true;

        while (process.Count>0){
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance <range){
                foreach (Tile tile in t.adjacencyList){
                    if (!tile.processed){
                        tile.parent = t;
                        tile.processed = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
    
    public void MoveToTile(Tile tile){

        path.Clear();

        tile.target = true;
        moving = true;

        Tile next = tile;

        while (next != null){
            path.Push(next);
            next = next.parent;
        }
    }

     public void Move(){
        if (path.Count > 0){

            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight+t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position,target)>= 0.05f){

                CalculateHeading(target);
                velocity = heading * moveSpeed;

                // TODO: ADD ANIMATIONS
                transform.forward = heading;
                transform.position += velocity*Time.deltaTime;
            }

            else{
                transform.position = target;
                path.Pop();
            }
        }

        else{
            RemoveSelectableTiles();
            moving = false;
            attacking = true;
        }
    }

    public void Attack(){

        if (path.Count> 0){

            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight+t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position,target)>= 0.05f){

                CalculateHeading(target);
                velocity = heading * moveSpeed;

                // TODO: ADD ANIMATIONS
                transform.forward = heading;
                transform.position += velocity*Time.deltaTime;
            }


            else{
                transform.position = target;
                path.Pop();
            }
            c+=1;
        }
        else if(attackDone){
            Debug.Log("DNoes");
            RemoveSelectableTiles();
            attacking = false;
            moving = false;
            attackDone = false;

            TurnManager.EndTurn();
        }

        else if( c > 0 && this.tag=="Player"){
            RemoveSelectableTiles();
            attacking = false;
            moving = false;           
            attackDone = false;

            TurnManager.EndTurn();
        }

        else if(this.tag=="NPC"){
            RemoveSelectableTiles();
            attacking = false;
            moving = false;
            attackDone = false;


            TurnManager.EndTurn();
        }
    }

    protected void RemoveSelectableTiles(){
        if (currentTile != null){

            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles){
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target){
        heading = target - transform.position;
        heading.Normalize();
    }

    public void BeginTurn(){
        turn = true;
        Debug.Log("BEGINS");

    }

    public void EndTurn(){
        turn = false;
        attacking = false;

    }

    protected Tile FindLowestF(List<Tile> list){

        Tile lowest = list[0];

        foreach (Tile t in list){
            if (t.f < lowest.f){
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t){
        int range;
        if(attacking) {range = attackRange;}
        else {range = moveRange;}

        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null){
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= range){
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= range; i++){
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile target){
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);

        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count> 0){
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target ){
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList){
                if (closedList.Contains(tile)){
                    
                }

                else if (openList.Contains(tile)){
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g){
                        tile.parent = t;
                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                
                else{
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }
    }
}