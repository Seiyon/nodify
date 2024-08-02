﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodify;

namespace TestWpfApplication.ViewModel
{
    public class BlackboardKeyEditorViewModel : ObservableObject
    {
        private ICollection<BlackboardKeyViewModel>? _availableKeys;
        public ICollection<BlackboardKeyViewModel>? AvailableKeys
        {
            get => _availableKeys;
            set => SetProperty(ref _availableKeys, value);
        }

        private BlackboardKeyViewModel? _target;
        public BlackboardKeyViewModel? Target
        {
            get => _target;
            set => SetProperty(ref _target, value);
        }

        private bool _canChangeInputType;
        public bool CanChangeInputType
        {
            get => _canChangeInputType;
            set => SetProperty(ref _canChangeInputType, value);
        }

        private bool _canChangeKeyType = true;
        public bool CanChangeKeyType
        {
            get => _canChangeKeyType;
            set => SetProperty(ref _canChangeKeyType, value);
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }
    }
}
