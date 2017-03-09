using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{

    private int health;

    private Transform target;
    private int minDistance;
    
    // Use this for initialization
    void Start()
    {
        minDistance = 10;
        health = 100;
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0 || health < 0)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position, target.position) > minDistance)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * Time.deltaTime);
            //transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 10F);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("ouch : " + collision.gameObject.tag);
        if (collision.gameObject.tag == "laser")
        {
            health = health - 20;
            //Debug.Log("vie restante : " + health.ToString());
        }
    }
}
