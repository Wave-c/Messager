﻿using Messager.ViewModels;
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
    /// Логика взаимодействия для ChatUC.xaml
    /// </summary>
    public partial class ChatUC : UserControl
    {
        public ChatUCViewModel ChatUCViewModel { get; set; }
        public ChatUC()
        {
            InitializeComponent();
            DataContext = new ChatUCViewModel();
            ChatUCViewModel = ((ChatUCViewModel)DataContext);
        }
    }
}
