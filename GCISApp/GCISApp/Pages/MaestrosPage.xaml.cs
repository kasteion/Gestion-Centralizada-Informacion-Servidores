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
    /// Lógica de interacción para MaestrosPage.xaml
    /// </summary>
    public partial class MaestrosPage : Page
    {
        public MaestrosPage()
        {
            InitializeComponent();
            //Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 1));
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

        private void BtnSAN_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SANPage());
        }

        private void BtnSoluciones_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SolucionesPage());
        }

        private void BtnStatus_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 1));
        }

        private void BtnTipo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 2));
        }

        private void BtnMarca_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 3));
        }

        private void BtnModelo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 4));
        }

        private void BtnProcesador_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 5));
        }

        private void BtnProveedor_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 6));
        }

        private void BtnContacto_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 7));
        }

        private void BtnAplicacion_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 8));
        }

        private void BtnOperativo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 9));
        }

        private void BtnEdificio_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 10));
        }

        private void BtnRack_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => TabMaestros.SelectedIndex = 11));
        }
    }
}
