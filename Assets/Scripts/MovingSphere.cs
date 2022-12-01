using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    [SerializeField, Range(1f, 100f)] private float maxSpeed = 10f;
    [SerializeField, Range(1f, 100f)] private float maxAcceleration = 10f;
    [SerializeField] private Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);
    [SerializeField, Range(0f, 1f)] private float bounciness = 0.5f; 
    private Vector3 velocity;
    
    private void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        //playerInput.Normalize();
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        //最大改变速度
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        /* if (velocity.x < desiredVelocity.x)
        // {
        //     velocity.x = Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);
        // }
        // else if (velocity.x > desiredVelocity.x)
        // {
        //     velocity.x = Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);
        // }*/
        
        //使所有方向上的速度改变不超过 预期速度 + 最大改变速度
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        
        Vector3 displacement = velocity * Time.deltaTime;
        Vector3 newPosition = transform.localPosition + displacement;
        //总体检查newPosition的位置是否在allowedArea中
        // if (!allowedArea.Contains(new Vector2(newPosition.x, newPosition.z)))
        // {
        //     //newPosition = transform.localPosition;
        //     newPosition.x = Mathf.Clamp(newPosition.x, allowedArea.xMin, allowedArea.xMax);
        //     newPosition.z = Mathf.Clamp(newPosition.z, allowedArea.yMin, allowedArea.xMax);
        // }
        //分别在对应的方向判断是否超出区域对应边，超出则把速度重置(0)/反转(-x/z)
        //bounciness 反转后速度减少的倍数
        if (newPosition.x < allowedArea.xMin)
        {
            newPosition.x = allowedArea.xMin;
            velocity.x = -velocity.x * bounciness;
        }
        else if (newPosition.x > allowedArea.xMax)
        {
            newPosition.x = allowedArea.xMax;
            velocity.x = -velocity.x * bounciness;
        }

        if (newPosition.z < allowedArea.yMin)
        {
            newPosition.z = allowedArea.yMin;
            velocity.z = -velocity.z * bounciness;
        }
        else if (newPosition.z > allowedArea.yMax)
        {
            newPosition.z = allowedArea.yMax;
            velocity.z = -velocity.z * bounciness;
        }
        
        transform.localPosition = newPosition;
    }
}
