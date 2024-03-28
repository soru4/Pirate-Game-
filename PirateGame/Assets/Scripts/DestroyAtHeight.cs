using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtHeight : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] GameObject particle;
    [SerializeField] Vector3 offset;

    void Update()
    {
        if (transform.position.y < Boyancy.getHeight(transform.position))
        {
            Destroy(gameObject);
            Destroy(Instantiate(particle, transform.position + offset, Quaternion.Euler(-90, 0, 0)), 1f);
        }
    }
}
