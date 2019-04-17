// GENERATED AUTOMATICALLY FROM 'Assets/settings/Player controls.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class Playercontrols : InputActionAssetReference
{
    public Playercontrols()
    {
    }
    public Playercontrols(InputActionAsset asset)
        : base(asset)
    {
    }
    [NonSerialized] private bool m_Initialized;
    private void Initialize()
    {
        // Clouet
        m_Clouet = asset.GetActionMap("Clouet");
        m_Clouet_Walkrun = m_Clouet.GetAction("Walk/run");
        m_Clouet_jump = m_Clouet.GetAction("jump");
        m_Clouet_Shortjump = m_Clouet.GetAction("Short jump");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Clouet = null;
        m_Clouet_Walkrun = null;
        m_Clouet_jump = null;
        m_Clouet_Shortjump = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Clouet
    private InputActionMap m_Clouet;
    private InputAction m_Clouet_Walkrun;
    private InputAction m_Clouet_jump;
    private InputAction m_Clouet_Shortjump;
    public struct ClouetActions
    {
        private Playercontrols m_Wrapper;
        public ClouetActions(Playercontrols wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walkrun { get { return m_Wrapper.m_Clouet_Walkrun; } }
        public InputAction @jump { get { return m_Wrapper.m_Clouet_jump; } }
        public InputAction @Shortjump { get { return m_Wrapper.m_Clouet_Shortjump; } }
        public InputActionMap Get() { return m_Wrapper.m_Clouet; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(ClouetActions set) { return set.Get(); }
    }
    public ClouetActions @Clouet
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new ClouetActions(this);
        }
    }
}
