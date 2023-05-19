using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput; //izveidotas skripts
    public PlayerInput.OnFootActions onFoot; //izveidotais actions
    // Start is called before the first frame update
    private PlayerMotor motor;
    private PlayerLook look;
    void Awake()
    {
        playerInput = new PlayerInput(); //izsaucam jaunu objektu no ta skripta
        onFoot = playerInput.OnFoot; // izsaucam jaunu objektu

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        // anytime onfoot is performed , callback context to call a function
        onFoot.Jump.performed += ctx => motor.Jump();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {  
       motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>()); 
    }

    private void LateUpdate()
    {
       look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
