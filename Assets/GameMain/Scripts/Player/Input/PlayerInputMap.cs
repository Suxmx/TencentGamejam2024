//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.1
//     from Assets/GameMain/Scripts/Player/Input/PlayerInputMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Tencent
{
    public partial class @PlayerInputMap: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInputMap()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputMap"",
    ""maps"": [
        {
            ""name"": ""GroundMove"",
            ""id"": ""add53290-b4c0-46c4-a809-037ae9ae46a2"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d48fbd46-bf59-4026-83eb-927add0e62d4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""542a62cd-c314-4919-b09f-158e3491b188"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""7baa6f5b-c808-4419-9f86-95d9067d624d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""9ff32371-4ad9-4ba6-8da7-c0ec994696fb"",
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
                    ""id"": ""3708d966-b1e8-4f43-83c6-f672c5dd430c"",
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
                    ""id"": ""639b33a5-ce9d-44bf-9910-b343227abaa9"",
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
                    ""id"": ""8c091b0f-0e1d-44d4-9267-075d37fd034d"",
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
                    ""id"": ""ed9c5fa7-f93a-4c92-973c-ceb1618fd751"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""def1cd7e-d63d-4b5d-9598-df862205f6ff"",
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
                    ""id"": ""02a7f2df-83fb-4c77-9104-421954664f99"",
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
                    ""id"": ""d37c3171-e3a7-4995-9ab0-f241a6a8c767"",
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
                    ""id"": ""e8c4a465-ce77-4c4e-ab86-42e42d1a35b4"",
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
                    ""id"": ""be004127-7442-4917-86ba-621ff38a0928"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""40d3b6c3-8d80-4ab5-8acd-fc72d86c83b1"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd18c8c9-00d1-471d-928e-d29ea380db91"",
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
                    ""id"": ""8464725e-c737-47fb-924f-9475ce1cc0d8"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""5ad1e505-4b4f-4cdc-83fa-0c4d0c9b4e5c"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""3c9971a8-62f8-48a6-9045-01a30407c58d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""65d65e12-ce90-447b-9a31-9a14fef2571a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MaterialGun"",
            ""id"": ""57401740-cd2a-499c-bb8b-0aaa87e31fab"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""ccf02268-029b-43a5-b5cf-64861df74c8e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d5aa1494-164e-42b1-908f-b3f4e1083c07"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // GroundMove
            m_GroundMove = asset.FindActionMap("GroundMove", throwIfNotFound: true);
            m_GroundMove_Move = m_GroundMove.FindAction("Move", throwIfNotFound: true);
            m_GroundMove_Jump = m_GroundMove.FindAction("Jump", throwIfNotFound: true);
            m_GroundMove_Crouch = m_GroundMove.FindAction("Crouch", throwIfNotFound: true);
            // Camera
            m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
            m_Camera_Look = m_Camera.FindAction("Look", throwIfNotFound: true);
            // MaterialGun
            m_MaterialGun = asset.FindActionMap("MaterialGun", throwIfNotFound: true);
            m_MaterialGun_Fire = m_MaterialGun.FindAction("Fire", throwIfNotFound: true);
        }

        ~@PlayerInputMap()
        {
            UnityEngine.Debug.Assert(!m_GroundMove.enabled, "This will cause a leak and performance issues, PlayerInputMap.GroundMove.Disable() has not been called.");
            UnityEngine.Debug.Assert(!m_Camera.enabled, "This will cause a leak and performance issues, PlayerInputMap.Camera.Disable() has not been called.");
            UnityEngine.Debug.Assert(!m_MaterialGun.enabled, "This will cause a leak and performance issues, PlayerInputMap.MaterialGun.Disable() has not been called.");
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

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // GroundMove
        private readonly InputActionMap m_GroundMove;
        private List<IGroundMoveActions> m_GroundMoveActionsCallbackInterfaces = new List<IGroundMoveActions>();
        private readonly InputAction m_GroundMove_Move;
        private readonly InputAction m_GroundMove_Jump;
        private readonly InputAction m_GroundMove_Crouch;
        public struct GroundMoveActions
        {
            private @PlayerInputMap m_Wrapper;
            public GroundMoveActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_GroundMove_Move;
            public InputAction @Jump => m_Wrapper.m_GroundMove_Jump;
            public InputAction @Crouch => m_Wrapper.m_GroundMove_Crouch;
            public InputActionMap Get() { return m_Wrapper.m_GroundMove; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GroundMoveActions set) { return set.Get(); }
            public void AddCallbacks(IGroundMoveActions instance)
            {
                if (instance == null || m_Wrapper.m_GroundMoveActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GroundMoveActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
            }

            private void UnregisterCallbacks(IGroundMoveActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Jump.started -= instance.OnJump;
                @Jump.performed -= instance.OnJump;
                @Jump.canceled -= instance.OnJump;
                @Crouch.started -= instance.OnCrouch;
                @Crouch.performed -= instance.OnCrouch;
                @Crouch.canceled -= instance.OnCrouch;
            }

            public void RemoveCallbacks(IGroundMoveActions instance)
            {
                if (m_Wrapper.m_GroundMoveActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGroundMoveActions instance)
            {
                foreach (var item in m_Wrapper.m_GroundMoveActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GroundMoveActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GroundMoveActions @GroundMove => new GroundMoveActions(this);

        // Camera
        private readonly InputActionMap m_Camera;
        private List<ICameraActions> m_CameraActionsCallbackInterfaces = new List<ICameraActions>();
        private readonly InputAction m_Camera_Look;
        public struct CameraActions
        {
            private @PlayerInputMap m_Wrapper;
            public CameraActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Look => m_Wrapper.m_Camera_Look;
            public InputActionMap Get() { return m_Wrapper.m_Camera; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
            public void AddCallbacks(ICameraActions instance)
            {
                if (instance == null || m_Wrapper.m_CameraActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CameraActionsCallbackInterfaces.Add(instance);
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }

            private void UnregisterCallbacks(ICameraActions instance)
            {
                @Look.started -= instance.OnLook;
                @Look.performed -= instance.OnLook;
                @Look.canceled -= instance.OnLook;
            }

            public void RemoveCallbacks(ICameraActions instance)
            {
                if (m_Wrapper.m_CameraActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICameraActions instance)
            {
                foreach (var item in m_Wrapper.m_CameraActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CameraActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CameraActions @Camera => new CameraActions(this);

        // MaterialGun
        private readonly InputActionMap m_MaterialGun;
        private List<IMaterialGunActions> m_MaterialGunActionsCallbackInterfaces = new List<IMaterialGunActions>();
        private readonly InputAction m_MaterialGun_Fire;
        public struct MaterialGunActions
        {
            private @PlayerInputMap m_Wrapper;
            public MaterialGunActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Fire => m_Wrapper.m_MaterialGun_Fire;
            public InputActionMap Get() { return m_Wrapper.m_MaterialGun; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MaterialGunActions set) { return set.Get(); }
            public void AddCallbacks(IMaterialGunActions instance)
            {
                if (instance == null || m_Wrapper.m_MaterialGunActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MaterialGunActionsCallbackInterfaces.Add(instance);
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
            }

            private void UnregisterCallbacks(IMaterialGunActions instance)
            {
                @Fire.started -= instance.OnFire;
                @Fire.performed -= instance.OnFire;
                @Fire.canceled -= instance.OnFire;
            }

            public void RemoveCallbacks(IMaterialGunActions instance)
            {
                if (m_Wrapper.m_MaterialGunActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMaterialGunActions instance)
            {
                foreach (var item in m_Wrapper.m_MaterialGunActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MaterialGunActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MaterialGunActions @MaterialGun => new MaterialGunActions(this);
        public interface IGroundMoveActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnCrouch(InputAction.CallbackContext context);
        }
        public interface ICameraActions
        {
            void OnLook(InputAction.CallbackContext context);
        }
        public interface IMaterialGunActions
        {
            void OnFire(InputAction.CallbackContext context);
        }
    }
}
