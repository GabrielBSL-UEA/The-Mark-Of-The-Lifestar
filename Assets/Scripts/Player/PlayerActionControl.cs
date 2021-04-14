// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Player
{
    public class @PlayerActionControl : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerActionControl()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Land"",
            ""id"": ""146f81bb-94ec-4ee3-be38-087382aa3c6c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""85bba435-6345-483e-94cf-b7bb5d9816b0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""4a5a6460-8761-4ece-b5c8-cb3afc8c78b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""d2f78390-b61d-422c-8ccf-633ae0c28dbc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c2b0c839-35d6-4a05-87ac-3abff7f10cfe"",
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
                    ""id"": ""060c26b8-42f4-4648-900b-be06cd30a869"",
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
                    ""id"": ""d31434f9-7bcb-49c9-829a-8c7bdf0baa8e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d724477c-1cfb-4887-addc-fdc2dd38797b"",
                    ""path"": ""<Keyboard>/rightShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""190e44e2-6cfc-4507-a69f-2928678f1f10"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4862c92e-d5c8-464c-a2a6-6578ce46f2e5"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5357411-4d4a-47e9-9948-fbf9d2792447"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard (Arrows)"",
                    ""id"": ""5f9d2882-db46-4846-a38c-dafd4e805b19"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""29c32035-92d7-482b-97e7-cb6c7b6fdf9a"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""201c09aa-b7ec-4608-98f4-7cf7f2118255"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""27e392fb-6db7-47e9-9d0a-6d5df21fa4e9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f9eab36e-13a7-4c11-ac95-9f599b542138"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard (WASD)"",
                    ""id"": ""1a9f9967-2be6-4fdf-94e1-7f8190477afe"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb3dda07-df76-4bf2-a9b8-1bc823e9f09e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""81cf02c2-b79a-462c-b031-d32935249bc6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""526a760c-0a4b-4c79-b2e0-af456bce007d"",
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
                    ""id"": ""d7401289-5ea3-4854-a2f7-1365bf3de5e0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Land
            m_Land = asset.FindActionMap("Land", throwIfNotFound: true);
            m_Land_Move = m_Land.FindAction("Move", throwIfNotFound: true);
            m_Land_Jump = m_Land.FindAction("Jump", throwIfNotFound: true);
            m_Land_Dash = m_Land.FindAction("Dash", throwIfNotFound: true);
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

        // Land
        private readonly InputActionMap m_Land;
        private ILandActions m_LandActionsCallbackInterface;
        private readonly InputAction m_Land_Move;
        private readonly InputAction m_Land_Jump;
        private readonly InputAction m_Land_Dash;
        public struct LandActions
        {
            private @PlayerActionControl m_Wrapper;
            public LandActions(@PlayerActionControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Land_Move;
            public InputAction @Jump => m_Wrapper.m_Land_Jump;
            public InputAction @Dash => m_Wrapper.m_Land_Dash;
            public InputActionMap Get() { return m_Wrapper.m_Land; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(LandActions set) { return set.Get(); }
            public void SetCallbacks(ILandActions instance)
            {
                if (m_Wrapper.m_LandActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                    @Jump.started -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                    @Dash.started -= m_Wrapper.m_LandActionsCallbackInterface.OnDash;
                    @Dash.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnDash;
                    @Dash.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnDash;
                }
                m_Wrapper.m_LandActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Dash.started += instance.OnDash;
                    @Dash.performed += instance.OnDash;
                    @Dash.canceled += instance.OnDash;
                }
            }
        }
        public LandActions @Land => new LandActions(this);
        public interface ILandActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
        }
    }
}
