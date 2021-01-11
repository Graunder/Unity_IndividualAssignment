using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    public int move = 3;
    public float jumpHeight = 0.4f;
    public float jumpVelocity = 4.5f;
    public float moveSpeed = 2;
    public bool moving = false;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    float halfHeight = 0;
    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingToEdge = false;
    Vector3 jumpTarget;

    public bool turn = false;

    protected void Init() {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    public void FindSelectableTile() {
        ComputeAdjacentLists();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.vistited = true;

        while (process.Count > 0) {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move) {
                foreach (Tile tile in t.adjacenctList) {
                    if (!tile.vistited) {
                        tile.parent = t;
                        tile.vistited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void GetCurrentTile() {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target) {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, - Vector3.up, out hit, 1)) {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
        //return null;
    }

    public void ComputeAdjacentLists() {
        foreach (GameObject tile in tiles) {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbours(jumpHeight);
        }
    }

    public void MoveToTile(Tile tile) {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null) {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move() {
        if (path.Count > 0) {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f) {
                bool jump = transform.position.y != target.y;

                if (jump) {
                    Jump(target);
                } else {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
                

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            } else {
                transform.position = target;
                path.Pop();
            }
        } else {
            RemoveSelectableTiles();
            moving = false;

            TurnManager.EndTurn();
        }
    }

    protected void RemoveSelectableTiles() {
        if (currentTile != null) {
            currentTile.current = false;
            currentTile = null;
        }
        
        foreach (Tile tile in selectableTiles) {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target) {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity() {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target) {
        if (fallingDown) {
            FallDownward(target);
        }else if (jumpingUp) {
            JumpUpward(target);
        }else if (movingToEdge) {
            MoveToEdge();
        } else {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target) {
        float targetY = target.y;
        target.y = transform.position.y;
        CalculateHeading(target);

        if (transform.position.y > targetY) {
            fallingDown = false;
            jumpingUp = false;
            movingToEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2f;
        } else {
            fallingDown = false;
            jumpingUp = true;
            movingToEdge = false;

            velocity = heading * moveSpeed / 3f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2f);
        }
    }

    void FallDownward(Vector3 target) {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y) {
            fallingDown = false;
            jumpingUp = false;
            movingToEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target) {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y) {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge() {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f) {
            SetHorizontalVelocity();
        } else {
            movingToEdge = false;
            fallingDown = true;

            velocity /= 5f;
            velocity.y = 1.5f;
        }
    }

    public void BeginTurn() {
        turn = true;
    }

    public void EndTurn() {
        turn = false;
    }
}
