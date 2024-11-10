using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

namespace SetVol
{
    public class SetVolume : MonoBehaviour
    {
        // Start is called before the first frame update
        public AudioMixer mixer;
        public Slider sld;
        public string mixerName;

        private int volVal;
        private float volValCalc;


        public void setVol(float sliderVal)
        {
            mixer.SetFloat(mixerName, Mathf.Log10(sliderVal) * 20);


        }

        private void Update()
        {
            volValCalc = sld.value * 100;
            volVal = (int)volValCalc;

        }
    }
}
