using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    [SerializeField] TestInputActions playerControls;
     public static  InputAction move;
     public static  InputAction melee;
     public static  InputAction jump;
     public static  InputAction slide;
     public static InputAction interact;
   
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    private void OnEnable()
    {   playerControls = new TestInputActions();
        InitializeAndEnableInputSystem();
        
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
        move.Disable();
        melee.Disable();
        slide.Disable();
        jump.Disable();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void InitializeAndEnableInputSystem()
    {   
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        


        slide = playerControls.Player.Slide;
        slide.Enable();

        interact = playerControls.Player.Interact;
        interact.Enable();
     
       


        melee = playerControls.Player.Attack;
        melee.Enable();

     
       
    }

}
