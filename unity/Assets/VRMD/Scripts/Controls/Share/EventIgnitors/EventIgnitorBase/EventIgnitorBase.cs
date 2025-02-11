/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public abstract class EventIgnitorBase : MonoBehaviour
{
    public UnityEvent OnEventStart = new UnityEvent();
    public UnityEvent OnEventEnd = new UnityEvent();
    public UnityEvent OnEvent = new UnityEvent();

    private bool IsOnEvent = false;

    private static HashSet<EventIgnitorBase> setOfIgnitingEvents = new HashSet<EventIgnitorBase>();
    public static HashSet<EventIgnitorBase> SetOfIgnitingEvents

    {
        get
        {
            return setOfIgnitingEvents;
        }
    }

    void Awake()
    {
        OnEventStart.AddListener(this.OnStart);
        OnEventEnd.AddListener(this.OnEnd);
    }

    void Update()
    {
        if (this.IsOnEvent)
        {
            this.OnEvent.Invoke();
        }
    }

    void OnStart()
    {
        this.IsOnEvent = true;
        setOfIgnitingEvents.Add(this);
    }

    void OnEnd()
    {
        this.IsOnEvent = false;
        setOfIgnitingEvents.Remove(this);
    }
}

