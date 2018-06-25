using UnityEngine;
using System.Collections;

public class ContactField : MonoBehaviour {

	[System.Serializable]
	public class Field{
		public float radius=1;
		public Vector3 pos;
	}
	public Field[] fields = new Field[0];
	
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.black;
		foreach (var item in fields) {
			if(GetComponent<Rigidbody>()){
				Gizmos.DrawWireSphere(transform.TransformPoint(GetComponent<Rigidbody>().centerOfMass)+ item.pos,item.radius);
			}else{
				Gizmos.DrawWireSphere(transform.position + item.pos,item.radius);
			}
		}
	}
	
	public float CalcArea(){
		float sum =0;
		foreach (var item in fields) {
			sum += Mathf.PI * item.radius * item.radius;
		}
		return sum;
	}
}
