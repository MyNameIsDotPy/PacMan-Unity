using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayer : MonoBehaviour
{
    private ScrPlayerMovement _scrPlayerMovement;
    private Animator _animator;
    public int vidas = 5;
    public int score = 0;
    public bool isDead = false;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _scrPlayerMovement = GetComponent<ScrPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        _animator.SetBool("IsDead", isDead);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
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
        }
    }
}
