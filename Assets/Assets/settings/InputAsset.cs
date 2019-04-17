// GENERATED AUTOMATICALLY FROM 'Assets/settings/InputAsset.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class InputAsset : InputActionAssetReference
{
    public InputAsset()
    {
    }
    public InputAsset(InputActionAsset asset)
        : base(asset)
    {
    }
    [NonSerialized] private bool m_Initialized;
    private void Initialize()
    {
        // Clouet
        m_Clouet = asset.GetActionMap("Clouet");
        m_Clouet_WalkRun = m_Clouet.GetAction("Walk/Run");
        m_Clouet_Jump = m_Clouet.GetAction("Jump");
        m_Clouet_ShortJump = m_Clouet.GetAction("ShortJump");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Clouet = null;
        m_Clouet_WalkRun = null;
        m_Clouet_Jump = null;
        m_Clouet_ShortJump = null;
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
    private InputAction m_Clouet_WalkRun;
    private InputAction m_Clouet_Jump;
    private InputAction m_Clouet_ShortJump;
    public struct ClouetActions
    {
        private InputAsset m_Wrapper;
        public ClouetActions(InputAsset wrapper) { m_Wrapper = wrapper; }
        public InputAction @WalkRun { get { return m_Wrapper.m_Clouet_WalkRun; } }
        public InputAction @Jump { get { return m_Wrapper.m_Clouet_Jump; } }
        public InputAction @ShortJump { get { return m_Wrapper.m_Clouet_ShortJump; } }
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
    private int m_xboxSchemeIndex = -1;
    public InputControlScheme xboxScheme
    {
        get

        {
            if (m_xboxSchemeIndex == -1) m_xboxSchemeIndex = asset.GetControlSchemeIndex("xbox");
            return asset.controlSchemes[m_xboxSchemeIndex];
        }
    }
}
