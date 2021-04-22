using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ObjectViewer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region BindableBase
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }
        #endregion

        private List<ObjectNode> _nodes;

        public List<ObjectNode> Nodes
        {
            get => _nodes;
            set => SetProperty(ref _nodes, value);
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Person p1 = new Person()
            {
                Name = "Scott",
                Age = 18,
                Sex = Sex.Female
            };

            Person p2 = new Person()
            {
                Name = "Alice",
                Age = 50,
                Sex = Sex.Male
            };

            //Person p3 = new Person()
            //{
            //    Name = "Cloud",
            //    Age = 70,
            //    Sex = Sex.Male
            //};

            p1.Father = p2;
            p2.Father = p1;

            Nodes = ObjectNode.DecodeObject(p1);
        }
    }
}
