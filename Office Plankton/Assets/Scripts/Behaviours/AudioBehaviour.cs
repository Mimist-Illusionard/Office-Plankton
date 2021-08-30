using UnityEngine;


public class AudioBehaviour : MonoBehaviour, IExecute
{
    private AudioSource _audioSource;
    private AudioClip _currentClip;

    [SerializeField] private bool _isRandomVolume = true;
    [SerializeField] private bool _isRandomPitch = true;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        RandomPitch();
        RandomVolume();

        GameManager.Singleton.SetNewExecuteObject(this);
    }

    public void RandomPitch()
    {
        if (!_audioSource) return;

        if (_isRandomPitch == true)
            _audioSource.pitch = Random.Range(0.3f, 1f);
    }

    public void RandomVolume()
    {
        if (!_audioSource && _isRandomVolume) return;

        if (_isRandomVolume == true)
        _audioSource.volume = Random.Range(0.3f, 0.8f);
    }

    //This is VERY bad but i don't have time :(
    public void Execute()
    {
        if (_currentClip != _audioSource.clip)
        {
            _audioSource.Play();
            _currentClip = _audioSource.clip;
        }
    }
}
