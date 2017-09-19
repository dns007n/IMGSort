using Prism.Mvvm;
using IMGSortLib.Sort;
using System.Windows.Input;
using Prism.Commands;

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
            _Logic.SourcePath = @"D:\Bilder\";
            _Logic.TargetPath = @"C:\TestX";
            _Logic.GetSourceFiles();
            _Logic.CalcTarget();
            _Logic.RemoveDuplicates();
            _Logic.CopyFilesToTarget();
        }
    }
}
