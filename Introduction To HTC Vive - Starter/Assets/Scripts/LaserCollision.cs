using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollision : MonoBehaviour {

	// Use this for initialization
	void OnCollisionEnter (Collision collision) {

        //Debug.Log("touché !");
        //if (collision.gameObject.tag == "pray"){
        //	Destroy(collision.gameObject);
        //	Destroy(gameObject);
        //	Debug.Log ("pray tag");
        //}
        Destroy(gameObject);
    }

    void Awake()
    {
        //StartCoroutine(Example());
    }
    
 //   IEnumerator Example() {
	//	yield return new WaitForSeconds(1);
	//	Destroy(gameObject);
	//}
}




