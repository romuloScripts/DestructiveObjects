using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWave : MonoBehaviour {

	public virtual void IniWave(float Time,float amplitude,Vector3 pos,float maxAmplitude){
		Invoke ("PlayAnim", Time);
	}

	public virtual void PlayAnim(){
		
	}
}
