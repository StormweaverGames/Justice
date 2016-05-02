using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Tools
{
    public delegate void StateChangedEvent<State, StateChange>(State previousState, StateChange changeInvoker);

    public class State
    {
        public bool Active
        {
            get;
            private set;
        }

        public string DebugName
        {
            get;
            set;
        }

        public event EventHandler OnStateActivated;
        public event EventHandler OnStateDeactivated;

        public State() { }

        public State(string name)
        {
            DebugName = name;
        }

        public void Activate()
        {
            OnStateActivated?.Invoke(this, EventArgs.Empty);
            Active = true;
        }

        public void Deactivate()
        {
            OnStateDeactivated?.Invoke(this, EventArgs.Empty);
            Active = false;
        }

        public override int GetHashCode()
        {
            int result = 17;
            
            if (DebugName != null)
                result += 31 * DebugName.GetHashCode();

            if (OnStateActivated != null)
                result += 31 * OnStateActivated.GetHashCode();

            if (OnStateDeactivated != null)
                result += 31 * OnStateDeactivated.GetHashCode();

            return result;
        }

        public override bool Equals(object obj)
        {
            State other = obj as State;
            return other != null && OnStateActivated == other.OnStateActivated && OnStateDeactivated == other.OnStateDeactivated;
        }
    }

    public class StateChangeCommand
    {
        public event EventHandler OnFired;

        public string DebugName
        {
            get;
            set;
        }

        public StateChangeCommand() { }

        public StateChangeCommand(string name)
        {
            DebugName = name;
        }

        public void Fire()
        {
            OnFired?.Invoke(this, EventArgs.Empty);
        }
        
        public override int GetHashCode()
        {
            int result = 17;

            if (DebugName != null)
                result += 31 * DebugName.GetHashCode();

            if (OnFired != null)
                result += 31 * OnFired.GetHashCode();
            
            return result;
        }

        public override bool Equals(object obj)
        {
            StateChangeCommand other = obj as StateChangeCommand;
            return other != null && OnFired == other.OnFired;
        }
    }

    public class FiniteStateMachine
    {
        public class StateTransition
        {
            readonly State CurrentState;
            readonly StateChangeCommand Command;

            public event StateChangedEvent<State, StateChangeCommand> OnTransitioned;
            
            public StateTransition(State currentState, StateChangeCommand command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState.Equals(other.CurrentState) && this.Command.Equals(other.Command);
            }
        }

        protected Dictionary<StateTransition, State> myTransitions;

        public State CurrentState
        {
            get;
            protected set;
        }

        public FiniteStateMachine(State initialState)
        {
            CurrentState = initialState;

            myTransitions = new Dictionary<StateTransition, State>();
        }

        public void AddTransition(State startState, State endState, StateChangeCommand command)
        {
            myTransitions.Add(new StateTransition(startState, command), endState);
        }

        public State GetNext(StateChangeCommand command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            State nextState;

            if (!myTransitions.TryGetValue(transition, out nextState))
                throw new InvalidOperationException(string.Format("No transition defined for {0} => {1}", CurrentState, command));

            return nextState;
        }

        public State MoveNext(StateChangeCommand command)
        {
            State prevState = CurrentState;
            CurrentState = GetNext(command);
            prevState.Deactivate();
            command.Fire();
            CurrentState.Activate();
            return CurrentState;
        }
    }
}
