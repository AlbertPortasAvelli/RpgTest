using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDuration = 0.5f;
    public int defaultDamageAmount = 20;
    public float attackRange = 2.0f;
    public LayerMask enemyLayer;
    private PlayerJump playerJump;


    private bool hasAttackedThisClick = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        if ( !playerJump.IsJumpingDown && !playerJump.IsJumpingUp)
        {
            if (Input.GetMouseButtonDown(0) && !hasAttackedThisClick)
            {
                Attack();
                StartCoroutine(ResetAttackState());
                hasAttackedThisClick = true;
            }
        }
    }

    private void Attack()
    {
        animator.SetBool("isAttacking", true);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        Enemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = collider.GetComponent<Enemy>();
                }
            }
        }

        if (nearestEnemy != null)
        {
            int damageToApply = GetDamageBasedOnEnemyType(nearestEnemy);
            nearestEnemy.TakeDamage(damageToApply);
        }
    }

    private IEnumerator ResetAttackState() 
    {
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("isAttacking", false);
        hasAttackedThisClick = false;
    }

    private int GetDamageBasedOnEnemyType(Enemy enemy)
    {
        Debug.Log("hitted!!!");
        return defaultDamageAmount;
    }
}
