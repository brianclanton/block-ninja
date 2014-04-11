using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public GameObject blockNinja;

	void Start() {
		Physics.IgnoreCollision(collider, blockNinja.collider);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyController>().TakeDamage(1, Mathf.Sign(other.transform.position.x - transform.position.x));
		}
	}
}
