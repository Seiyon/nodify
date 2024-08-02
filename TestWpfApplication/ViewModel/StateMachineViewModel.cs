﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodify;

namespace TestWpfApplication.ViewModel
{
    using Runner;
    using Runner.Blackboard;
    using System.Windows;

    public class StateMachineViewModel : ObservableObject
    {
        private NodifyObservableCollection<StateViewModel> _states = new NodifyObservableCollection<StateViewModel>();
        public NodifyObservableCollection<StateViewModel> States
        {
            get => _states;
            set => SetProperty(ref _states, value);
        }

        private NodifyObservableCollection<StateViewModel> _selectedStates = new NodifyObservableCollection<StateViewModel>();
        public NodifyObservableCollection<StateViewModel> SelectedStates
        {
            get => _selectedStates;
            set => SetProperty(ref _selectedStates, value);
        }

        private NodifyObservableCollection<TransitionViewModel> _connections = new NodifyObservableCollection<TransitionViewModel>();
        public NodifyObservableCollection<TransitionViewModel> Transitions
        {
            get => _connections;
            set => SetProperty(ref _connections, value);
        }

        private StateViewModel? _selectedState;
        public StateViewModel? SelectedState
        {
            get => _selectedState;
            set => SetProperty(ref _selectedState, value);
        }

        private string? _name = "State Machine";
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool IsRunning => Runner.State != MachineState.Stopped;
        public bool IsPaused => Runner.State == MachineState.Paused;

        public TransitionViewModel PendingTransition { get; }
        public StateMachineRunnerViewModel Runner { get; }
        public BlackboardViewModel Blackboard { get; }

        public INodifyCommand DeleteTransitionCommand { get; }
        public INodifyCommand DeleteSelectionCommand { get; }
        public INodifyCommand DisconnectStateCommand { get; }
        public INodifyCommand DisconnectSelectionCommand { get; }
        public INodifyCommand RenameStateCommand { get; }
        public INodifyCommand AddStateCommand { get; }
        public INodifyCommand CreateTransitionCommand { get; }
        public INodifyCommand RunCommand { get; }
        public INodifyCommand PauseCommand { get; }

        public StateMachineViewModel()
        {
            PendingTransition = new TransitionViewModel();
            Runner = new StateMachineRunnerViewModel(this);

            Blackboard = new BlackboardViewModel()
            {
                Actions = new NodifyObservableCollection<BlackboardItemReferenceViewModel>(Helpers.BlackboardDescriptor.GetAvailableItems<IBlackboardAction>()),
                Conditions = new NodifyObservableCollection<BlackboardItemReferenceViewModel>(Helpers.BlackboardDescriptor.GetAvailableItems<IBlackboardCondition>())
            };

            Transitions.WhenAdded(c => c.Source.Transitions.Add(c.Target))
            .WhenRemoved(c => c.Source.Transitions.Remove(c.Target))
            .WhenCleared(c => c.ForEach(i =>
            {
                i.Source.Transitions.Clear();
                i.Target.Transitions.Clear();
            }));

            States.WhenAdded(x => x.Graph = this)
                 .WhenRemoved(x => DisconnectState(x))
                 .WhenCleared(x =>
                 {
                     Transitions.Clear();
                     OnCreateDefaultNodes();
                 });

            OnCreateDefaultKeys();
            OnCreateDefaultNodes();

            RenameStateCommand = new RequeryCommand(() => SelectedStates[0].IsRenaming = true, () => SelectedStates.Count == 1 && SelectedStates[0].IsEditable);
            DisconnectStateCommand = new RequeryCommand<StateViewModel>(x => DisconnectState(x), x => !IsRunning && x.Transitions.Count > 0);
            DisconnectSelectionCommand = new RequeryCommand(() => SelectedStates.ForEach(x => DisconnectState(x)), () => !IsRunning && SelectedStates.Count > 0 && Transitions.Count > 0);
            DeleteSelectionCommand = new RequeryCommand(() => SelectedStates.ToList().ForEach(x => x.IsEditable.Then(() => States.Remove(x))), () => !IsRunning && (SelectedStates.Count > 1 || (SelectedStates.Count == 1 && SelectedStates[0].IsEditable)));

            AddStateCommand = new RequeryCommand<Point>(p => States.Add(new StateViewModel
            {
                Name = "New State",
                IsRenaming = true,
                Location = p,
                ActionReference = Blackboard.Actions.Count > 0 ? Blackboard.Actions[0] : null
            }), p => !IsRunning);

            CreateTransitionCommand = new DelegateCommand<(object Source, object? Target)>(s => Transitions.Add(new TransitionViewModel
            {
                Source = (StateViewModel)s.Source,
                Target = (StateViewModel)s.Target!
            }), s => !IsRunning && s.Source is StateViewModel source && s.Target is StateViewModel target && target != s.Source && target != States[0] && !source.Transitions.Contains(s.Target));

            DeleteTransitionCommand = new RequeryCommand<TransitionViewModel>(t => Transitions.Remove(t), t => !IsRunning);

            RunCommand = new RequeryCommand(() => IsRunning.Then(Runner.Stop).Else(Runner.Start), () => Transitions.Count > 0);
            PauseCommand = new RequeryCommand(Runner.TogglePause, () => IsRunning);
        }

        public void DisconnectState(StateViewModel state)
        {
            var transitions = Transitions.Where(t => t.Source == state || t.Target == state).ToList();
            transitions.ForEach(t => Transitions.Remove(t));
        }

        protected virtual void OnCreateDefaultNodes()
        {

        }

        protected virtual void OnCreateDefaultKeys()
        {

        }
    }
}
