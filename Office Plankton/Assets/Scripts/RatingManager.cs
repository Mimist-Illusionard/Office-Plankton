using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.Audio;


public class RatingManager : MonoBehaviour
{
    [SerializeField] private GameObject _winWindow;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private GameObject _loseWindow;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Task Settings")]
    [SerializeField] private int _taskRatingSuccess;
    [SerializeField] private int _taskRatingFailed;
    [SerializeField] private int _endGameRating;

    [Space(5)]
    [SerializeField] private int _currentRating;
    [SerializeField] private Counter _ratingCounter;

    [Header("Post Settings")]
    [SerializeField] private PostUi _postUi;
    [SerializeField] private Post _currentPost;
    [SerializeField] private List<Post> _post = new List<Post>();
    [SerializeField] private float _speedModifier;
    [SerializeField] private float _bonusTime;

    public Action<int> OnRatingChange;

    public static RatingManager Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        OnRatingChange += _ratingCounter.OnValueChange;
        OnRatingChange?.Invoke(_currentRating);
        _postUi.ChangePostUi(_currentPost);
    }

    public void TaskSucceed()
    {
        _currentRating += UnityEngine.Random.Range(_taskRatingSuccess - 5, _taskRatingSuccess + 5);
        CheckRating();

        OnRatingChange?.Invoke(_currentRating);
    }

    public void TaskFailed()
    {
        _currentRating += UnityEngine.Random.Range(_taskRatingFailed - 5, _taskRatingFailed + 5);
        CheckRating();

        OnRatingChange?.Invoke(_currentRating);
    }

    private void CheckRating()
    {
        if (_currentRating <= _endGameRating)
        {
            UnityEngine.Debug.Log("End game logic");
            _loseWindow.SetActive(true);
            _scoreText.text = _currentRating.ToString();
            GameManager.Singleton.ClearAllExeciteObjects();
            AudioListener.pause = true;
            AudioListener.volume  = 0;
        }

        if (_currentRating >= 500f)
        {
            _winWindow.SetActive(true);
            _timeText.text = TimerManager.Singleton.GetTime() + " sec";
            GameManager.Singleton.ClearAllExeciteObjects();
            AudioListener.pause = true;
            AudioListener.volume = 0;
        }

        Post nextPost = null;
        for (int i = 0; i < _post.Count; i++)
        {
            var post = _post[i];
            if (post.NeedRatingAmount > _currentRating && post.PostID == _currentPost.PostID)
            {
                if (i == 0) break; 
                var previousPost = _post[i - 1];

                _currentPost = previousPost;
                PlayerManager.Singleton.GetPlayer().RemoveSpeedModifier(_speedModifier);
                TaskManager.Singleton.AddBonusTime(-_bonusTime);

                return;
            }

            if (post.NeedRatingAmount <= _currentRating)
            {
                nextPost = post;               
            }
        }

        if (nextPost != null && nextPost != _currentPost)
        {
            _currentPost = nextPost;
            _postUi.ChangePostUi(_currentPost);

            PlayerManager.Singleton.GetPlayer().AddSpeedModifier(_speedModifier);
            TaskManager.Singleton.AddBonusTime(_bonusTime);
        }        
    }
}

[Serializable]
public class Post
{
    public Sprite PostImage;
    public string PostName;
    public int NeedRatingAmount;
    public int PostID;
}