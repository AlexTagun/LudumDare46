using UnityEngine;

public class RandomSoundPlayingComponent : MonoBehaviour
{
    private void Awake() {
        _audioSource.clip = getRandomSound();
    }

    private void Start() {
        StartCoroutine(mainCoroutine());
    }

    System.Collections.IEnumerator mainCoroutine() {
        if (_delayOnStart)
            yield return delayCoroutine();

        while (true) {
            yield return playCoroutine();
            yield return delayCoroutine();
        }
    }

    System.Collections.IEnumerator delayCoroutine() {
        float theDelay = Random.Range(_minTimeBetweenSounds, _maxTimeBetweenSounds);
        yield return new WaitForSeconds(theDelay);
    }

    System.Collections.IEnumerator playCoroutine() {
        if (!_selectVariantOnce)
            _audioSource.clip = getRandomSound();
        _audioSource.Play();

        while (_audioSource.isPlaying)
            yield return null;
    }

    private AudioClip getRandomSound() {
        if (null == _soundVariants || 0 == _soundVariants.Length)
            throw(new System.Exception("Selecting without variants"));

        int theRandomScreamIndex = Random.Range(0, _soundVariants.Length);
        return _soundVariants[theRandomScreamIndex];
    }

    private bool isPlaying => _audioSource.isPlaying;

    //Fields
    [SerializeField] AudioClip[] _soundVariants = null;
    [SerializeField] float _minTimeBetweenSounds = 4f;
    [SerializeField] float _maxTimeBetweenSounds = 10f;
    [SerializeField] bool _selectVariantOnce = true;
    [SerializeField] bool _delayOnStart = true;

    [SerializeField] AudioSource _audioSource = null;
}
