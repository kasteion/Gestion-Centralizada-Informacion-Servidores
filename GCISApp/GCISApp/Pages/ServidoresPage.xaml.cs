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

namespace GCISApp
{
    /// <summary>
    /// Lógica de interacción para ServidoresPage.xaml
    /// </summary>
    public partial class ServidoresPage : Page
    {
        public ServidoresPage()
        {
            InitializeComponent();
        }

        private void BtnMaestros_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MaestrosPage());
        }

        private void BtnServidores_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ServidoresPage());
        }

        private void BtnInstancias_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InstanciasPage());
        }

        private void BtnDatabases_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DatabasesPage());
        }

        private void BtnSoluciones_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SolucionesPage());
        }

        private void BtnSAN_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SANPage());
        }

        private void BtnFisico_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabServidores.SelectedIndex = 1));
        }

        private void BtnVirtual_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabServidores.SelectedIndex = 2));
        }
    }
}
