using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector3 mouseWorldPos;
    [SerializeField] Transform fireOrigin, cannonBase, cannon;
    [SerializeField] GameObject cannonBall;
    [SerializeField] float velocity;
    float vInvSqr;

    Vector3 v0;

    // Start is called before the first frame update
    void Start()
    {
        vInvSqr = 1 / (velocity * velocity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
        Physics.Raycast(origin, pos - origin, out RaycastHit hit);
        mouseWorldPos = hit.point;

        //if (Input.GetMouseButtonDown(0))
        //{
            Click();
        //}
    }

    void Click()
    {
        Vector3 flatFP = new (fireOrigin.position.x, 0, fireOrigin.position.z);
        Vector3 flatMP = new (mouseWorldPos.x, 0, mouseWorldPos.z);
        float theta = Vector3.SignedAngle(flatMP - flatFP + Vector3.right, Vector3.right, Vector3.up) + 180;

        float dist = Vector3.Distance(flatFP, flatMP);
        float height = fireOrigin.position.y - mouseWorldPos.y;
        float g = Physics.gravity.y;
        print(dist + "   " + height);

        float a = -g * dist * dist * 0.5f * vInvSqr;
        // b = dist
        // c = height + a

        float tangent = Quad(a, dist, height + a);
        float phi = 90 - Mathf.Atan(tangent) * Mathf.Rad2Deg;
        print(theta + "  |   " + phi);

        cannonBase.localEulerAngles = new Vector3(0, 180-theta, 0);
        cannon.localEulerAngles = new Vector3(180 - phi, 0, 0);

        theta *= Mathf.Deg2Rad; phi *= Mathf.Deg2Rad;

        v0 = velocity * new Vector3(
            Mathf.Cos(theta) * Mathf.Cos(phi),
            //Mathf.Sin(phi),
            0,
            Mathf.Sin(theta) * Mathf.Cos(phi)
            
        );

        //print(v0);
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody ball = Instantiate(cannonBall, fireOrigin.position, Quaternion.identity).GetComponent<Rigidbody>();
            ball.velocity = v0;
            Destroy(ball.gameObject, 5f);
        }
        float Quad(float a, float b, float c)
        {
            print(b * b - 4 * a * c);
            return b * b - 4 * a * c > 0 ? (-b - Mathf.Sqrt(b * b - 4 * a * c)) / 2 * a : 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mouseWorldPos, 1);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(fireOrigin.position + v0, 0.75f);
    }
}
