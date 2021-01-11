using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{

    private Grid grid;
    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {
        grid = new Grid(5, 5, 10f, new Vector3(0, 0, 0));
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0)) {
            //grid.SetValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                grid.SetValue(hit.point, 1);
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            //Debug.Log(grid.GetValue(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(grid.GetValue(hit.point));
            }
        }

    }
}
