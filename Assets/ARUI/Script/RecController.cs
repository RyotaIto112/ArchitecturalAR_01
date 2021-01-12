using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using pmjo.NextGenRecorder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ARUI {
    public class RecController : MonoBehaviour {

        [SerializeField] protected Button startRecordingButton;
        [SerializeField] protected Button stopRecordingButton;
        public bool isRecording;
        //[SerializeField] protected Button saveRecordingButton;
        //[SerializeField] protected Button viewRecordingButton;

        long mLastSessionId;

        void OnEnable () {
            Recorder.RecordingStarted += RecordingStarted;
            Recorder.RecordingPaused += RecordingPaused;
            Recorder.RecordingResumed += RecordingResumed;
            Recorder.RecordingStopped += RecordingStopped;
            Recorder.RecordingExported += RecordingExported;
        }

        void OnDisable () {
            Recorder.RecordingStarted -= RecordingStarted;
            Recorder.RecordingPaused -= RecordingPaused;
            Recorder.RecordingResumed -= RecordingResumed;
            Recorder.RecordingStopped -= RecordingStopped;
            Recorder.RecordingExported -= RecordingExported;
        }

        void Awake () {
            mLastSessionId = Recorder.GetLastRecordingSession ();
            UpdateStartAndStopRecordingButton ();
            //UpdateSaveOrViewRecordingButton ();
        }

        void Start () { CreateEventSystemIfItDoesNotExist (); }

        void RecordingStarted (long sessionId) {
            UpdateStartAndStopRecordingButton ();
            //UpdateSaveOrViewRecordingButton ();
        }

        void RecordingPaused (long sessionId) { }
        void RecordingResumed (long sessionId) { }

        void RecordingStopped (long sessionId) {
            mLastSessionId = sessionId;
            UpdateStartAndStopRecordingButton ();
            //UpdateSaveOrViewRecordingButton ();
            if (mLastSessionId > 0) Recorder.ExportRecordingSession (mLastSessionId);
        }

        public void StartRecording () {isRecording = true; Recorder.StartRecording (); }
        public void StopRecording () {isRecording = false; Recorder.StopRecording (); }

        void ExportLastRecording () {
            if (mLastSessionId > 0) Recorder.ExportRecordingSession (mLastSessionId);
        }

        void RecordingExported (long sessionId, string path, Recorder.ErrorCode errorCode) {
            if (errorCode == Recorder.ErrorCode.NoError) {
                Debug.Log ("Recording exported to " + path + ", session id " + sessionId);
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                CopyFileToDesktop (path, "MyAwesomeRecording.mp4");
#elif UNITY_IOS ||  UNITY_TVOS
                //PlayVideo (path);
                Debug.Log ("Recording exported to " + path + ", session id " + sessionId);
                NativeGallery.SaveVideoToGallery (path, "Gallery/pmjosNextGenRecorder", "MyAwesomeRecording.mp4");
#endif
            } else {
                Debug.Log ("Failed to export recording, error code " + errorCode + ", session id " + sessionId);
            }
        }

        void UpdateStartAndStopRecordingButton () {
            if (Recorder.IsSupported) {
                startRecordingButton.interactable = true;
                stopRecordingButton.interactable = true;
                startRecordingButton.gameObject.SetActive (!Recorder.IsRecording);
                stopRecordingButton.gameObject.SetActive (Recorder.IsRecording);
            } else {
                startRecordingButton.gameObject.SetActive (true);
                startRecordingButton.interactable = false;
                stopRecordingButton.gameObject.SetActive (false);
            }
        }

        //void UpdateSaveOrViewRecordingButton () {
        //    #if UNITY_EDITOR ||  UNITY_STANDALONE
        //    saveRecordingButton.gameObject.SetActive (true);
        //    saveRecordingButton.interactable = (mLastSessionId > 0) && !Recorder.IsRecording;
        //    viewRecordingButton.gameObject.SetActive (false);
        //    #else
        //    viewRecordingButton.gameObject.SetActive (true);
        //    viewRecordingButton.interactable = (mLastSessionId > 0) && !Recorder.IsRecording;
        //    saveRecordingButton.gameObject.SetActive (false);
        //    #endif
        //}

#if UNITY_EDITOR_OSX ||  UNITY_STANDALONE_OSX
        static void CopyFileToDesktop (string path, string fileName) {
            string desktopPath = Environment.GetFolderPath (Environment.SpecialFolder.Desktop);
            string dstPath = Path.Combine (desktopPath, fileName);
            File.Copy (path, dstPath, true);
            Debug.Log ("Recording " + fileName + " copied to the desktop");
        }

#elif UNITY_IOS ||  UNITY_TVOS
        static void PlayVideo (string path) {
            if (!path.Contains ("file://")) path = "file://" + path;
            Handheld.PlayFullScreenMovie (path);
        }
#endif

        static void CreateEventSystemIfItDoesNotExist () {
            EventSystem eventSystem = FindObjectOfType<EventSystem> ();
            StandaloneInputModule inputModule = FindObjectOfType<StandaloneInputModule> ();
            if (eventSystem == null && inputModule == null) {
                new GameObject ("EventSystem", typeof (EventSystem), typeof (StandaloneInputModule));
            }
        }
    }
}