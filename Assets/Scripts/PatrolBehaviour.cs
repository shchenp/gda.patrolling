using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> _patrolPoints;

    [SerializeField]
    private float _speed;

    [SerializeField] 
    private float _patrolTime;

    private float _currentTravelTime;
    private float _currentPatrollingTime;
    private float _nextPointDuration;

    private float _distanceBetweenPoints;
    
    private bool _isPatrolling;

    private int _currentPoint;
    private int _nextPoint;
    
    private Vector3 _currentPointPosition;
    private Vector3 _nextPointPosition;

    private void Awake()
    {
        _currentPoint = -1;
        _nextPoint = 0;
    }

    private void Start()
    {
        _currentPointPosition = transform.position;
        CheckNextPoint();
    }

    private void Update()
    {
        TryMoveNextPoint();
        TryPatrolPoint();
    }

    private void TryMoveNextPoint()
    {
        if (_isPatrolling)
        {
            return;
        }

        _currentTravelTime += Time.deltaTime;
        var progress = _currentTravelTime / _nextPointDuration;
        transform.position = Vector3.Lerp(_currentPointPosition, _nextPointPosition, progress);

        if (progress >= 1)
        {
            _currentTravelTime = 0;
            _currentPatrollingTime = 0;
            _currentPointPosition = _nextPointPosition;
            _isPatrolling = true;
        }
    }

    private void TryPatrolPoint()
    {
        if (!_isPatrolling)
        {
            return;
        }

        _currentPatrollingTime += Time.deltaTime;

        if (_currentPatrollingTime >= _patrolTime)
        {
            _isPatrolling = false;
            CheckNextPoint();
        }
    }

    private void CheckNextPoint()
    {
        _currentPoint++;
        _nextPoint++;

        if (_nextPoint >= _patrolPoints.Count)
        {
            _nextPoint = 0;
        }

        if (_currentPoint >= _patrolPoints.Count)
        {
            _currentPoint = 0;
        }

        _nextPointPosition = _patrolPoints[_nextPoint];
        _distanceBetweenPoints = Vector3.Distance(_currentPointPosition, _nextPointPosition);
        _nextPointDuration = _distanceBetweenPoints / _speed;
    }
}
