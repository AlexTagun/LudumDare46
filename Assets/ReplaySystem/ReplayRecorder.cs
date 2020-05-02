using UnityEngine;
using FFmpegOut;
using System.Collections.Generic;

namespace GameReplay
{
    [AddComponentMenu("GameReplay/ReplayRecorder")]
    public sealed class ReplayRecorder : MonoBehaviour
    {
        public void startNewRecording() {
            createFolderToSaveReplaysIfNo();
            _recordingStates.Add(createRecordingState());

            ++_currentCaptureIndexForCaptureFileName;
        }

        private void createFolderToSaveReplaysIfNo() {
            System.IO.Directory.CreateDirectory(folderToSaveCaptures);
        }

        private RecordingState createRecordingState() {
            return new RecordingState(replayRecordingCamera, pathToSaveCurrentCapture, _frameRate, _preset);
        }

        public System.Collections.IEnumerator stopLastRecording(System.Action<Replay> resultCallback) {
            int theCount = _recordingStates.Count;
            int theIndexToStop = theCount - 1;
            if (0 == theCount)
                throw(new System.Exception("Incorrect stopping of recording"));
            RecordingState theStateToStop = _recordingStates[theIndexToStop];
            if (null == theStateToStop || theStateToStop.isClosing)
                throw (new System.Exception("Incorrect stopping of recording"));

            yield return theStateToStop.closeRecordingSession(resultCallback);
            
            _recordingStates.RemoveAt(theIndexToStop);
        }

        public bool isRecording =>
                _recordingStates.Count > 0 && !_recordingStates[_recordingStates.Count - 1].isClosing;

        private class RecordingState {
            public RecordingState(Camera inCamera, string inPathToSaveResult, float inFrameRate, FFmpegPreset inPreset) {
                tempRT = createCaptureTexture(inCamera);
                inCamera.targetTexture = tempRT;
                blitter = Blitter.CreateInstance(inCamera);

                int width = Mathf.Max(8, inCamera.pixelWidth) / 2 * 2;
                int height = Mathf.Max(8, inCamera.pixelHeight) / 2 * 2;

                frameRate = inFrameRate;

                session = FFmpegSession.CreateWithOutputPath(inPathToSaveResult, width, height, inFrameRate, inPreset);

                startTime = Time.time;
                frameCount = 0;
                frameDropCount = 0;

                _camera = inCamera;

                _pathToSaveResult = inPathToSaveResult;
            }

            public void update() {
                float frameTime = startTime + (frameCount - 0.5f) / frameRate;
                float gap = Time.time - frameTime;
                float delta = 1f / frameRate;

                if (gap < 0) {
                    session.PushFrame(null);
                } else if (gap < delta) {
                    session.PushFrame(_camera.targetTexture);
                    frameCount++;
                } else if (gap < delta * 2) {
                    session.PushFrame(_camera.targetTexture);
                    session.PushFrame(_camera.targetTexture);
                    frameCount += 2;
                } else {
                    if (++frameDropCount == 10) {
                        Debug.LogWarning(
                            "Significant frame droppping was detected. This may introduce " +
                            "time instability into output video. Decreasing the recording " +
                            "frame rate is recommended."
                        );
                    }

                    session.PushFrame(_camera.targetTexture);
                    frameCount += Mathf.FloorToInt(gap * frameRate);
                }
            }

            public System.Collections.IEnumerator closeRecordingSession(System.Action<Replay> inResultCallback) {
                if (_closing)
                    yield break;
                _closing = true;

                yield return session.AsyncClose();
                session.Dispose();
                _camera.targetTexture = null;
                Destroy(tempRT);
                Destroy(blitter);

                inResultCallback(new Replay(_pathToSaveResult));
            }

            public bool isClosing => _closing;

            static private RenderTexture createCaptureTexture(Camera inCamera) {
                int width = Mathf.Max(8, inCamera.pixelWidth) / 2 * 2;
                int height = Mathf.Max(8, inCamera.pixelHeight) / 2 * 2;
            
                RenderTextureFormat theFormat = inCamera.allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
                var theResult = new RenderTexture(width, height, 24, theFormat);
                theResult.antiAliasing = inCamera.allowMSAA ? QualitySettings.antiAliasing : 1;
                return theResult;
            }

            //Fields
            public FFmpegSession session = null;
            public RenderTexture tempRT = null;
            public GameObject blitter = null;

            public int frameCount = 0;
            public float startTime = 0;
            public int frameDropCount = 0;
            public float frameRate = 1;
            public string _pathToSaveResult = null;

            private Camera _camera = null;
            private bool _closing = false;
        }

        private void Update() {
            foreach (RecordingState theRecordingState in _recordingStates)
                theRecordingState.update();
        }

        private void OnDisable() {
            foreach (RecordingState theRecordingState in _recordingStates)
                theRecordingState.closeRecordingSession((Replay unused)=>{});
        }

        private Camera replayRecordingCamera { get { return _replayRecordingCamera; } }

        private static string folderToSaveCaptures => Application.persistentDataPath + "/" + "GameReplay"; 
        private string pathToSaveCurrentCapture =>
                folderToSaveCaptures + "/" + "replayCapture" + _currentCaptureIndexForCaptureFileName + _preset.GetSuffix();

        //Fields
        private List<RecordingState> _recordingStates = new List<RecordingState>();
        private int _currentCaptureIndexForCaptureFileName = 0;

        [SerializeField] private Camera _replayRecordingCamera = null;
        [SerializeField] private FFmpegPreset _preset = new FFmpegPreset();
        [SerializeField] private float _frameRate = 60;

        //NB: This code was in original implementation, maybe it will be needed in future
        //IEnumerator Start() { for (var eof = new WaitForEndOfFrame(); ;) { yield return eof; _session?.CompletePushFrames(); } }
    }
}

