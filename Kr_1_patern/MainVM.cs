using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Kr_1_patern
{
    /// This program can find first string match in directory files
    /// <inheritdoc />
    internal class MainVM : BindableBase
    {
        private string _searchStr;

        private ObservableCollection<string> _files;
        private ObservableCollection<int> _indexis;

        public string SearchStr
        {
            get => _searchStr;
            set => _searchStr = value;
        }

        public ObservableCollection<string> Files => _files;

        public ObservableCollection<int> Indexis => _indexis;

        public MainVM()
        {
            _files = new ObservableCollection<string>();
            _indexis = new ObservableCollection<int>();

            StartCommand = new DelegateCommand(Start);
        }


        public DelegateCommand StartCommand{
            get;
        }

        public void Start()
        {
            if (SearchStr == null || SearchStr == "")
            if(_files.Any()) 
                _files.Clear();

            if (_indexis.Any())
                _indexis.Clear();

            var folderBrowser = new FolderBrowserDialog();

            var result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                var files = Directory.GetFiles(folderBrowser.SelectedPath, "*.txt", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    using (var sr = new StreamReader(file))
                    {
                        string str;
                        var index = 0;
                        while ((str = sr.ReadLine())!=null)
                        {
                            if (str.Contains(SearchStr))
                            {
                                _files.Add(file);
                                _indexis.Add(index);
                                OnPropertyChanged("Files");
                                OnPropertyChanged("Indexis");
                                break;
                                
                            }
                            index++;
                        }

                    }
                }

            }


            
        }
    }
}
