using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace QuickUnity.Core
{
    public class StateMachine<TContext>
    {
        public abstract class StateBase
        {
            internal StateMachine<TContext> StateMachine;
            internal ConcurrentDictionary<int, StateBase> StateMapping;
            protected StateMachine<TContext> GetStateMachine => StateMachine;

            protected TContext Context => StateMachine.Context;

            protected internal virtual void OnEnter()
            {
            }

            protected internal virtual void OnUpdate(float deltaTime)
            {
            }

            protected internal virtual void OnFixedUpdate(float deltaTime)
            {
            }

            protected internal virtual void OnLateUpdate(float deltaTime)
            {
            }

            protected internal virtual void OnExit()
            {
            }
        }

        public TContext Context { get; }

        private StateBase currentState;
        private StateBase nextState;
        private readonly List<StateBase> stateCacheList;
        private float updateDeltaTime;
        private float fixedUpdateDeltaTime;

        public static StateMachine<TContext> CreateStateMachine<TStartState>(TContext context)
            where TStartState : StateBase, new()
        {
            StateMachine<TContext> stateMachine = new StateMachine<TContext>(context);
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

        public void ChangeState(int triggerId)
        {
            StateBase nextState;
            if (currentState.StateMapping.TryGetValue(triggerId, out nextState))
            {
                currentState.OnExit();
                currentState = nextState;
                currentState.OnEnter();
            }
        }

        public void AddTransition<TPrevState, TNextState>(int triggerId) where TPrevState : StateBase, new()
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

        /// <summary>
        /// Initialize StateMachine.
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
            newState.StateMapping = new ConcurrentDictionary<int, StateBase>();

            return newState;
        }
    }
}
