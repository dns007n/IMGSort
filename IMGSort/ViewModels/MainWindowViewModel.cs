using Prism.Mvvm;
using IMGSortLib.Sort;
using System.Windows.Input;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace IMGSort.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Unity Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _SourcePath;

        public string SourcePath
        {
            get { return _SourcePath; }
            set { SetProperty(ref _SourcePath, value); }
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

        public ICommand StartCommand { get; set; }

        private SortLogic _Logic;

        public MainWindowViewModel()
        {
            _Logic = new SortLogic();
            StartCommand = new DelegateCommand(() => Start());
        }

        internal void Start()
        {
            _Logic.SourcePath = SourcePath;
            _Logic.TargetPath = TargetPath;
            _Logic.SourcePath = @"D:\Test";
            _Logic.TargetPath = @"C:\TestX";
            _Logic.GetSourceFiles();
            _Logic.CalcTarget();
            _Logic.RemoveDuplicates();
            TargetList = new ObservableCollection<DeltaItem>(_Logic.FileItems);
            DuplicateCount = _Logic.Duplicates;
            ItemCount = TargetList.Count;
            //_Logic.CopyFilesToTarget();
        }
    }
}
