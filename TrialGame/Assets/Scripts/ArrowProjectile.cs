using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private Rigidbody arrowRigidbody;
    private BoxCollider arrowCollider;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        arrowCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        float speed = 7777f * Time.deltaTime;
        //float speed = 50f;
        arrowRigidbody.AddForce(transform.forward * speed , ForceMode.Impulse);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //arrowCollider.isTrigger = false;
        //arrowRigidbody.useGravity = false;
        //arrowRigidbody.velocity = Vector3.zero;
        arrowRigidbody.isKinematic = true;
        transform.parent = other.transform;
        arrowCollider.isTrigger = false;
        //Destroy(arrowRigidbody);

        if (other.GetComponent<ArrowTarget>() != null)
        {
            Debug.Log("Target hit");
        }
        else
        {
            Debug.Log("Miss");
        }
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        arrowRigidbody.isKinematic = true;
        arrowCollider.isTrigger = true;
        transform.parent = collision.transform;

        if (collision.gameObject.GetComponent<ArrowTarget>() != null)
        {
            Debug.Log("Target hit");
            
            if(collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 0.1f, ForceMode.Impulse);
            }
        }
        else
        {
            Debug.Log("Miss");
        }
    }
}
