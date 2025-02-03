using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayer : MonoBehaviour
{
    private ScrPlayerMovement _scrPlayerMovement;
    private Animator _animator;
    private AudioSource _audioSource;

    [SerializeField]
    private bool isSuper;
    
    public AudioClip coinClip;
    public AudioClip deathClip;
    
    public int vidas = 5;
    public int score = 0;
    public bool isDead = false;
    
    [SerializeField]
    private Renderer topRenderer;
    private Material _material;
    
    
    void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _animator = GetComponentInChildren<Animator>();
        _scrPlayerMovement = GetComponent<ScrPlayerMovement>();
        _material = topRenderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            SetSuperPill(true);
        }
    }

    public void SetSuperPill(bool superState)
    {
        isSuper = superState;
        _animator.SetBool("IsSuper", superState);
        _material.SetInt("_IsSuper", isSuper ? 1 : 0);
    }

    public void RevivePlayer()
    {   
        isDead = false;
        _animator.SetBool("IsDead", isDead);
        _scrPlayerMovement.ResetPosition();
    }

    public void KillPlayer()
    {
        isDead = true;
        _audioSource.PlayOneShot(deathClip);
        _animator.SetBool("IsDead", isDead);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy") && !isDead && !isSuper)
        {
            vidas --;
            KillPlayer();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pill"))
        {
            score++;
            _audioSource.PlayOneShot(coinClip);
        }
    }
}
