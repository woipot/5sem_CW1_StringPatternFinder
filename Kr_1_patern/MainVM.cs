using System.Collections.Generic;
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
        private readonly ObservableCollection<string> _files;
        private readonly ObservableCollection<int> _indexis;

        public string SearchStr { get; set; }

        public IEnumerable<string> Files => _files;

        public IEnumerable<int> Indexis => _indexis;

        public MainVM()
        {
            _files = new ObservableCollection<string>();
            _indexis = new ObservableCollection<int>();

            StartCommand = new DelegateCommand(Start);
        }


        public DelegateCommand StartCommand{
            get;
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(SearchStr))
            {
                MessageBox.Show(@"Pattern string is empty");
                return;
            }

            if (_files.Any()) 
                _files.Clear();

            if (_indexis.Any())
                _indexis.Clear();

            var folderBrowser = new FolderBrowserDialog();

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
