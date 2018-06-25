using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionWave : MonoBehaviour {

	public AnimationCurve curveAmplitude;
	public AnimationCurve curveDistance;
	public float timeLenght=1,amplitude=1;
	public LayerMask cellfloor;
	public SphereCollider col;


	void Start(){
        Destroy(gameObject, timeLenght * 3);
		Collider[] cols = Physics.OverlapSphere (transform.position, col.radius, cellfloor);
		foreach (var item in cols) {
			ObjectWave c = item.GetComponent<ObjectWave> ();
			if (c) {
				float dis = Vector3.Distance (transform.position, c.transform.position);
				dis = Mathf.InverseLerp (0, col.radius, dis);
                c.IniWave (curveDistance.Evaluate(dis)*timeLenght, curveAmplitude.Evaluate(dis)* amplitude,transform.position,amplitude);
			}
		}
	}
}
