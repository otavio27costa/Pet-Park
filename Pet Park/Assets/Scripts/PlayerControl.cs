using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerControl : MonoBehaviour
{
    [Header("Follow Mouse")]
    public float followSpeed = 5f;      // Velocidade ao seguir o mouse

    [Header("Dash")]
    public float dashSpeed = 25f;        // Velocidade do dash
    public float dashStopDistance = 1;
    public float dashDistance = 10f;
    public float dashCooldown = 3f;
    public float cooldown = 0;
    public bool canDash;

    [Header("Attack")]
    public float attackRadius = 1f;
    public LayerMask enemyLayer;

    private Vector3 mouseWorldPos;
    private Vector3 dashTarget;
    public bool isDashing;
    public TrailRenderer tr;

    private void Awake()
    {
        tr.emitting = false;
    }

    void Update()
    {

        cooldown += Time.deltaTime;
        if (cooldown >= dashCooldown)
        {
            canDash = true;
            cooldown = 0;
        }
        // Posição do mouse no mundo
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // Clique para iniciar dash
        if (Input.GetMouseButtonDown(0) && canDash == true)
        {
            Vector3 dashDirection = (mouseWorldPos - transform.position).normalized;
            dashTarget = transform.position + dashDirection * dashDistance;
            isDashing = true;
            tr.emitting = true;
            canDash = false;
        }

        if (isDashing)
        {
            DashMove();
            Attack();
        }
        else
        {
            FollowMouse();
        }
    }

    // ================= MOVIMENTO =================

    void FollowMouse()
    {
        // Movimento suave em direção ao mouse
        transform.position = Vector3.MoveTowards(
            transform.position,
            mouseWorldPos,
            followSpeed * Time.fixedDeltaTime
        );
    }

    void DashMove()
    {
        // Movimento rápido de dash
        transform.position = Vector3.MoveTowards(
            transform.position,
            dashTarget,
            dashSpeed * Time.deltaTime
        );

        // Para o dash quando chega ao destino
        if (Vector3.Distance(transform.position, dashTarget) < dashStopDistance)
        {
            tr.emitting = false;
            isDashing = false;
        }
    }

    // ================= ATAQUE =================

    void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            Destroy(enemy.gameObject); // substitua por dano se quiser
        }
    }

    // ================= DEBUG =================

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
