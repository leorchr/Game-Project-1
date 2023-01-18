using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Electric"))
        {
            GetComponentInParent<EnemyPatrol>().Freeze();
            animator.SetBool("IsDead", true);
            Invoke("EnemyDie", 2f);
            
        }

    }

    void EnemyDie()
    {
        Destroy(transform.parent.gameObject);
    }
}
