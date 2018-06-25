using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(DestructiveObj))]
public class DestructiveObjEditor : Editor {

	private static DestructiveObj aux;

	public override void OnInspectorGUI () {
		aux = (DestructiveObj)target;

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

	public void Explode (DestructiveObj aux) {
		for (int i=0; i<aux.activate.Length; i++) {
			aux.activate[i].SetActive(true);
		}
		for (int i=0; i<aux.deactivate.Length; i++) {
			aux.deactivate[i].SetActive(false);
		}
	}

	public void Restore (DestructiveObj aux) {
		for (int i=0; i<aux.activate.Length; i++) {
			aux.activate[i].SetActive(false);
		}
		for (int i=0; i<aux.deactivate.Length; i++) {
			aux.deactivate[i].SetActive(true);
		}
	}

}
