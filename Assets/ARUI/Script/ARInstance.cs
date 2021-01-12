using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARUI {
    public class ARInstance : MonoBehaviour {

        [SerializeField] protected PlayableDirector[] directors;

        protected bool posActive;
        protected bool rotActive;
        protected bool sclActive;
        protected float sDst;
        protected Vector2 sPos;
        protected Vector3 sScl;
        protected Quaternion sRot;

        float v = 1.0f;
        float W => Screen.width;
        float H => Screen.height;
        float D => Mathf.Sqrt (Mathf.Pow (W, 2) + Mathf.Pow (H, 2));

        void OnEnable () {
            sScl = transform.localScale;
            directors = GetComponentsInChildren<PlayableDirector> ();
        }

        void Initialize () {
            posActive = false;
            rotActive = false;
            sclActive = false;
        }

        void Update () {
            if (posActive) Pos ();
            if (rotActive) Rot ();
            if (sclActive) Scl ();
        }

        public void PausePlayer () { foreach (var d in directors) d.Pause (); }
        public void ResumePlayer () { foreach (var d in directors) d.Resume (); }
        public void StopPlayer () { foreach (var d in directors) d.Stop (); }
        public void PlayPlayer () { foreach (var d in directors) d.Play (); }

        #region Transfrom Control

        public void PosActivate (Touch t) {
            sPos = t.position;
            sRot = transform.rotation;
            posActive = true;
        }

        public void RotActivate (Touch t) {
            sPos = t.position;
            sRot = transform.rotation;
            rotActive = true;
        }

        public void SclActive (Touch t1, Touch t2) {
            sDst = Vector2.Distance (t1.position, t2.position);
            sclActive = true;
        }

        protected List<ARRaycastHit> hitResults = new List<ARRaycastHit> ();
        public ARRaycastManager raycastMgr { get; set; }
        void Pos () {
            if (Input.touchCount == 1) {
                var t = Input.GetTouch (0);
                var p = t.position;
                switch (t.phase) {
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (raycastMgr.Raycast (p, hitResults, TrackableType.PlaneWithinBounds)) {
                            var _p = hitResults[0].pose.position;
                            transform.position = _p;
                        }
                        //var dst = transform.position - arCam.transform.position;
                        //var dpt = Vector3.Dot (dst, arCam.transform.forward);
                        //transform.position = arCam.ScreenToWorldPoint (new Vector3 (p.x, p.y, dpt));
                        break;
                    case TouchPhase.Ended:
                        Initialize ();
                        break;
                }
            }
        }

        void Rot () {
            if (Input.touchCount == 1) {
                var t = Input.GetTouch (0);
                var p = t.position;
                switch (t.phase) {
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        transform.rotation = sRot;
                        transform.Rotate (
                            new Vector3 (90 * (p.y - sPos.y) / H, -90 * (p.x - sPos.x) / W),
                            Space.World);
                        break;
                    case TouchPhase.Ended:
                        Initialize ();
                        break;
                }
            }
        }

        public void Scl () {
            if (Input.touchCount >= 2) {
                Touch t1 = Input.GetTouch (0);
                Touch t2 = Input.GetTouch (1);
                if (
                    (t1.phase == TouchPhase.Moved || t1.phase == TouchPhase.Stationary) &&
                    (t2.phase == TouchPhase.Moved || t2.phase == TouchPhase.Stationary) &&
                    sclActive) {
                    var nDst = Vector2.Distance (t1.position, t2.position);
                    v = v + (nDst - sDst) / D;
                    sDst = nDst;
                    transform.localScale = sScl * v;
                } else if (t1.phase == TouchPhase.Ended || t2.phase == TouchPhase.Ended)
                    Initialize ();
            }
        }
        #endregion
    }
}