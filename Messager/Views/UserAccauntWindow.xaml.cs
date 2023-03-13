using Messager.Models.Entitys;
using Messager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messager.Views
{
    /// <summary>
    /// Логика взаимодействия для UserAccauntWindow.xaml
    /// </summary>
    public partial class UserAccauntWindow : Window
    {
        public UserAccauntWindow(params User[] users)
        {
            InitializeComponent();
            DataContext = new UserAccauntWindowViewModel(users);
        }
    }
}
