using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostMovement : MonoBehaviour
{
    public Transform checkpointsGroup;
    public Transform[] _checkpoints;
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
        _checkpoints = new Transform[checkpointsGroup.childCount];
        for (int i = 0; i < checkpointsGroup.childCount; i++)
        {
            _checkpoints[i] = checkpointsGroup.GetChild(i);
        }
        
        Debug.Log(_checkpoints.Length);
        transform.position = _checkpoints[_currentCheckpoint].position;
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
        _goal = new[] {Mathf.RoundToInt(_checkpoints[index].transform.position.x),Mathf.RoundToInt(_checkpoints[index].transform.position.z)};
        _path = _dijkstra.CalculatePath(_start, _goal);
        _index = 0;
    }

    void Update()
    {
    }

    public Transform GetCheckpoint(int index)
    {
        return _checkpoints[index];
    }

    public void SetCheckpoint(int newIndex)
    {
        _currentCheckpoint = newIndex;
        _t = 0;
    }

    public int GetNearestCheckpoint()
    {
        int minIndex = 0;
        float minDistance = float.MaxValue;
        
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            Transform checkpoint = _checkpoints[i];
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
            if (_t >= 1 && _index != _path.Count-1)
            {
                _t = 0;
                _initialPosition = new Vector3(_path[_index][0], 0, _path[_index][1]);
                
                _finalPosition = new Vector3(_path[_index+1][0], 0, _path[_index+1][1]);
                
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
        
        if (Vector3.Distance(transform.position, _checkpoints[_currentCheckpoint].position) < 0.05f)
        {
            _currentCheckpoint = (_currentCheckpoint + 1)%_checkpoints.Length;
            CalculatePathToCheckpoint(_currentCheckpoint);
        }
        FollowPath();
    }

    private void OnDrawGizmos()
    {
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