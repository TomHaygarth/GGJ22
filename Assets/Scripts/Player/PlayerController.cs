using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool m_isGrounded = false;

    private Rigidbody2D m_rigidBody;
    private BoxCollider2D m_collider;

    [SerializeField]
    private float m_runAcceleration = 5.0f;
    [SerializeField]
    private float m_maxHorizontalVelocity = 15.0f;

    [SerializeField]
    private float m_jumpForce = 10.0f;

    [SerializeField]
    private float m_jumpCooldown = 0.1f;

    private float m_currentJumpCooldown = 0.0f;

    [SerializeField]
    private AudioBankPlayer m_jumpSfxBank = null;

    private bool m_isJumping = false;
    private bool m_playJumpSfx = false;
    private bool m_playLandSfx = false;


    private int ground_mask = 0;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
        ground_mask = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        if (m_playJumpSfx)
        {
            if (m_jumpSfxBank != null)
            {
                m_jumpSfxBank.PlayRand();
            }
            m_playJumpSfx = false;
        }
    }

    private void FixedUpdate()
    {
        bool update_velocity = false;
        var current_velocity = m_rigidBody.velocity;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            update_velocity = true;
            current_velocity.x -= m_runAcceleration * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            update_velocity = true;
            current_velocity.x += m_runAcceleration * Time.fixedDeltaTime;
        }


        if (m_currentJumpCooldown <= 0.0f)
        {
            if (CheckGrounded())
            {
                if (Input.GetKey(KeyCode.W) ||
                    Input.GetKey(KeyCode.UpArrow) ||
                    Input.GetKey(KeyCode.Space))
                {
                    m_currentJumpCooldown = m_jumpCooldown;
                    update_velocity = true;
                    current_velocity.y = m_jumpForce * m_rigidBody.gravityScale;
                    m_isJumping = true;
                    m_playJumpSfx = true;
                }
            }
        }
        else
        {
            m_currentJumpCooldown -= Time.fixedTime;
        }

        if (update_velocity)
        {
            m_rigidBody.velocity = current_velocity;
        }
    }

    private bool CheckGrounded()
    {
        //var result = Physics2D.BoxCast(m_rigidBody.position + m_collider.offset,
        //                               m_collider.size,
        //                               0.0f,
        //                               Vector2.down,
        //                               0.1f);

        //return result.transform != null;
        return Physics2D.IsTouchingLayers(m_collider, ground_mask);
    }
}
