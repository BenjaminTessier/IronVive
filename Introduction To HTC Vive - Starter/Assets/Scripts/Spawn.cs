using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public GameObject enemy; 
	private int maxEnemies;
	// Use this for initialization
	void Start () {
        maxEnemies = 20;
		StartCoroutine(Example());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Example() {
		for(int i = 0; i < maxEnemies; i++)
		{
			yield return new WaitForSeconds(5);
			int rdnx = Random.Range (20, 50);
			int rdnz = Random.Range (20, 50);
			GameObject newEnemy = GameObject.Instantiate (enemy);
			newEnemy.transform.position = new Vector3(transform.position.x + rdnx, transform.position.y, transform.position.z + rdnz);
			//Debug.Log(i+" : "+rdnx+" - "+rdnz);
		}
	}
}
