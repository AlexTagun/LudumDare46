using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace GameReplay
{
    public class ReplayPlayerObject : MonoBehaviour
    {
        private enum EReplayPlayingStatus {
            NoAction,
            NextReplayPreporation,
            Playing,
            FinishedCurrentReplay
        }

        public System.Collections.IEnumerator playReplays(
            List<Replay> inReplays,
            System.Action<Replay> inAfterStartNextReplayPreporation,
            System.Action<Replay> inBeforeStartNextReplay,
            System.Action inLastReplayFinished)
        {
            int theReplayIndexForExit = inReplays.Count;
            int theCurrentReplayIndex = 0;

            EReplayPlayingStatus theReplayStatus = EReplayPlayingStatus.NoAction;

            videoPlayer.prepareCompleted += (VideoPlayer unused1) => {
                inBeforeStartNextReplay(inReplays[theCurrentReplayIndex]);
                videoPlayer.Play();
                theReplayStatus = EReplayPlayingStatus.Playing;
            };

            videoPlayer.loopPointReached += (VideoPlayer unused2) => {
                theReplayStatus = EReplayPlayingStatus.FinishedCurrentReplay;
            };

            System.Action theStartCurrentReplayPreporation = ()=>{
                Replay theCurrentReplay = inReplays[theCurrentReplayIndex];
                videoPlayer.url = theCurrentReplay.replayURL;
                videoPlayer.Prepare();
                theReplayStatus = EReplayPlayingStatus.NextReplayPreporation;
                inAfterStartNextReplayPreporation(inReplays[theCurrentReplayIndex]);
            };

            while (true) {
                if (EReplayPlayingStatus.NoAction == theReplayStatus) {
                    theStartCurrentReplayPreporation();
                } else if (EReplayPlayingStatus.FinishedCurrentReplay == theReplayStatus) {
                    if (++theCurrentReplayIndex == theReplayIndexForExit) {
                        break;
                    } else {
                        theStartCurrentReplayPreporation();
                        yield return null;
                    }
                } else {
                    yield return null;
                }
            }

            inLastReplayFinished();
        }

        private void Start() {
            RenderTexture theInteractionTexture = createTexture();
            videoPlayer.targetTexture = theInteractionTexture;
            image.texture = theInteractionTexture;
        }

        private RenderTexture createTexture() {
            Rect theMyRect = GetComponent<RectTransform>().rect;
            return new RenderTexture((int)theMyRect.width, (int)theMyRect.height, 1, GraphicsFormat.R8G8B8A8_UNorm, 1);
        }

        private RenderTexture _interactionTexture = null;
        [SerializeField] private RawImage image = null;
        [SerializeField] private VideoPlayer videoPlayer = null;
    }
}
