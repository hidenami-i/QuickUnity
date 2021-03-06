using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace QuickUnity.Core
{
    public class StateMachine<TContext, TTriggerType> where TTriggerType : Enum
    {
        public abstract class StateBase
        {
            internal StateMachine<TContext, TTriggerType> StateMachine;
            internal ConcurrentDictionary<TTriggerType, StateBase> StateMapping;
            protected StateMachine<TContext, TTriggerType> GetStateMachine => StateMachine;

            protected TContext Context => StateMachine.Context;
            protected internal virtual void OnEnter() { }
            protected internal virtual void OnUpdate(float deltaTime) { }
            protected internal virtual void OnFixedUpdate(float deltaTime) { }
            protected internal virtual void OnLateUpdate(float deltaTime) { }
            protected internal virtual void OnExit() { }
            protected internal virtual void OnError(Exception exception) { }
        }

        private sealed class AnyState : StateBase { }

        public TContext Context { get; }
        private StateBase currentState;
        private StateBase nextState;
        private readonly List<StateBase> stateCacheList;
        private float updateDeltaTime;
        private float fixedUpdateDeltaTime;
        public string GetCurrentStateName => currentState.GetType().Name;

        public static StateMachine<TContext, TTriggerType> CreateStateMachine(TContext context)
        {
            return CreateStateMachine<AnyState>(context);
        }

        public static StateMachine<TContext, TTriggerType> CreateStateMachine<TStartState>(TContext context)
            where TStartState : StateBase, new()
        {
            if (context == null)
            {
                throw new ArgumentNullException($"{nameof(context)} is null.");
            }

            StateMachine<TContext, TTriggerType> stateMachine = new StateMachine<TContext, TTriggerType>(context);
            stateMachine.SetStartState<TStartState>();
            return stateMachine;
        }

        public void UpdateMe(float deltaTime)
        {
            currentState?.OnUpdate(deltaTime);
        }

        public void FixedUpdateMe(float deltaTime)
        {
            currentState?.OnFixedUpdate(deltaTime);
        }

        public void LateUpdateMe(float deltaTime)
        {
            currentState?.OnFixedUpdate(deltaTime);
        }

        public void ChangeState(TTriggerType triggerId)
        {
            if (currentState.StateMapping.TryGetValue(triggerId, out StateBase state))
            {
                try
                {
                    currentState.OnExit();
                }
                catch (Exception exception)
                {
                    currentState.OnError(exception);
                }

                try
                {
                    currentState = state;
                    currentState.OnEnter();
                }
                catch (Exception exception)
                {
                    currentState.OnError(exception);
                }
            }
        }

        public void ChangeAnyState()
        {
            if (currentState is AnyState)
            {
                return;
            }

            currentState.OnExit();
            currentState = GetOrCreateState<AnyState>();
            currentState.OnEnter();
        }

        public void AddTransitionFromAny<TNextState>(TTriggerType triggerId) where TNextState : StateBase, new()
        {
            AddTransition<AnyState, TNextState>(triggerId);
        }

        public void AddTransition<TPrevState, TNextState>(TTriggerType triggerId) where TPrevState : StateBase, new()
            where TNextState : StateBase, new()
        {
            StateBase prevState = GetOrCreateState<TPrevState>();

            if (prevState.StateMapping.ContainsKey(triggerId))
            {
                throw new ArgumentException(
                    $"An event id {triggerId} has already been set for {prevState.GetType().Name} state.");
            }

            prevState.StateMapping[triggerId] = GetOrCreateState<TNextState>();
        }

        private StateMachine() { }

        /// <summary>
        /// Setup StateMachine.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private StateMachine(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException($"{typeof(TContext)} is null.");
            }

            Context = context;
            stateCacheList = new List<StateBase>();
        }

        private void SetStartState<TStartStat>() where TStartStat : StateBase, new()
        {
            currentState = GetOrCreateState<TStartStat>();
            currentState.OnEnter();
        }

        private StateBase GetOrCreateState<TState>() where TState : StateBase, new()
        {
            foreach (StateBase state in stateCacheList)
            {
                if (state.GetType() == typeof(TState))
                {
                    return state;
                }
            }

            StateBase newState = Activator.CreateInstance<TState>();
            stateCacheList.Add(newState);
            newState.StateMachine = this;
            newState.StateMapping = new ConcurrentDictionary<TTriggerType, StateBase>();

            return newState;
        }
    }
}
