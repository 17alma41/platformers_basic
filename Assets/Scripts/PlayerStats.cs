using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("Ground")]
    public float groundAcceleration = 20;
    public float maxGroundHorizontalSpeed = 0;
    public float groundFriction = 0;

    [Header("Jump")]
    public int onAirJump = 1;
    public float jumpStregth = 0;
    public float maxJumpPressTime = 0.2f;

    [Header("Air")]
    public float yVelocityLowGravityThreshold = 2;
    public float defaultGravity = 3;
    public float lowGravity = 1;
    public float fallingGravity = 6;

}