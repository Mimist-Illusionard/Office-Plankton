using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrayerSystem : MonoBehaviour, IExecute
{
    [SerializeField] private Transform _bossPosition;
    [SerializeField] private Transform _agentSpawnpoint;
    [SerializeField] private List<Transform> _prayPoints;
    [SerializeField] private List<GameObject> _prayerPrefabs;

    [Header("Appear Time Settings")]
    [SerializeField] private float _timeAppearPrayer;
    [SerializeField] private float _timeAppearRandomizeBorder;
    [SerializeField] private int _maximalAgentAppear;

    [SerializeField] private float _timePray;

    private List<Agent> _agents = new List<Agent>();
    private List<Agent> _temporaryAgents = new List<Agent>();
    private float _currentTimeAppearPrayer;
    private float _delayTime = 1f;
    private float _delayTime_2 = 4f;
    private bool _isHasPrayers;

    private void Start()
    {
        GameManager.Singleton.SetNewExecuteObject(this);
        _currentTimeAppearPrayer = UnityEngine.Random.Range(_timeAppearPrayer - _timeAppearRandomizeBorder, _timeAppearPrayer + _timeAppearRandomizeBorder);
    }

    public void Execute()
    {
        if (_isHasPrayers == false)
        {
            _currentTimeAppearPrayer -= Time.deltaTime;
            if (_currentTimeAppearPrayer <= 0)
            {
                var amount = UnityEngine.Random.Range(1, _maximalAgentAppear);
                CreateAgents(amount);

                _currentTimeAppearPrayer = UnityEngine.Random.Range(_timeAppearPrayer - _timeAppearRandomizeBorder, _timeAppearPrayer + _timeAppearRandomizeBorder);
                _isHasPrayers = true;
            }
        }

        if (_isHasPrayers)
        {
            _delayTime -= Time.deltaTime;
            if (_delayTime >= 0) return;

            for (int i = 0; i < _temporaryAgents.Count; i++)
            {
                var agent = _temporaryAgents[i];
                var animatorBehaviour = agent.GetComponentInChildren<AnimatorBehaviour>();

                if (agent.IsLastPoint())
                {
                    var timePray = UnityEngine.Random.Range(_timePray - 2, _timePray + 2);

                    animatorBehaviour.SetDurable(timePray);
                    animatorBehaviour.SetBoolDurable("isPray");
                    var rotateBehaviour = agent.GetComponent<RotateToPointBehaviour>();
                    rotateBehaviour.RotateToPoint(_bossPosition.position);

                    _temporaryAgents.Remove(agent);
                }
            }

            for (int i = 0; i < _agents.Count; i++)
            {
                var agent = _agents[i];
                var animatorBehaviour = agent.GetComponentInChildren<AnimatorBehaviour>();
                var rotateBehaviour = agent.GetComponent<RotateToPointBehaviour>();
                rotateBehaviour.RotateToPoint(_bossPosition.position);

                if (animatorBehaviour.IsDurableEnded())
                {
                    rotateBehaviour.DisableRotation = true;
                    agent.ClearWaypoints();
                    agent.AddWaypoint(_agentSpawnpoint);
                    agent.SetDestination(_agentSpawnpoint.position);
                    _delayTime_2 -= Time.deltaTime;
                }
                
                if (_delayTime_2 >= 0) return;

                if (agent.IsLastPoint())
                {
                    Destroy(agent.gameObject);
                    _agents.Remove(agent);
                    GameManager.Singleton.RemoveExecuteObject(agent);
                }
            }

            if (_agents.Count <= 0)
            {
                _isHasPrayers = false;
                _delayTime = 1f;
                _delayTime_2 = 4f;
}
        }
    }

    private void CreateAgents(int amount)
    {
        List<Transform> prayPoints = new List<Transform>();
        for (int i = 0; i < _prayPoints.Count; i++)
        {
            var point = _prayPoints[i];
            prayPoints.Add(point);
        }

        for (int i = 0; i < amount; i++)
        {
            var createdAgent = Instantiate(_prayerPrefabs[UnityEngine.Random.Range(1, _prayerPrefabs.Count)], _agentSpawnpoint.position, Quaternion.identity);

            var agent = createdAgent.GetComponent<Agent>();
            var point = prayPoints[UnityEngine.Random.Range(0, prayPoints.Count)];

            agent.AddWaypoint(point);
            prayPoints.Remove(point);

            _agents.Add(agent);
        }

        for (int i = 0; i < _agents.Count; i++)
        {
            var agent = _agents[i];
            _temporaryAgents.Add(agent);
        }
    }
}
