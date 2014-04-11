using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			Debug.Log("hit enemy");
			other.gameObject.GetComponent<EnemyController>().TakeDamage(1, Mathf.Sign(other.transform.position.x - transform.position.x));
		}
	}
}
