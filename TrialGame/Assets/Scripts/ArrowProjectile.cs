using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private Rigidbody arrowRigidbody;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 7777f * Time.deltaTime;
        arrowRigidbody.AddForce(transform.forward * speed , ForceMode.Impulse);
    }

    private void Update()
    {
    }
}
