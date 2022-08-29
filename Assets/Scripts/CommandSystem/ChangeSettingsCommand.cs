using System;
using Sever.Gridder.Data;

namespace Sever.Gridder.CommandSystem
{
    public class ChangeSettingsCommand : ICommand
    {
        private readonly Project _project;
        private readonly Action _onApplied;
        private readonly ProjectSettings _settings;
        private ProjectSettings _previousSettings;
        
        
        public ChangeSettingsCommand(Project project, ProjectSettings settings, Action onApplied)
        {
            _project = project;
            _onApplied = onApplied;
            _settings = settings;
        }

        public void Execute()
        {
            _previousSettings = _project.Settings;
            _project.UpdateSettings(_settings);
            _onApplied?.Invoke();
        }

        public void Undo()
        {
            _project.UpdateSettings(_previousSettings);
            _onApplied?.Invoke();
        }
    }
}