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

                            StartCoroutine(animateImageAlpha(playingVideoImage, 1f, 0.1f, 0.2f));
                            theReplayStatus = EReplayPlayingStatus.VideoPlaying;
                        }
                        break;

                    case EReplayPlayingStatus.VideoPlaying:
                        if (!playingVideoPlayer.isPlaying) {
                            StartCoroutine(animateImageAlpha(playingVideoImage, 0f));
                            if (theNextVideoIndex != theVideoIndexToExit) {
                                swapVideoPlayer();
                                ++theNextVideoIndex;
                                theTryPrepareNextVideo();

                                theReplayStatus = EReplayPlayingStatus.VideoPreparing;
                            } else {
                                cleanState();
                                inLastReplayFinished();
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

            setImageVisibility(imageA, false);
            setImageVisibility(imageB, false);

            isVideoPlayerAUsedForPlay = true;
        }

        private void Start() {
            RenderTexture theInteractionTextureA = createTexture();
            videoPlayerA.targetTexture = theInteractionTextureA;
            videoPlayerA.waitForFirstFrame = true;
            imageA.texture = theInteractionTextureA;

            RenderTexture theInteractionTextureB = createTexture();
            videoPlayerB.targetTexture = theInteractionTextureB;
            videoPlayerB.waitForFirstFrame = true;
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
        }

        private void setImageVisibility(RawImage inImage, bool inIsVisible) {
            Color theStartColor = inImage.color;
            theStartColor.a = inIsVisible ? 1f : 0f;
            inImage.color = theStartColor;
        }

        private System.Collections.IEnumerator animateImageAlpha(RawImage inImage, float inTargetAlpha, float inTimeToAnimate = 0f, float inStartingDelay = 0f) {
            if (inStartingDelay > 0f)
                yield return new WaitForSeconds(inStartingDelay);

            Color theStartColor = inImage.color;
            
            if (inTimeToAnimate > 0f)
            {
                float theTotalDeltaAlpha = inTargetAlpha - theStartColor.a;

                float theAnimationTime = 0f;
                
                while (theAnimationTime < inTimeToAnimate) {
                    float theAnimationProgress = theAnimationTime / inTimeToAnimate;
                    float theAlpha = theStartColor.a + theTotalDeltaAlpha * theAnimationProgress;
                    inImage.color = new Color(theStartColor.r, theStartColor.g, theStartColor.b, theAlpha);
                    theAnimationTime += Time.fixedDeltaTime;
                    yield return null;
                }
            }

            inImage.color = new Color(theStartColor.r, theStartColor.g, theStartColor.b, inTargetAlpha);
        }

        private VideoPlayer playingVideoPlayer => isVideoPlayerAUsedForPlay ? videoPlayerA : videoPlayerB;
        private VideoPlayer preparingVideoPlayer => isVideoPlayerAUsedForPlay ? videoPlayerB : videoPlayerA;
        private RawImage playingVideoImage => isVideoPlayerAUsedForPlay ? imageA : imageB;

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
