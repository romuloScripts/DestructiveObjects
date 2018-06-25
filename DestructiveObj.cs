using UnityEngine;
using System.Collections;

public class DestructiveObj: MonoBehaviour {

	[System.Serializable]
	public class SelfPower {
		public bool use = false;
		public Vector3 offSet = Vector3.zero;
		public float radius = 1f;
		public float force = 1f;
		public LayerMask layer;
		public GameObject prefab;

		public bool destructivePower = false;
		public float damage = 0f;
		public float delay = 0f;
	}

	public bool externalForces = true;
	public bool disengage = false;
	public float destroyTime = 3f;
	public SelfPower selfPower;

	public GameObject[] activate;
	public GameObject[] deactivate;
	public GameObject[] destroy;
	public Rigidbody[] activeRigd;
	
	protected bool destructed = false;
	protected Collider[] colliders;

	public bool Explode() {
		if (destructed)
			return false;

		if (selfPower.use) {
			if (selfPower.destructivePower && selfPower.delay>0f) {
				Invoke("Destruction", Random.Range(selfPower.delay*0.5f, selfPower.delay));
			} else {
				Destruction();
			}
			return true;
		} else if (externalForces) {
			Destruction();
			return true;
		}
		return false;
	}


	void Destruction() {
		if (destructed)
			return;
		destructed = true;

		Disengage();
		SelfDestruction();
		DestructionPower();
	}

	protected virtual void DestructionPower(){
		if (selfPower.use){
			DestructionEffect();
			DamageOthers();
			AddExplosionForce();
		}
	}

	protected virtual void Disengage(){ if(disengage) transform.parent = null;}

	protected virtual void SelfDestruction(){
		for (int i=0; i<activate.Length; i++) {
			if (activate[i]!=null)
				activate[i].SetActive(true);
		}
		for (int i=0; i<deactivate.Length; i++) {
			deactivate[i].SetActive(false);
		}
		for (int i=0; i<destroy.Length; i++) {
			if (destroy[i]!=null)
				Destroy(destroy[i], destroyTime);
		}
		for (int i=0; i<activeRigd.Length; i++) {
			if (activeRigd[i]!=null)
				activeRigd[i].isKinematic = false;
		}
	}

	protected void DestructionEffect(){
		if (selfPower.prefab!=null)
				Instantiate(selfPower.prefab, transform.position + selfPower.offSet, selfPower.prefab.transform.rotation);
	}

	void DamageOthers(){
		if (selfPower.destructivePower) {
			colliders = Physics.OverlapSphere(transform.position + selfPower.offSet, selfPower.radius, selfPower.layer);
			foreach (Collider hit in colliders) {
				if (!hit) continue;
					//TO DO apply forces and damage
			}
		}
	}

	void AddExplosionForce(){
		colliders = Physics.OverlapSphere (transform.position + selfPower.offSet, selfPower.radius, selfPower.layer);
		foreach (Collider hit in colliders) {
			if (!hit) continue;

			if (hit.GetComponent<Rigidbody>()) {
				hit.GetComponent<Rigidbody>().AddExplosionForce(selfPower.force, transform.position+selfPower.offSet, selfPower.radius);
			}
		}
	}

	void OnDrawGizmosSelected () {
		if (!selfPower.use) return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position + selfPower.offSet, selfPower.radius);
	}
}
