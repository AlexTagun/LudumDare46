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
            VideoPreparing,
            VideoPlaying
        }

        public System.Collections.IEnumerator playReplays(List<Replay> inReplays, System.Action inLastReplayFinished) {
            if (inReplays == null || inReplays.Count == 0) {
                inLastReplayFinished();
                yield break;
            }

            List<System.Action<VideoPlayer>> theAllVideosPreprationActions = new List<System.Action<VideoPlayer>>();
            for (int theIndex = 0; theIndex < inReplays.Count - 1; ++theIndex) {
                string theReplayURL = (string)inReplays[theIndex].replayURL.Clone();
                theAllVideosPreprationActions.Add((VideoPlayer inPlayer) => {
                    inPlayer.url = theReplayURL;
                    inPlayer.Prepare();
                });
                    
                VideoClip theNextCutaway = getNextRandomCutaway();
                if (null != theNextCutaway) {
                    theAllVideosPreprationActions.Add((VideoPlayer inPlayer) => {
                        preparingVideoPlayer.clip = theNextCutaway;
                        inPlayer.Prepare();
                    });
                }
            }

            {
                string theReplayURL = (string)inReplays[inReplays.Count - 1].replayURL.Clone();
                theAllVideosPreprationActions.Add((VideoPlayer inPlayer) => {
                    inPlayer.url = theReplayURL;
                    inPlayer.Prepare();
                });

                if (finalCutaway) {
                    theAllVideosPreprationActions.Add((VideoPlayer inPlayer) => {
                        preparingVideoPlayer.clip = finalCutaway;
                        inPlayer.Prepare();
                    });
                }
            }

            int theVideoIndexToExit = theAllVideosPreprationActions.Count;
            int theNextVideoIndex = 0;
            EReplayPlayingStatus theReplayStatus = EReplayPlayingStatus.NoAction;

            System.Action theTryPrepareNextVideo = () => {
                if (theNextVideoIndex != theVideoIndexToExit)
                    theAllVideosPreprationActions[theNextVideoIndex].Invoke(preparingVideoPlayer);
            };

            while (true) {
                switch (theReplayStatus)
                {
                    case EReplayPlayingStatus.NoAction:
                        theTryPrepareNextVideo();

                        swapVideoPlayer();
                        ++theNextVideoIndex;
                        theTryPrepareNextVideo();
                        
                        theReplayStatus = EReplayPlayingStatus.VideoPreparing;
                        break;

                    case EReplayPlayingStatus.VideoPreparing:
                        if (playingVideoPlayer.isPrepared) {
                            playingVideoPlayer.Play();
                            theReplayStatus = EReplayPlayingStatus.VideoPlaying;
                        }
                        break;

                    case EReplayPlayingStatus.VideoPlaying:
                        if (!playingVideoPlayer.isPlaying) {
                            if (theNextVideoIndex != theVideoIndexToExit) {
                                swapVideoPlayer();
                                ++theNextVideoIndex;
                                theTryPrepareNextVideo();

                                theReplayStatus = EReplayPlayingStatus.VideoPreparing;
                            } else {
                                cleanState();
                                yield break;
                            }
                        }
                        break;

                    default:
                        break;
                }
                yield return null;
            }
        }

        private void cleanState() {
            videoPlayerA.Stop();
            videoPlayerB.Stop();

            imageA.gameObject.SetActive(true);
            imageB.gameObject.SetActive(false);

            isVideoPlayerAUsedForPlay = true;
        }

        private void Start() {
            RenderTexture theInteractionTextureA = createTexture();
            videoPlayerA.targetTexture = theInteractionTextureA;
            imageA.texture = theInteractionTextureA;

            RenderTexture theInteractionTextureB = createTexture();
            videoPlayerB.targetTexture = theInteractionTextureB;
            imageB.texture = theInteractionTextureB;

            cleanState();
        }

        private RenderTexture createTexture() {
            Rect theMyRect = GetComponent<RectTransform>().rect;
            return new RenderTexture((int)theMyRect.width, (int)theMyRect.height, 1, GraphicsFormat.R8G8B8A8_UNorm, 1);
        }

        private VideoClip getNextRandomCutaway() {
            return (null == cutaways || 0 == cutaways.Length) ?
                    null : cutaways[Random.Range(0, cutaways.Length)];
        }

        private void swapVideoPlayer() {
            isVideoPlayerAUsedForPlay = !isVideoPlayerAUsedForPlay;
            imageA.gameObject.SetActive(isVideoPlayerAUsedForPlay);
            imageB.gameObject.SetActive(!isVideoPlayerAUsedForPlay);
        }

        private VideoPlayer playingVideoPlayer => isVideoPlayerAUsedForPlay ? videoPlayerA : videoPlayerB;
        private VideoPlayer preparingVideoPlayer => isVideoPlayerAUsedForPlay ? videoPlayerB : videoPlayerA;

        //Fields
        [SerializeField] private RawImage imageA = null;
        [SerializeField] private RawImage imageB = null;
        [SerializeField] private VideoPlayer videoPlayerA = null;
        [SerializeField] private VideoPlayer videoPlayerB = null;
        [SerializeField] private bool isVideoPlayerAUsedForPlay = true;

        [SerializeField] private VideoClip[] cutaways = null;
        [SerializeField] private VideoClip finalCutaway = null;
    }
}
