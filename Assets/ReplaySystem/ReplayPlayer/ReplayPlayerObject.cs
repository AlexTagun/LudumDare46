using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameReplay
{
    public class ReplayPlayerObject : MonoBehaviour
    {
        public void playReplay(Replay inReplay, bool inClearReplayAfterFinish = true) {
            videoPlayer.url = inReplay.replayURL;
            videoPlayer.prepareCompleted += (VideoPlayer unused1) => {
                if (!_interactionTexture)
                    _interactionTexture = createTexture(videoPlayer.texture);
                videoPlayer.targetTexture = _interactionTexture;
                image.texture = _interactionTexture;
                image.enabled = true;

                videoPlayer.loopPointReached += (VideoPlayer unused2) => {
                    image.enabled = false;
                    if (inClearReplayAfterFinish)
                        inReplay.clear();
                };

                videoPlayer.Play();
            };
            videoPlayer.Prepare();
        }

        private void Awake() {
            image.enabled = false;
        }

        static private RenderTexture createTexture(Texture inVideoPlayerTexture) {
            return new RenderTexture(inVideoPlayerTexture.width, inVideoPlayerTexture.height, 1, inVideoPlayerTexture.graphicsFormat, 1);
        }

        private RenderTexture _interactionTexture = null;
        [SerializeField] private RawImage image = null;
        [SerializeField] private VideoPlayer videoPlayer = null;
    }
}
