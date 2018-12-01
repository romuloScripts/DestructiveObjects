using UnityEngine;

public class ExplosionWave2D : MonoBehaviour {

	public AnimationCurve curveAmplitude;
	public AnimationCurve curveDistance;
	public float timeLenght=1,amplitude=1;
	public LayerMask cellfloor;
	public float radius;

	void Start(){
		Destroy(gameObject, timeLenght * 3);
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius, cellfloor);
		foreach (var item in cols) {
			ObjectWave c = item.GetComponent<ObjectWave> ();
			if (c) {
				float dis = Vector3.Distance (transform.position, c.transform.position);
				dis = Mathf.InverseLerp (0, radius, dis);
				c.IniWave (curveDistance.Evaluate(dis)*timeLenght, curveAmplitude.Evaluate(dis)* amplitude,transform.position,amplitude);
			}
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.matrix = Matrix4x4.TRS(transform.position,Quaternion.identity,new Vector3(1,1,0));
		Gizmos.DrawWireSphere(Vector3.zero,radius);
	}
}
