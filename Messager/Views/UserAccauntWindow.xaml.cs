using Messager.Helpers;
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
        private User _addingUser;
        public UserAccauntWindow(params User[] users)
        {
            InitializeComponent();
            _addingUser = users[1];
            DataContext = new UserAccauntWindowViewModel(users);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((UserAccauntWindowViewModel)DataContext).Image = await BitmapHelper.GetUserImageAsync(_addingUser);
        }
    }
}
