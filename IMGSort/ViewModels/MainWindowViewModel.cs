using IMGSortLib.Sort;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace IMGSort.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region BindingProperties
        private string _title = "Bilder Sortierer";
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

        private bool _PBEnabled;
        public bool PBEnabled
        {
            get { return _PBEnabled; }
            set { SetProperty(ref _PBEnabled, value); }
        }
        private bool _PBIndeterminateEnabled;
        public bool PBIndeterminateEnabled
        {
            get { return _PBIndeterminateEnabled; }
            set { SetProperty(ref _PBIndeterminateEnabled, value); }
        }

        #endregion

        public ICommand SearchCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand SourceSelectCommand { get; set; }
        public ICommand SourceRemoveCommand { get; set; }
        public ICommand TargetSelectCommand { get; set; }
        public ICommand SourceRemoveAllCommand { get; set; }
        private SortLogic _Logic;

        public MainWindowViewModel()
        {
            _Logic = new SortLogic();
            SearchCommand = new DelegateCommand(() => Search());
            CopyCommand = new DelegateCommand(() => CopyContentAsync());
            SourceSelectCommand = new DelegateCommand(() => SelectSourcePath());
            SourceRemoveCommand = new DelegateCommand(() => SelectRemovePath());
            TargetSelectCommand = new DelegateCommand(() => SelectTargetPath());
            SourceRemoveAllCommand = new DelegateCommand(() => SelectRemoveAllPaths());
            if (Properties.Settings.Default.LastSourcePath == null)
            {
                Properties.Settings.Default.LastSourcePath = new System.Collections.Specialized.StringCollection();
            }
            SourcePaths = new ObservableCollection<string>(Properties.Settings.Default.LastSourcePath.Cast<string>());
            TargetPath = Properties.Settings.Default.LastTargetPath;
        }

        private void SelectRemoveAllPaths()
        {
            SourcePaths.Clear();
            Properties.Settings.Default.LastSourcePath.Clear();
            Properties.Settings.Default.Save();
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

        internal async void Search()
        {
            PBEnabled = true;
            PBIndeterminateEnabled = true;
            _Logic.SourcePaths = SourcePaths;
            _Logic.TargetPath = TargetPath;
            await _Logic.GetSourceFilesAsync();
            await _Logic.CalcTargetAsync();
            await _Logic.RemoveDuplicatesAsync();
            TargetList = new ObservableCollection<DeltaItem>(_Logic.FileItems.OrderBy(x => x.SourceFile.CreationTime));
            DuplicateCount = _Logic.Duplicates;
            ItemCount = TargetList.Count;
            PBIndeterminateEnabled = false;
            PBEnabled = false;
        }

        private async void CopyContentAsync()
        {
            PBEnabled = true;
            PBIndeterminateEnabled = true;
            await _Logic.CopyFilesToTargetAsync();
            PBIndeterminateEnabled = false;
            PBEnabled = false;
        }

    }
}
