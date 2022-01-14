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
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""cf985a54-e41e-4808-871c-a316950594fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerMove"",
                    ""type"": ""Value"",
                    ""id"": ""c5e98e6c-aa02-4993-b873-b2e04b6082ca"",
                    ""expectedControlType"": """",
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
                    ""name"": ""1D Axis"",
                    ""id"": ""921c2e47-269d-4a45-b335-2f8954b57006"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e14399a0-9d51-4ac6-ba3e-41c9579f4607"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a2a0e989-df34-42cb-aa07-b5299b277484"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cd5d80ca-fa49-434d-a771-026e0c34a47e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMove"",
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
        m_Player_Movement_Jump = m_Player_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Player_Movement_PlayerMove = m_Player_Movement.FindAction("PlayerMove", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Movement_Jump;
    private readonly InputAction m_Player_Movement_PlayerMove;
    public struct Player_MovementActions
    {
        private @PlayerController m_Wrapper;
        public Player_MovementActions(@PlayerController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Player_Movement_Jump;
        public InputAction @PlayerMove => m_Wrapper.m_Player_Movement_PlayerMove;
        public InputActionMap Get() { return m_Wrapper.m_Player_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player_MovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayer_MovementActions instance)
        {
            if (m_Wrapper.m_Player_MovementActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnJump;
                @PlayerMove.started -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnPlayerMove;
                @PlayerMove.performed -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnPlayerMove;
                @PlayerMove.canceled -= m_Wrapper.m_Player_MovementActionsCallbackInterface.OnPlayerMove;
            }
            m_Wrapper.m_Player_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @PlayerMove.started += instance.OnPlayerMove;
                @PlayerMove.performed += instance.OnPlayerMove;
                @PlayerMove.canceled += instance.OnPlayerMove;
            }
        }
    }
    public Player_MovementActions @Player_Movement => new Player_MovementActions(this);
    public interface IPlayer_MovementActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnPlayerMove(InputAction.CallbackContext context);
    }
}
