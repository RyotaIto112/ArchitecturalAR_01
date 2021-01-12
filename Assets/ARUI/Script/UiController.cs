using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace ARUI {
    public class UiController : MonoBehaviour {
        [SerializeField] protected ARPlaneManager planeManager;
        [SerializeField] protected List<UiInitializer> initializers;
        [SerializeField] protected Button animStartBtn;
        [SerializeField] protected Button animStopBtn;
        [SerializeField] protected UnityEvent animationResume;
        [SerializeField] protected UnityEvent animationPause;
        public bool isAnyUiActive { get; private set; }
        public static OparateState currOpState;

        void Start () {
            InitializeAll ();
        }

        void Update () {
            isAnyUiActive = initializers.Any (i => i.IsActive ());
            foreach (var p in planeManager.trackables) p.gameObject.SetActive (debugPlaneState);
        }

        public void InitializeAll () {
            if (initializers == null) initializers = new List<UiInitializer> ();
            if (initializers.Count == 0) initializers = GetComponentsInChildren<UiInitializer> ().ToList ();
            initializers.ForEach (i => i.Initialize ());
        }

        bool debugPlaneState = true;
        public void TogglePlaneDetection () => debugPlaneState = !debugPlaneState;
        public void Debugger (string text) { Debug.Log (text); }
        public void AnimResume () { animationResume.Invoke (); AnimToggle(false); }
        public void AnimPause ()  { animationPause.Invoke (); AnimToggle(true); }

        void AnimToggle (bool flag) {
            animStartBtn.gameObject.SetActive (flag);
            animStopBtn.gameObject.SetActive (!flag);
            animStartBtn.interactable = flag;
            animStopBtn.interactable = !flag;
        }

    }

    public enum OparateState { Spawn, Position, Rotation }

    public interface UiInitializer {
        bool Initialize ();
        bool IsActive ();
    }
}