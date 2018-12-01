using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWaveFloor : ObjectWave {

	public Renderer rend;
	public LocalOffsetValue offset;
    public CustomPunchBase punch;

    private bool blockAmplitude;

	public override void IniWave(float Time,float amplitude,Vector3 pos, float maxAmplitude){
        if (amplitude < 0 || blockAmplitude){
            if (offset)
                offset.amplitude = 0;
            if (punch)
                punch.amplitude = 0;
        }else{
            if (offset)
            {
                offset.amplitude += amplitude;
                offset.amplitude = Mathf.Clamp(offset.amplitude, 0, maxAmplitude);
            }

            if (punch)
            {
                punch.amplitude += amplitude;
                punch.amplitude = Mathf.Clamp(punch.amplitude, 0, maxAmplitude);
            }

            base.IniWave (Time, amplitude,pos,maxAmplitude);
        }
	}

    public override void PlayAnim(){
        if (punch)
            punch.Punch();
        base.PlayAnim();
    }

    public void ResetAmplitude(){
        if (offset)
            offset.SetAmplitude(0);
        if (punch)
            punch.amplitude = 0;
        blockAmplitude = false;
    }
}
