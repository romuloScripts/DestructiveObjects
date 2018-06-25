using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public struct ExplosionInfo{
	public float time, damage, mass;
	public Vector3 posExplosion;
	public GameObject owner;
}

public class ExplosionTimer : MonoBehaviour {

	public ExplosionInfo info;
	public ExplosionEvent onExplosion;

	[System.Serializable]
	public class ExplosionEvent: UnityEvent<ExplosionInfo>{} 
	
	public void SetExplosion(float time, Vector3 posExplosion, float mass , GameObject owner, float damage){
		info.damage = damage;
		info.time = time;
		info.owner = owner;
		info.mass = mass;
		StartCoroutine(Reach());
	}

	public void SetExplosion(){
		StartCoroutine(Reach());
	}
	
	IEnumerator Reach(){
		yield return new WaitForSeconds(info.time);
		onExplosion.Invoke(info);
		Destroy(this);
	}
}
