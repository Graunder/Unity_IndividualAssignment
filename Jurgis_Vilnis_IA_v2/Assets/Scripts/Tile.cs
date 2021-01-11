using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool walkable = true;

    public List<Tile> adjacenctList = new List<Tile>();

    //
    public bool vistited = false;
    public Tile parent = null;
    public int distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (current) {
            GetComponent<Renderer>().material.color = Color.magenta;
        } else if(target){
            GetComponent<Renderer>().material.color = Color.blue;
        }else if (selectable) {
            GetComponent<Renderer>().material.color = new Color(0.1f, 0.4f, 0.9f, 1f);
        } else {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void FindNeighbours(float jumpheight) {
        Reset();
        CheckTile(Vector3.forward, jumpheight);
        CheckTile(-Vector3.forward, jumpheight);
        CheckTile(Vector3.right, jumpheight);
        CheckTile(-Vector3.right, jumpheight);

    }

    public void Reset() {
        current = false;
        target = false;
        selectable = false;
        vistited = false;
        parent = null;
        distance = 0;
        adjacenctList.Clear();
    }

    public void CheckTile(Vector3 direction, float jumpHeight) {

        Vector3 halfExtends = new Vector3(0.25f, (1 * jumpHeight) / 2f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);

        foreach (Collider item in colliders) {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable) {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1)) {
                    adjacenctList.Add(tile);
                }
            }
        }
    }
}
