using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchVent : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("CameraSwitcher"))
        //{
            animator.enabled = true;
        //}
        
    }
}
