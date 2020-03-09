using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Atlassian.Jira;
using AutoMapper.QueryableExtensions;
using JetBrains.Annotations;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Rapport.Contracts;
using Rapport.Data.Models;

namespace Rapport.ViewModels
{
    public class TrackedIssuePageViewModel : RefreshViewModelBase, IActiveAware
    {
        private readonly IJiraService _jiraService;
        public event EventHandler IsActiveChanged;

        public TrackedIssuePageViewModel() : base(null)
        {
            // Design Time Constructor
        }

        public TrackedIssuePageViewModel(
            IJiraService jiraService,
            INavigationService navigationService) : base(navigationService)
        {
            _jiraService = jiraService;
            Title = "Tracking";
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        private bool _showHint;

        public bool ShowHint
        {
            get { return _showHint; }
            set { SetProperty(ref _showHint, value); }
        }

        private IssueModel _selectedIssue;
        public IssueModel SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                var oldValue = value;
                var changed = SetProperty(ref _selectedIssue, value);
                if (changed)
                {
                    SelectedIssueChanged(oldValue, _selectedIssue);
                }
            }
        }

        private void SelectedIssueChanged([CanBeNull] IssueModel oldValue, [CanBeNull] IssueModel newValue)
        {
            if (oldValue != null)
            {
                _ = _jiraService.EndWorkingOnIssueAsync(oldValue).ConfigureAwait(false);
            }

            if (newValue != null)
            {
                _ = _jiraService.BeginWorkingOnIssueAsync(newValue).ConfigureAwait(false);
            }
        }

        public IList<IssueModel> TrackedIssues { get; } = new ObservableCollection<IssueModel>();

        private ICommand _deleteTrackedIssueCommand;

        public ICommand DeleteTrackedIssueCommand =>
            _deleteTrackedIssueCommand ?? (_deleteTrackedIssueCommand =
                new DelegateCommand<IssueModel>(ExecuteDeleteTrackedIssueCommand));

        private void ExecuteDeleteTrackedIssueCommand(IssueModel issue)
        {
            TrackedIssues.Remove(issue);
            _ = _jiraService.RemoveTrackedIssueAsync(issue).ConfigureAwait(false);
            ShowHint = !TrackedIssues.Any();
        }

        protected virtual void RaiseIsActiveChanged()
        {
            if (RefreshCommand.CanExecute())
            {
                RefreshCommand.Execute();
            }
        }

        protected override async Task RefreshAsync()
        {
            if (IsActive)
            {
                TrackedIssues.Clear();

                var activeIssues = await _jiraService.GetTrackedIssuesAsync().ConfigureAwait(true);

                foreach (var issue in activeIssues)
                {
                    TrackedIssues.Add(issue);
                }

                ShowHint = !TrackedIssues.Any();
            }
        }
    }
}
