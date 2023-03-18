using Messager.Helpers;
using Messager.Models.Entitys;
using Messager.Models.Requests;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _currentUser;
        public MainWindow(User user)
        {
            _currentUser = user;
            InitializeComponent();
            DataContext = new MainWindowViewModel(user);
        }

        protected override async void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            using (var request = await RequestsFactory.CreateRequestAsync<CloseRequest, User>(_currentUser))
            {
                await request.SendRequestAsync();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ((MainWindowViewModel)DataContext).UpdateChatsAsync();
            var temp = await BitmapHelper.GetUserImageAsync(_currentUser);
            ((MainWindowViewModel)DataContext).Image = temp;
        }
    }
}
