using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostMovement : MonoBehaviour
{
    public Transform[] checkpoints;
    public float moveSpeed = 1;
    private float _t = 1;
    private Vector3 _initialPosition;
    private Vector3 _finalPosition;
    private Vector3 _dir;
    private int _currentCheckpoint;
    public GameObject player;
    
    private ScrDijkstra _dijkstra;
    
    // Chase player
    private int[] _start;
    private int[] _goal;
    private List<int[]> _path;
    private int _index;
    
    void Start()
    {
        transform.position = checkpoints[_currentCheckpoint].position;
        _initialPosition = transform.position;
        _finalPosition = transform.position;
        _dijkstra = new ScrDijkstra(15, 19);
    }

    public void CalculatePathToPlayer()
    {
        _start = new[] {Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z) };
        _goal = new[] {Mathf.RoundToInt(player.transform.position.x),Mathf.RoundToInt(player.transform.position.z)};
        _path = _dijkstra.CalculatePath(_start, _goal);
        _index = 0;
    }
    
    public void CalculatePathToCheckpoint(int index)
    {
        _start = new[] {Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z) };
        _goal = new[] {Mathf.RoundToInt(checkpoints[index].transform.position.x),Mathf.RoundToInt(checkpoints[index].transform.position.z)};
        _path = _dijkstra.CalculatePath(_start, _goal);
        _index = 0;
    }

    void Update()
    {
    }

    public void SetCheckpoint(int newIndex)
    {
        _currentCheckpoint = newIndex;
        _initialPosition = checkpoints[_currentCheckpoint].position;
        _finalPosition = _initialPosition;
        _dir = checkpoints[(_currentCheckpoint + 1)%checkpoints.Length].position - transform.position;
        _dir.Normalize();
    }

    public int GetNearestCheckpoint()
    {
        int minIndex = 0;
        float minDistance = float.MaxValue;
        
        for (int i = 0; i < checkpoints.Length; i++)
        {
            Transform checkpoint = checkpoints[i];
            float distance = Vector3.Distance(checkpoint.position, transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }
        
        return minIndex;
    }

    public void FollowPath()
    {
        if (_path == null || _path.Count == 0)
        {
            Debug.Log("No path");
            return;
        }
        
        if (_index < _path.Count)
        {
            if (_t >= 1)
            {
                _t = 0;
                _initialPosition = new Vector3(_path[_index][0], 0, _path[_index][1]);
                if (_index != _path.Count-1)
                {
                    _finalPosition = new Vector3(_path[_index+1][0], 0, _path[_index+1][1]);
                }
                _index++;
            }
            Debug.DrawRay(transform.position, _dir, Color.red);

            _t += Time.deltaTime * moveSpeed;
            _t = Mathf.Clamp(_t, 0, 1);
            gameObject.transform.position = Vector3.Lerp(_initialPosition, _finalPosition, _t);
        }
        
    }

    public void FollowCheckpoints()
    {
        if (_t >= 1)
        {
            _t = 0;
            _initialPosition = gameObject.transform.position;
            
            if (Vector3.Distance(transform.position, checkpoints[_currentCheckpoint].position) < 0.1f)
            {
                _dir = checkpoints[(_currentCheckpoint + 1)%checkpoints.Length].position - transform.position;
                _dir.Normalize();
                _currentCheckpoint = (_currentCheckpoint + 1)%checkpoints.Length;
            }
            
            _finalPosition = _initialPosition + _dir;
        }
        Debug.DrawRay(transform.position, _dir, Color.red);

        _t += Time.deltaTime * moveSpeed;
        _t = Mathf.Clamp(_t, 0, 1);
        gameObject.transform.position = Vector3.Lerp(_initialPosition, _finalPosition, _t);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            Vector3 dir = checkpoints[(i + 1)%checkpoints.Length].position - checkpoints[i].position;
            Gizmos.DrawRay(checkpoints[i].position, dir);
        }

        if (_path == null || _path.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _path.Count; i++)
        {
            Vector3 newDir = new Vector3(_path[i][0], 0, _path[i][1]);
            Gizmos.DrawCube(newDir, Vector3.one/2);
        }

    }
}