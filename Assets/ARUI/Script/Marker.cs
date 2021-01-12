using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARUI {
    public class Marker : MonoBehaviour {
        protected Vector3 initScale;

        void OnEnable() {
            initScale = transform.localScale;
        }
        
        public void Minimize(){
            transform.localScale = Vector3.zero;
        }

        public void Maximize(){
            transform.localScale = initScale;
        }
    }
}