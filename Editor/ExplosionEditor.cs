using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Explosion))]
public class ExplosionEditor : Editor {

	private static Explosion aux;
	
	public override void OnInspectorGUI () {
		aux = (Explosion)target;
		
		GUILayout.BeginHorizontal();
		if (Application.isPlaying) {
			if ( GUILayout.Button("Explode") )
				aux.Explode();
		} else {
			if ( GUILayout.Button("Explode") )
				Explode(aux);
			if ( GUILayout.Button("Restore") )
				Restore(aux);
		}
		GUILayout.EndHorizontal();

		base.OnInspectorGUI ();
	}

	public void Explode (Explosion aux) {
		for (int i=0; i<aux.activate.Length; i++) {
			aux.activate[i].SetActive(true);
		}
		for (int i=0; i<aux.deactivate.Length; i++) {
			aux.deactivate[i].SetActive(false);
		}
	}

	public void Restore (Explosion aux) {
		for (int i=0; i<aux.activate.Length; i++) {
			aux.activate[i].SetActive(false);
		}
		for (int i=0; i<aux.deactivate.Length; i++) {
			aux.deactivate[i].SetActive(true);
		}
	}
}
