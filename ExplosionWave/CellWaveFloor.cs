using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWaveFloor : ObjectWave {

	public Renderer rend;
	public LocalOffsetValue offset;
    public CustomPunch punch;

    private bool blockAmplitude;

	public override void IniWave(float Time,float amplitude,Vector3 pos, float maxAmplitude){
        if (amplitude < 0 || blockAmplitude){
            offset.amplitude = 0;
            punch.amplitude = 0;
        }else{
            offset.amplitude += amplitude;
            offset.amplitude = Mathf.Clamp(offset.amplitude, 0, maxAmplitude);
            punch.amplitude += amplitude;
            punch.amplitude = Mathf.Clamp( punch.amplitude, 0, maxAmplitude);
            base.IniWave (Time, amplitude,pos,maxAmplitude);
        }
	}

    public override void PlayAnim(){
        punch.Punch();
        base.PlayAnim();
    }

    public void ResetAmplitude(){
        offset.SetAmplitude(0);
        punch.amplitude = 0;
        blockAmplitude = false;
    }
}
