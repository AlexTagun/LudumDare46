using UnityEngine;

public class SoundSequencePlayer : MonoBehaviour
{
    private void Start() {
        _audioSourceA.loop = _loopSounds;
        _audioSourceB.loop = _loopSounds;

        StartCoroutine(elementIndexTrackingCoroutine());
        StartCoroutine(soundPlayingCoroutine());
    }

    private System.Collections.IEnumerator elementIndexTrackingCoroutine() {
        for (_elementIndexThatShouldBePlayed = 0;
            _elementIndexThatShouldBePlayed < _sequenceElements.Length;
            ++_elementIndexThatShouldBePlayed)
        {
            yield return new WaitForSeconds(
                    _sequenceElements[_elementIndexThatShouldBePlayed].timeSpanToPlay);
        }
    }

    private System.Collections.IEnumerator soundPlayingCoroutine() {
        while (_elementIndexThatIsPlaying < _sequenceElements.Length) {
            if (_elementIndexThatIsPlaying != _elementIndexThatShouldBePlayed) {
                AudioSource theFadingInAudioSource = _audioSourceAIsMain ? _audioSourceB : _audioSourceA;
                AudioSource theFadingOutAudioSource = _audioSourceAIsMain ? _audioSourceA : _audioSourceB;
                yield return soundsChangingCoroutine(
                        _sequenceElements[_elementIndexThatShouldBePlayed].clipToPlay,
                        theFadingInAudioSource, theFadingOutAudioSource);
                _audioSourceAIsMain = !_audioSourceAIsMain;

                _elementIndexThatIsPlaying = _elementIndexThatShouldBePlayed;
            }  {
                yield return null;
            }
        }
    }

    private System.Collections.IEnumerator soundsChangingCoroutine(
            AudioClip inClipToFadeIn, AudioSource inFadingInAudio, AudioSource inFadingOutAudio)
    {
        _soundsChangingRemainingTime = _timeSpanToChangeElements;

        inFadingInAudio.clip = inClipToFadeIn;
        inFadingInAudio.Play();

        while (_soundsChangingRemainingTime > 0f) {
            inFadingOutAudio.volume = _soundsChangingRemainingTime / _timeSpanToChangeElements;
            inFadingInAudio.volume = 1f - _soundsChangingRemainingTime / _timeSpanToChangeElements;

            _soundsChangingRemainingTime -= Time.fixedDeltaTime;
            yield return null;
        }

        inFadingOutAudio.volume = 0f;
        inFadingInAudio.volume = 1f;

        inFadingOutAudio.Stop();
    }

    [System.Serializable]
    struct SequenceElement {
        public float timeSpanToPlay;
        public AudioClip clipToPlay;
    }

    //Fields
    [SerializeField] private SequenceElement[] _sequenceElements;
    [SerializeField] private float _timeSpanToChangeElements = 1f;

    [SerializeField] private bool _loopSounds = true;
    [SerializeField] private bool _loopLastSound = true;

    [SerializeField] private AudioSource _audioSourceA;
    [SerializeField] private AudioSource _audioSourceB;

    private float _soundsChangingRemainingTime = 0f;
    private bool _audioSourceAIsMain = true;

    private int _elementIndexThatIsPlaying = -1;
    private int _elementIndexThatShouldBePlayed = 0;
}
