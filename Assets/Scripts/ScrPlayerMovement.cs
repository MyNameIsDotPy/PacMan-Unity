using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayerMovement : MonoBehaviour
{

    private Vector3 _dirActual;
    private Vector3 _dir;
    private Vector3 _initialPosition;
    private Vector3 _finalPosition;
    private ScrPlayer _scrPlayer;
    
    private GameObject _childMesh;
    
    public float moveSpeed;
    
    private float _t;
    
    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _finalPosition = transform.position;
        
        _childMesh = transform.GetChild(0).gameObject;
        _scrPlayer = GetComponent<ScrPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_scrPlayer.isDead)
        {
            bool space = Input.GetKeyDown("space");
            if (space)
            {
                _scrPlayer.RevivePlayer();
            }
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            _dirActual = new Vector3(horizontal, 0, 0);
        }
        
        if (vertical != 0)
        {
            _dirActual = new Vector3(0, 0, vertical);
        }

        if (Physics.Raycast(transform.position, _dir, out RaycastHit hit2, 1))
        {
            if (hit2.collider.gameObject.CompareTag("Wall"))
            {
                _dir = Vector3.zero;
            }
        }
        
        if (_t >= 1)
        {
            if (Physics.Raycast(transform.position, _dirActual, out RaycastHit hit, 1))
            {
                if (!hit.collider.CompareTag("Wall"))
                {
                    _dir = _dirActual;
                }
            }
            else
            {
                _dir = _dirActual;
            }
            _t = 0;
            _initialPosition = gameObject.transform.position;
            _finalPosition = _initialPosition + _dir;
            _childMesh.transform.rotation = Quaternion.LookRotation(-_dir, transform.up);
        }

        _t += Time.deltaTime * moveSpeed;
        _t = Mathf.Clamp(_t, 0, 1);
        gameObject.transform.position = Vector3.Lerp(_initialPosition, _finalPosition, _t);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + _dirActual);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _dir);
    }

    public void ResetPosition()
    {
        _dir = Vector3.zero;
        _dirActual = Vector3.zero;
        _initialPosition = new Vector3(7, 0, 10);
        _finalPosition = _initialPosition;
        transform.position = _initialPosition;
    }
}
