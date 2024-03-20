using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector3 fireOrigin, mouseWorldPos;
    [SerializeField] Transform cannon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
        Physics.Raycast(origin, pos - origin, out RaycastHit hit);
        mouseWorldPos = hit.point;

        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mouseWorldPos, 1);
    }
}
