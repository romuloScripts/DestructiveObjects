using UnityEngine;
using System.Collections;

public class Explosion : DestructiveObj{

	public ForcaExterna forcaExterna;
	public float mass=10;
	[Range(0,5)]
	public float speed = 1;
	public const float airPressure = 50f;
	
	[System.Serializable]
	public class ForcaExterna {
		public Vector3 pExplosao;
		public float massa;
	}

	private Vector3 posExplosion{
		get{return transform.position+selfPower.offSet;}
	}

	protected override void DestructionPower(){
		if (selfPower.use) {
			DestructionEffect();
			ExplodeOthers();		
		}
	}

	protected override void SelfDestruction(){

		for (int i=0; i<activate.Length; i++) {
			if (activate[i]!=null){
				activate[i].SetActive(true);
				Rigidbody[] rigdbodies = activate[i].GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody rb in rigdbodies) {
					if(selfPower.use){
						GameObject g = new GameObject();
						g.transform.position = rb.transform.position + Random.insideUnitSphere;
						float z = CalcZ(GetDistance(posExplosion,g.transform),mass);
						Vector3 explosionForce = CalcExplosionForce(posExplosion,g.transform,z);
						rb.AddForceAtPosition(explosionForce,transform.position);	
						Destroy(g);
					}else{
						float z = CalcZ(GetDistance(forcaExterna.pExplosao,rb),forcaExterna.massa);
						Vector3 explosionForce = CalcExplosionForce(forcaExterna.pExplosao,rb,z);
						rb.AddForceAtPosition(explosionForce,forcaExterna.pExplosao);	
					}
				}
			}
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

	private void ExplodeOthers(){
		if (!selfPower.destructivePower) return;
		colliders = Physics.OverlapSphere(transform.position + selfPower.offSet, selfPower.radius, selfPower.layer);
		Collider selfCollider = GetComponent<Collider>();
		foreach (Collider hit in colliders) {
			ExplosionTimer timer = hit.gameObject.GetComponent<ExplosionTimer>();
			if (!timer || !hit || hit == selfCollider || WallDetection(hit)) continue;
			float z = CalcZ(GetDistance(posExplosion,hit.bounds.center),mass);
			float explosionTime =z*speed;
			timer.SetExplosion(explosionTime,posExplosion,mass,gameObject,selfPower.damage);		
		}
	}

	bool WallDetection(Collider col){
		Ray ray = new Ray(posExplosion,(col.bounds.center - posExplosion).normalized);
		RaycastHit[] hits = Physics.RaycastAll(ray,Vector3.Distance(col.bounds.center,posExplosion),selfPower.layer);
		foreach (var item in hits) {
			if(item.collider.gameObject.isStatic){
				return true;
			}
		}
		return false;
	}

	//scientific mathematical formulas for explosion (TNT based)

	public static float CalcZ(float dist, float mass){	
		return dist/Mathf.Pow(mass,1f/3f);
	}

	public static Vector3 CalcExplosionForce(Vector3 pExplosion, Rigidbody rigd, float z){	
		return GetExplosionForce(rigd.gameObject,z) * (rigd.transform.TransformPoint(rigd.centerOfMass) - pExplosion).normalized;
	}
	
	public static Vector3 CalcExplosionForce(Vector3 pExplosion, Transform trans, float z){
		return GetExplosionForce(trans.gameObject,z) * (trans.position - pExplosion).normalized;
	}
	
	private static float GetExplosionForce(GameObject obj, float z){
		
		//scientific mathematical formulas for explosion
		float excessPressure = 808f*(1f+(Mathf.Pow(z/4.5f,2)))/ 
			(Mathf.Sqrt(1f+(Mathf.Pow(z/0.048f,2))) 
			 * Mathf.Sqrt(1f+(Mathf.Pow(z/0.32f,2))) 
			 * Mathf.Sqrt(1f+(Mathf.Pow(z/1.35f,2))));
		excessPressure *= airPressure;
		
		//contact field
		ContactField cf = obj.GetComponent<ContactField>();
		float contactField=0;
		if(cf){
			contactField = cf.CalcArea();
		}else{
			contactField = obj.transform.localScale.x*0.5f;
			contactField = Mathf.PI * contactField * contactField;
		}
		// return explosionForce;
		return excessPressure* contactField;	
	}

	public static float GetDistance(Vector3 pExplosion, Rigidbody rigd){
		return Vector3.Distance(pExplosion,rigd.transform.TransformPoint(rigd.centerOfMass));
	}
	
	public static float GetDistance(Vector3 pExplosion, Vector3 pos){
		return Vector3.Distance(pExplosion,pos);
	}
	
	public static float GetDistance(Vector3 pExplosion, Transform trans){
		return Vector3.Distance(pExplosion,trans.position);
	}
}
