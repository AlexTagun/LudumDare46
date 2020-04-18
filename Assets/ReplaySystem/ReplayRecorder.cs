using UnityEngine;
using FFmpegOut;
using UnityEngine.Rendering;

namespace GameReplay
{
    [AddComponentMenu("GameReplay/ReplayRecorder")]
    public sealed class ReplayRecorder : MonoBehaviour
    {
        public void startRecording() {
            if (isRecording)
                throw (new System.Exception("Start recording during recording is performing"));

            System.IO.Directory.CreateDirectory(folderToSaveCaptures);
            _recordingState = new RecordingState(replayRecordingCamera, pathToSaveCurrentCapture, _frameRate, _preset);
        }

        public Replay stopRecording() {
            if (!isRecording)
                throw (new System.Exception("Stop recording on recording is not performed"));

            Replay newReplay = new Replay(pathToSaveCurrentCapture);

            _recordingState.closeRecordingSession();
            _recordingState = null;
            ++_currentCaptureIndex;

            return newReplay;
        }

        private class RecordingState {
            public RecordingState(Camera inCamera, string inPathToSaveResult, float inFrameRate, FFmpegPreset inPreset) {
                tempRT = createCaptureTexture(inCamera);
                inCamera.targetTexture = tempRT;
                blitter = Blitter.CreateInstance(inCamera);

                int width = Mathf.Max(8, inCamera.pixelWidth) / 2 * 2;
                int height = Mathf.Max(8, inCamera.pixelHeight) / 2 * 2;

                session = FFmpegSession.CreateWithOutputPath(
                    inPathToSaveResult,
                    width,
                    height,
                    inFrameRate, inPreset
                );

                startTime = Time.time;
                frameCount = 0;
                frameDropCount = 0;

                _camera = inCamera;
            }

            public void closeRecordingSession() {
                session.Close();
                session.Dispose();
                _camera.targetTexture = null;
                Destroy(tempRT);
                Destroy(blitter);
            }

            static private RenderTexture createCaptureTexture(Camera inCamera) {
                int width = Mathf.Max(8, inCamera.pixelWidth) / 2 * 2;
                int height = Mathf.Max(8, inCamera.pixelHeight) / 2 * 2;
            
                RenderTextureFormat theFormat = inCamera.allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
                var theResult = new RenderTexture(width, height, 24, theFormat);
                theResult.antiAliasing = inCamera.allowMSAA ? QualitySettings.antiAliasing : 1;
                return theResult;
            }

            public FFmpegSession session = null;
            public RenderTexture tempRT = null;
            public GameObject blitter = null;

            public int frameCount = 0;
            public float startTime = 0;
            public int frameDropCount = 0;

            private Camera _camera = null;
        }

        private void Update() {
            if (null == _recordingState)
                return;

            float frameTime = _recordingState.startTime + (_recordingState.frameCount - 0.5f) / _frameRate;
            float gap = Time.time - frameTime;
            float delta = 1f / _frameRate;

            if (gap < 0) {
                _recordingState.session.PushFrame(null);
            } else if (gap < delta) {
                _recordingState.session.PushFrame(replayRecordingCamera.targetTexture);
                _recordingState.frameCount++;
            } else if (gap < delta * 2) {
                _recordingState.session.PushFrame(replayRecordingCamera.targetTexture);
                _recordingState.session.PushFrame(replayRecordingCamera.targetTexture);
                _recordingState.frameCount += 2;
            } else {
                if (++_recordingState.frameDropCount == 10) {
                    Debug.LogWarning(
                        "Significant frame droppping was detected. This may introduce " +
                        "time instability into output video. Decreasing the recording " +
                        "frame rate is recommended."
                    );
                }

                _recordingState.session.PushFrame(replayRecordingCamera.targetTexture);
                _recordingState.frameCount += Mathf.FloorToInt(gap * _frameRate);
            }
        }

        private void OnDisable() {
            if (isRecording) {
                _recordingState.closeRecordingSession();
            }
        }

        private Camera replayRecordingCamera { get { return _replayRecordingCamera; } }
        private bool isRecording { get { return (null != _recordingState); } }

        private static string folderToSaveCaptures { get { return Application.persistentDataPath + "/" + "GameReplay"; } }
        private string currentCaptureFileName { get { return "replayCapture" + _currentCaptureIndex; } }
        private string pathToSaveCurrentCapture { get { return folderToSaveCaptures + "/" + currentCaptureFileName + _preset.GetSuffix(); } }

        //Fields
        private RecordingState _recordingState = null;
        private int _currentCaptureIndex = 0;

        [SerializeField] Camera _replayRecordingCamera = null;
        [SerializeField] FFmpegPreset _preset; public FFmpegPreset preset { get { return _preset; } set { _preset = value; } }
        [SerializeField] float _frameRate = 60; public float frameRate { get { return _frameRate; } set { _frameRate = value; } }

        //IEnumerator Start() { for (var eof = new WaitForEndOfFrame(); ;) { yield return eof; _session?.CompletePushFrames(); } }
    }
}

