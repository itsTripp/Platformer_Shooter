// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/PlayerController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerController"",
    ""maps"": [
        {
            ""name"": ""Player_Movement"",
            ""id"": ""3962d0c8-7cdf-42ab-b46a-77f0280f527a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f440fc26-bbe8-4cc8-907d-938e7de1ef11"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""cf985a54-e41e-4808-871c-a316950594fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire_Left"",
                    ""type"": ""Button"",
                    ""id"": ""0d40ef55-f85c-42fb-aeb7-de5acafd1902"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire_Right"",
                    ""type"": ""Button"",
                    ""id"": ""497a18e0-8da3-4c18-879f-2e325627803f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5b1f7bc1-3f5a-4d3f-9163-6afaad438739"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35f1c987-99bf-4697-ab4a-d3f0d05a4049"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5b2e49f-0b0e-4c9a-8473-7cf131da8ed3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c0a1670-4926-4350-aca2-aa6258459ed3"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da0bcebd-587c-4dc1-8af7-a41a118c71fa"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""fdb3baca-e33f-45a5-bc76-15c2bd47df41"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3c5c0ebc-60bd-447e-a136-3f0207ef6546"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e6591acf-1b25-4ef2-8f22-f7bc2ad28acb"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a2457356-8f65-466b-8920-5703fe54e92a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd25741e-d099-4278-bb27-cbe993f6ba45"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player_Movement
        m_Player_Movement = asset.FindActionMap("Player_Movement", throwIfNotFound: true);
        m_Player_Movement_Move = m_Player_Movement.FindAction("Move", throwIfNotFound: true);
        m_Player_Movement_Jump = m_Player_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Player_Movement_Fire_Left = m_Player_Movement.FindAction("Fire_Left", throwIfNotFound: true);
        m_Player_Movement_Fire_Right = m_Player_Movement.FindAction("Fire_Right", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player_Movement
    private readonly InputActionMap m_Player_Movement;
    private IPlayer_MovementActions m_Player_MovementActionsCallbackInterface;
    private readonly InputAction m_Player_Movement_Move;
    private readonly InputAction m_Player_Movement_Jump;
    private readonly InputAction m_Player_Movement_Fire_Left;
    private readonly InputAction m_Player_Movement_Fire_Right;
    public struct Player_MovementActions
    {
        private @PlayerController m_Wrapper;
        public Player_MovementActions(@PlayerController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Movement_Move;
        public InputAction @Jump => m_Wrapper.m_Player_Movement_Jump;
        public InputAction @Fire_Left => m_Wrapper.m_Player_Movement_Fire_Left;
        public InputAction @Fire_Right => m_Wrapper.m_Player_Movement_Fire_Right;
        public InputActionMap Get() { return m_Wrapper.m_Player_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player_MovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayer_MovementActions instance)
        {
            if (m_Wrapper.m_Player_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnJump;
                @Fire_Left.started -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnFire_Left;
                @Fire_Left.performed -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnFire_Left;
                @Fire_Left.canceled -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnFire_Left;
                @Fire_Right.started -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnFire_Right;
                @Fire_Right.performed -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnFire_Right;
                @Fire_Right.canceled -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnFire_Right;
            }
            m_Wrapper.m_Player_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Fire_Left.started += instance.OnFire_Left;
                @Fire_Left.performed += instance.OnFire_Left;
                @Fire_Left.canceled += instance.OnFire_Left;
                @Fire_Right.started += instance.OnFire_Right;
                @Fire_Right.performed += instance.OnFire_Right;
                @Fire_Right.canceled += instance.OnFire_Right;
            }
        }
    }
    public Player_MovementActions @Player_Movement => new Player_MovementActions(this);
    public interface IPlayer_MovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnFire_Left(InputAction.CallbackContext context);
        void OnFire_Right(InputAction.CallbackContext context);
    }
}
