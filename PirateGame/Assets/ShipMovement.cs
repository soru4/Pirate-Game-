using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{

    [SerializeField] float maxSpeed, interp, boundsBuffer, maxAngle;
    [SerializeField] Vector2 bounds;
    float realVelocity;
    float targetVelocity;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horiz = -Input.GetAxisRaw("Horizontal") * maxSpeed;
        bool keyInputs = Input.GetAxisRaw("Horizontal") != 0;
        targetVelocity += horiz * Time.deltaTime;

        if (!keyInputs)
            targetVelocity = Mathf.Lerp(targetVelocity, 0, interp * Time.deltaTime);

        targetVelocity = Mathf.Clamp(targetVelocity, -maxSpeed, maxSpeed);

        bool edgeRStopping = targetVelocity < 0 && Mathf.Abs(transform.position.z - bounds[0]) < boundsBuffer,
             edgeLStopping = targetVelocity > 0 && Mathf.Abs(transform.position.z - bounds[1]) < boundsBuffer;

        if (edgeRStopping || edgeLStopping)
            targetVelocity = 0;

        realVelocity = Mathf.Lerp(realVelocity, targetVelocity, interp * Time.deltaTime);

        transform.position += realVelocity * Time.deltaTime * Vector3.forward;
        transform.eulerAngles = Vector3.up * (-realVelocity * maxAngle / maxSpeed - 90);
    }
}
