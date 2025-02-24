using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _playerTransform;
    [SerializeField] float _visionRange=15f;
    [SerializeField] float _attackRange=3f;
    [SerializeField] Transform[] _patrolPoints;
    private int _patrolIndex;
    public enum EnemyState{
        Patrolling,
        Chasing,
        Attacking
    }
    public EnemyState currentState;
    
    void Awake(){
        _agent=GetComponent<NavMeshAgent>();
        _playerTransform=GameObject.FindWithTag("Player").transform;
    }
    void Start(){
        currentState=EnemyState.Patrolling;
        SetPatrolPoints();
    }

    void Update(){
        switch(currentState){
            case EnemyState.Patrolling:
                Patrol();
            break;
            case EnemyState.Chasing:
                Chase();
            break;
            case EnemyState.Attacking:
                Attack();
            break;
        }
    }
    void SetPatrolPoints(){
        _agent.destination=_patrolPoints[_patrolIndex].position;
        _patrolIndex++;
        if(_patrolIndex>=_patrolPoints.Length) _patrolIndex=0;
    }
    bool InRange(float range){
        return Vector3.Distance(transform.position, _playerTransform.position) < range;
    }
    void Patrol(){
        if(InRange(_visionRange)) currentState=EnemyState.Chasing;
        if(_agent.remainingDistance<0.5) SetPatrolPoints();
    }
    void Chase(){
        if(!InRange(_visionRange)) currentState=EnemyState.Patrolling;
        if(InRange(_attackRange)) currentState=EnemyState.Attacking;
        _agent.destination=_playerTransform.position;
    }
    void Attack(){
        Debug.Log("Attacking");
        currentState=EnemyState.Chasing;
    }
}
