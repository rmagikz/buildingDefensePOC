using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterSound : MonoBehaviour
{
    private float synthGain = 0.0f;
    private double synthIncrement, synthPhase, synthFrequency;

    void OnAudioFilterRead(float[] data, int channels) {
        synthIncrement = synthFrequency * 2.0 * Mathf.PI / 48000.0;
        for (int i = 0; i < data.Length; i+=channels) {
            synthPhase += synthIncrement;
            data[i] = (float) (synthGain * Mathf.Sin((float) synthPhase));

            if (channels == 2) {
                data[i+1] = data[i];
            }

            if (synthPhase > Mathf.PI * 2) {
                synthPhase = 0.0;
            }
        }
    }

    public void minigunSpin(float spin, int min, int max) {
        synthGain = spin * 0.1f + (1 - spin) * 0.0f;
        synthFrequency = spin * max + (1 - spin) * min;
    }

    public void ResetGain() {
        synthGain = 0;
    }
}
