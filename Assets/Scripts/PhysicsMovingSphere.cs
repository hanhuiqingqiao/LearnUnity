using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMovingSphere : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 10f, maxAirAcceleration = 1f;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 2f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0, 90)] private float maxGroundAngle = 25f;
    private Vector3 _velocity, _desiredVelocity;
    private Rigidbody _body;
    private bool _desiredJump;
    private int _jumpPhase;
    private float _minGroundDotProduct;
    private Vector3 _contactNormal;
    private int _groundContactCount;
    private bool OnGround => _groundContactCount > 0;

    private void OnValidate()
    {
        _minGroundDotProduct = Mathf.Cos(maxGroundAngle) * Mathf.Deg2Rad;
    }

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        OnValidate();
    }

    private void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        //playerInput.Normalize();
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        _desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        _desiredJump |= Input.GetButtonDown("Jump");
        
        GetComponent<Renderer>().material.SetColor(
            "_Color", Color.white * (_groundContactCount * 0.25f)
        );
    }

    private void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();
        // //最大改变速度 根据是否在地面上算不同的最大速度
        // float acceleration = _onGround ? maxAcceleration : maxAirAcceleration;
        // float maxSpeedChange = acceleration * Time.deltaTime;
        //
        // //使所有方向上的速度改变不超过 预期速度 + 最大改变速度
        // _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
        // _velocity.z = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);

        if (_desiredJump)
        {
            _desiredJump = false;
            Jump();
        }
        
        _body.velocity = _velocity;
        ClearState();
    }

    private void ClearState()
    {
        _groundContactCount = 0;
        _contactNormal = Vector3.zero;
    }

    private void UpdateState()
    {
        _velocity = _body.velocity;
        if (OnGround)
        {
            _jumpPhase = 0;
            if (_groundContactCount > 1)
            {
                _contactNormal.Normalize();
            }
        }
        else
        {
            _contactNormal = Vector3.up;
        }
    }

    private void Jump()
    {
        if (OnGround || _jumpPhase < maxAirJumps)
        {
            _jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(_velocity, _contactNormal);
            if (alignedSpeed > 0f)
            {
                //连续快速的空中跳跃可以达到比单次跳跃高得多的上升速度，重力在跳跃过程中可能变小
                //使第二次或后续跳跃速度不会比设定的跳跃速度高
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            //_velocity.y += jumpSpeed;
            _velocity += _contactNormal * jumpSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            //判断碰撞体碰到物体的法线方向是否是向上的  可以判断出是否碰到的是地面
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= _minGroundDotProduct)
            {
                _groundContactCount += 1;
                _contactNormal += normal;
            }
        }
    }

    private Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - _contactNormal * Vector3.Dot(vector, _contactNormal);
    }

    private void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(_velocity, xAxis);
        float currentZ = Vector3.Dot(_velocity, zAxis);
        
        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);

        _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }
}
