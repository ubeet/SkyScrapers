﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CinemachineInputProviderCustom : MonoBehaviour, AxisState.IInputAxisProvider
{
    [SerializeField] private InputActionReference secondaryAction;
    public bool _isBlocked = false;


    /// <summary>
    /// Leave this at -1 for single-player games.
    /// For multi-player games, set this to be the player index, and the actions will
    /// be read from that player's controls
    /// </summary>
    [Tooltip("Leave this at -1 for single-player games.  "
             + "For multi-player games, set this to be the player index, and the actions will "
             + "be read from that player's controls")]
    public int PlayerIndex = -1;

    /// <summary>If set, Input Actions will be auto-enabled at start</summary>
    [Tooltip("If set, Input Actions will be auto-enabled at start")]
    public bool AutoEnableInputs = true;

    /// <summary>Vector2 action for XY movement</summary>
    [Tooltip("Vector2 action for XY movement")]
    public InputActionReference XYAxis;

    /// <summary>Float action for Z movement</summary>
    [Tooltip("Float action for Z movement")]
    public InputActionReference ZAxis;

    /// <summary>
    /// Implementation of AxisState.IInputAxisProvider.GetAxisValue().
    /// Axis index ranges from 0...2 for X, Y, and Z.
    /// Reads the action associated with the axis.
    /// </summary>
    /// <param name="axis"></param>
    /// <returns>The current axis value</returns>
    public virtual float GetAxisValue(int axis)
    {
        if (enabled)
        {
            var action = ResolveForPlayer(axis, axis == 2 ? ZAxis : XYAxis);
            if (action != null && !_isBlocked)
            {
                if (Mouse.current.middleButton.isPressed)
                {
                    switch (axis)
                    {
                        case 0: return action.ReadValue<Vector2>().x;
                        case 1: return action.ReadValue<Vector2>().y;
                    }
                }

                switch (axis)
                {
                    case 2: return action.ReadValue<float>();
                }
                
            }
        }
        
        return 0f;
    }

    public void Block() => _isBlocked = true;
    public void UnBlock() => _isBlocked = false;

    const int NUM_AXES = 3;
    InputAction[] m_cachedActions;

    /// <summary>
    /// In a multi-player context, actions are associated with specific players
    /// This resolves the appropriate action reference for the specified player.
    /// 
    /// Because the resolution involves a search, we also cache the returned 
    /// action to make future resolutions faster.
    /// </summary>
    /// <param name="axis">Which input axis (0, 1, or 2)</param>
    /// <param name="actionRef">Which action reference to resolve</param>
    /// <returns>The cached action for the player specified in PlayerIndex</returns>
    protected InputAction ResolveForPlayer(int axis, InputActionReference actionRef)
    {
        if (axis < 0 || axis >= NUM_AXES)
            return null;
        if (actionRef == null || actionRef.action == null)
            return null;
        if (m_cachedActions == null || m_cachedActions.Length != NUM_AXES)
            m_cachedActions = new InputAction[NUM_AXES];
        if (m_cachedActions[axis] != null && actionRef.action.id != m_cachedActions[axis].id)
            m_cachedActions[axis] = null;

        if (m_cachedActions[axis] == null)
        {
            m_cachedActions[axis] = actionRef.action;
            if (PlayerIndex != -1)
                m_cachedActions[axis] = GetFirstMatch(InputUser.all[PlayerIndex], actionRef);

            if (AutoEnableInputs && actionRef != null && actionRef.action != null)
                actionRef.action.Enable();
        }

        // Update enabled status
        if (m_cachedActions[axis] != null && m_cachedActions[axis].enabled != actionRef.action.enabled)
        {
            if (actionRef.action.enabled)
                m_cachedActions[axis].Enable();
            else
                m_cachedActions[axis].Disable();
        }

        return m_cachedActions[axis];

        // local function to wrap the lambda which otherwise causes a tiny gc
        InputAction GetFirstMatch(in InputUser user, InputActionReference aRef) =>
            user.actions.First(x => x.id == aRef.action.id);
    }

    // Clean up
    protected virtual void OnDisable()
    {
        m_cachedActions = null;
    }
}