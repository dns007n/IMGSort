using Prism.Mvvm;
using IMGSortLib.Sort;
using System.Windows.Input;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Linq;

namespace IMGSort.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region BindingProperties
        private string _title = "Prism Unity Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private ObservableCollection<string> _SourcePath;

        public ObservableCollection<string> SourcePaths
        {
            get { return _SourcePath; }
            set { SetProperty(ref _SourcePath, value); }
        }

        private string _SelectedSourceItem;
        public string SelectedSourceItem
        {
            get { return _SelectedSourceItem; }
            set { SetProperty(ref _SelectedSourceItem, value); }
        }
        private string _TargetPath;

        public string TargetPath
        {
            get { return _TargetPath; }
            set { SetProperty(ref _TargetPath, value); }
        }
        private int _DuplicateCount;

        public int DuplicateCount
        {
            get { return _DuplicateCount; }
            set { SetProperty(ref _DuplicateCount, value); }
        }
        private int _ItemCount;

        public int ItemCount
        {
            get { return _ItemCount; }
            set { SetProperty(ref _ItemCount, value); }
        }

        private ObservableCollection<DeltaItem> _TargetList;

        public ObservableCollection<DeltaItem> TargetList
        {
            get { return _TargetList; }
            set { SetProperty(ref _TargetList, value); }
        }

        #endregion

        public ICommand StartCommand { get; set; }
        public ICommand SourceSelectCommand { get; set; }
        public ICommand SourceRemoveCommand { get; set; }
        public ICommand TargetSelectCommand { get; set; }

        private SortLogic _Logic;

        public MainWindowViewModel()
        {
            _Logic = new SortLogic();
            StartCommand = new DelegateCommand(() => Start());
            SourceSelectCommand = new DelegateCommand(() => SelectSourcePath());
            SourceRemoveCommand = new DelegateCommand(() => SelectRemovePath());
            TargetSelectCommand = new DelegateCommand(() => SelectTargetPath());
            if (Properties.Settings.Default.LastSourcePath == null)
            {
                Properties.Settings.Default.LastSourcePath = new System.Collections.Specialized.StringCollection();
            }
            SourcePaths = new ObservableCollection<string>(Properties.Settings.Default.LastSourcePath.Cast<string>());
            TargetPath = Properties.Settings.Default.LastTargetPath;
        }

        private void SelectRemovePath()
        {
            SourcePaths.Remove(SelectedSourceItem);
            Properties.Settings.Default.LastSourcePath.Remove(SelectedSourceItem);
            Properties.Settings.Default.Save();
        }

        private void SelectSourcePath()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = SourcePaths.LastOrDefault();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok && !SourcePaths.Contains(dialog.FileName))
            {
                SourcePaths.Add(dialog.FileName);
                Properties.Settings.Default.LastSourcePath.Add(dialog.FileName);
                Properties.Settings.Default.Save();
            }
        }
        private void SelectTargetPath()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = TargetPath;
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                TargetPath = dialog.FileName;
                Properties.Settings.Default.LastTargetPath = TargetPath;
                Properties.Settings.Default.Save();
            }
        }

        internal async void Start()
        {
            _Logic.SourcePaths = SourcePaths;
            _Logic.TargetPath = TargetPath;
            await _Logic.GetSourceFilesAsync();
            await _Logic.CalcTargetAsync();
            await _Logic.RemoveDuplicatesAsync();
            TargetList = new ObservableCollection<DeltaItem>(_Logic.FileItems);
            DuplicateCount = _Logic.Duplicates;
            ItemCount = TargetList.Count;
            //await _Logic.CopyFilesToTargetAsync();
        }
    }
}
