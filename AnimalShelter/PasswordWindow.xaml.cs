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

namespace AnimalShelter
{
    /// <summary>
    /// Логика взаимодействия для PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string UserEmail
        {
            get { return email.Text; }
        }

        public string UserPassword
        {
            get { return password.Password;  }
        }
    }
}
