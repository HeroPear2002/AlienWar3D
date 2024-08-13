using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private float speedAnimation = 2f;
    [SerializeField] MultiAimConstraint multiAimConstraint;
    //[SerializeField] private ScriptableobjectPlayer playerData;
    private PlayerInput playerInput;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction shiftAction;
    private InputAction jumpAction;
    private Animator anim;
    private float toAimValue;
    private float t_x = 0, t_y = 0, left_x = 0, left_y = 0;
    private int toAim;
    private int normalStateLayer;
    private int aimStateLayer;
    private int velocityX;
    private int velocityZ;
    private int shootHash;
    private int jumpFHash;
    private int jumpBHash;
    private bool wasPressUp = false, wasPressDown = false, wasPressLeft = false, wasPressRight = false;
    private bool isPressingUp = false, isPressingDown = false, isPressingLeft = false, isPressingRight = false;

    float bound_value_x = .5f;
    float bound_value_y = .5f;
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        upAction = playerInput.actions["Up"];
        downAction = playerInput.actions["Down"];
        leftAction = playerInput.actions["Left"];
        rightAction = playerInput.actions["Right"];
        aimAction = playerInput.actions["Aim"];
        shootAction = playerInput.actions["Shooting"];
        shiftAction = playerInput.actions["Shift"];
        jumpAction = playerInput.actions["Jump"];
        normalStateLayer = anim.GetLayerIndex("Normal State");
        aimStateLayer = anim.GetLayerIndex("Aim State");
        toAim = Animator.StringToHash("To Aim");
        velocityX = Animator.StringToHash("Velocity X");
        velocityZ = Animator.StringToHash("Velocity Z");
        shootHash = Animator.StringToHash("Shoot");
        jumpFHash = Animator.StringToHash("JumpF");
        jumpBHash = Animator.StringToHash("JumpB");

    }
    void FixedUpdate()
    {
        ChangeAnimation();
    }
    void ChangeAnimation()
    {
        if ((upAction.IsPressed() || downAction.IsPressed() || leftAction.IsPressed() || rightAction.IsPressed()) &&
            !(upAction.IsPressed() && downAction.IsPressed()) && !(leftAction.IsPressed() && rightAction.IsPressed()))
        {
            if (shiftAction.IsPressed() && !aimAction.IsPressed())
            {
                if (upAction.IsPressed())
                {
                    wasPressUp = false;
                    isPressingUp = true;
                    bound_value_y = 1f;
                }
                if (downAction.IsPressed())
                {
                    wasPressDown = false;
                    isPressingDown = true;
                    bound_value_y = -1f;
                }
                if (leftAction.IsPressed())
                {
                    wasPressLeft = false;
                    isPressingLeft = true;
                    bound_value_x = -1f;
                }
                if (rightAction.IsPressed())
                {
                    wasPressRight = false;
                    isPressingRight = true;
                    bound_value_x = 1f;
                }
            }
            else
            {
                left_x = 0;
                left_y = 0;
                if (upAction.IsPressed())
                {
                    wasPressUp = false;
                    isPressingUp = true;
                    bound_value_y = .5f;
                }
                if (downAction.IsPressed())
                {
                    wasPressDown = false;
                    isPressingDown = true;
                    bound_value_y = -.5f;
                }
                if (leftAction.IsPressed())
                {
                    wasPressLeft = false;
                    isPressingLeft = true;
                    bound_value_x = -.5f;
                }
                if (rightAction.IsPressed())
                {
                    wasPressRight = false;
                    isPressingRight = true;
                    bound_value_x = .5f;
                }
            }
            if ((leftAction.IsPressed() || rightAction.IsPressed()) && !aimAction.IsPressed()!)
            {
                if (shiftAction.triggered)
                {
                    t_x = 0;
                    left_x = anim.GetFloat(velocityX);
                }
                t_x += Time.deltaTime * speedAnimation;
                if (t_x > 1) t_x = 1;
                anim.SetFloat(velocityX, Mathf.Lerp(left_x, bound_value_x, t_x));
            }
            if (upAction.IsPressed() || downAction.IsPressed() && !aimAction.IsPressed())
            {
                if (shiftAction.triggered)
                {
                    t_y = 0;
                    left_y = anim.GetFloat(velocityZ);
                }
                t_y += Time.deltaTime * speedAnimation;
                if (t_y > 1) t_y = 1;
                anim.SetFloat(velocityZ, Mathf.Lerp(left_y, bound_value_y, t_y));
            }
        }
        if (upAction.WasReleasedThisFrame())
        {
            wasPressUp = true;
            isPressingUp = false;
        }
        if (downAction.WasReleasedThisFrame())
        {
            wasPressDown = true;
            isPressingDown = false;
        }
        if (leftAction.WasReleasedThisFrame())
        {
            wasPressLeft = true;
            isPressingLeft = false;
        }
        if (rightAction.WasReleasedThisFrame())
        {
            wasPressRight = true;
            isPressingRight = false;
        }

        if ((wasPressUp || wasPressDown) && !(wasPressUp && isPressingDown) && !(wasPressDown && isPressingUp) &&
            anim.GetFloat(velocityZ) != 0)
        {
            t_y -= Time.deltaTime * speedAnimation;
            if (t_y < 0) t_y = 0;
            anim.SetFloat(velocityZ, Mathf.Lerp(left_y, bound_value_y, t_y));
            if (anim.GetFloat(velocityZ) == 0)
            {
                wasPressUp = false;
                wasPressDown = false;
            }
        }
        if ((wasPressLeft || wasPressRight) && !(wasPressLeft && isPressingRight) && !(wasPressRight && isPressingLeft)
            && anim.GetFloat(velocityX) != 0)
        {
            t_x -= Time.deltaTime * speedAnimation;
            if (t_x < 0) t_x = 0;
            anim.SetFloat(velocityX, Mathf.Lerp(left_x, bound_value_x, t_x));
            if (anim.GetFloat(velocityX) == 0)
            {
                wasPressLeft = false;
                wasPressRight = false;
            }
        }
        if(jumpAction.WasPressedThisFrame() || (jumpAction.WasPressedThisFrame() && upAction.WasPressedThisFrame()))
        {
            anim.CrossFade(jumpFHash, .4f);
        }
        else if(jumpAction.WasPressedThisFrame() && downAction.WasPressedThisFrame())
        {
            anim.CrossFade(jumpBHash, .4f);
        }
        if (aimAction.IsPressed())
        {
            toAimValue += speedAnimation * Time.deltaTime;
            if (toAimValue > 1) toAimValue = 1;
            anim.SetFloat(toAim, Mathf.Lerp(0, 1, toAimValue));
            anim.SetLayerWeight(aimStateLayer, Mathf.Lerp(0, 1, toAimValue));
            anim.SetLayerWeight(normalStateLayer, Mathf.Lerp(1, 0, toAimValue));
            multiAimConstraint.data.offset = new Vector3(0f, 50f, 20f);
            if (shootAction.WasPressedThisFrame()) anim.CrossFade(shootHash, .15f);
        }
        else
        {
            toAimValue -= speedAnimation * Time.deltaTime;
            if (toAimValue < 0) toAimValue = 0;
            anim.SetFloat(toAim, Mathf.Lerp(0, 1, toAimValue));
            anim.SetLayerWeight(aimStateLayer, Mathf.Lerp(0, 1, toAimValue));
            anim.SetLayerWeight(normalStateLayer, Mathf.Lerp(1, 0, toAimValue));
            multiAimConstraint.data.offset = new Vector3(8f, 20f, 10f);
        }
    }
}