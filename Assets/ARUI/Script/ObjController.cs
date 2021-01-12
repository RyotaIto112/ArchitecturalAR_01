using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARUI {
    [RequireComponent (typeof (ARRaycastManager))]
    public class ObjController : MonoBehaviour {
        [SerializeField] protected TrackableType type;
        [SerializeField] protected Marker marker;
        [SerializeField] protected Camera arCam;
        [SerializeField] protected RecController recController;
        [SerializeField] protected List<ARInstance> objectPrefabs;
        protected ARInstance activePrefab;

        protected ARRaycastManager raycastMgr;
        protected List<ARRaycastHit> hitResults = new List<ARRaycastHit> ();
        protected List<ARInstance> instances = new List<ARInstance> ();

        public void Clear () {
            for (int i = instances.Count - 1; i >= 0; i--)
                Destroy (instances[i].gameObject);
            instances.Clear ();
        }

        public void Activate (int id) {
            activePrefab = objectPrefabs[Mathf.Clamp (id, 0, objectPrefabs.Count - 1)];
        }

        void Start () {
            raycastMgr = GetComponent<ARRaycastManager> ();
            Activate (0);
        }

        void Update () {
            switch (UiController.currOpState) {
                case OparateState.Spawn:
                    SpawnCheck ();
                    break;
                case OparateState.Position:
                    marker.Minimize ();
                    PosCheck ();
                    SclCheck ();
                    break;
                case OparateState.Rotation:
                    marker.Minimize ();
                    RotCheck ();
                    SclCheck ();
                    break;
            }
        }

        public void PausePlayer ()  { instances.ForEach (i => i.PausePlayer ());  }
        public void ResumePlayer () { instances.ForEach (i => i.ResumePlayer ()); }
        public void PlayPlayer ()   { instances.ForEach (i => i.PlayPlayer ());   }
        public void StopPlayer ()   { instances.ForEach (i => i.StopPlayer ());   }

        void SpawnCheck () {
            var c = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
            if (raycastMgr.Raycast (c, hitResults, type) && !recController.isRecording) {
                var p = hitResults[0].pose.position;
                marker.transform.position = p;
                marker.Maximize ();
                if (Input.touchCount >= 1) {
                    var t = Input.GetTouch (0);
                    if (t.phase == TouchPhase.Began) {
                        var r = arCam.ScreenPointToRay (t.position);
                        if (Physics.Raycast (r, out RaycastHit h)) {
                            var m = h.transform.GetComponent<Marker> ();
                            if (m != null) {
                                var g = Instantiate (activePrefab, p, Quaternion.identity);
                                g.transform.SetParent (this.transform);
                                g.raycastMgr = raycastMgr;
                                instances.Add (g);
                            }
                        }
                    }
                }
            } else {
                marker.Minimize ();
            }
        }

        void PosCheck () {
            if (Input.touchCount == 1) {
                var t = Input.GetTouch (0);
                if (t.phase == TouchPhase.Began) {
                    var r = arCam.ScreenPointToRay (t.position);
                    if (Physics.Raycast (r, out RaycastHit h)) {
                        var g = h.transform.GetComponent<ARInstance> ();
                        if (g != null) g.PosActivate (t);
                    }
                }
            }
        }

        void RotCheck () {
            if (Input.touchCount == 1) {
                var t = Input.GetTouch (0);
                if (t.phase == TouchPhase.Began) {
                    var r = arCam.ScreenPointToRay (t.position);
                    if (Physics.Raycast (r, out RaycastHit h)) {
                        var g = h.transform.GetComponent<ARInstance> ();
                        if (g != null) g.RotActivate (t);
                    }
                }
            }
        }

        void SclCheck () {
            if (Input.touchCount >= 2) {
                var t1 = Input.GetTouch (0);
                var t2 = Input.GetTouch (1);
                if (t2.phase == TouchPhase.Began) {
                    var r1 = arCam.ScreenPointToRay (t1.position);
                    var r2 = arCam.ScreenPointToRay (t2.position);
                    if (Physics.Raycast (r1, out RaycastHit h1) && Physics.Raycast (r2, out RaycastHit h2)) {
                        var g1 = h1.transform.GetComponent<ARInstance> ();
                        var g2 = h2.transform.GetComponent<ARInstance> ();
                        if (g1 != null) g1.SclActive (t1, t2);
                        if (g2 != null) g2.SclActive (t1, t2);
                    }
                }
            }
        }

    }
}