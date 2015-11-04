using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AppFacturas.Models;
using Windows.UI.Core;
using SQLitePCL;
using AppFacturas.DataBase;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppFacturas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Facturas facturas;
        Frame rootFrame;
        SQLiteConnection con;
        FacturaDao factDao;

        public MainPage()
        {
            this.InitializeComponent();
            con = new SQLiteConnection(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "FacturasBD.sqlite"));
            factDao = new FacturaDao(con);
            facturas = App.Current.Resources["facturas"] as Facturas;
            rootFrame = Window.Current.Content as Frame;

        }

        private void addFactura(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(AddFacturaPage), "");
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string msg = e.Parameter as string;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            if (msg.Equals("0")) //Redireccion desde addFacturaPage
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Has creado un nuevo recordatorio para una Factura.", "Recordatorio Nuevo");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Entendido") { Id = 1 });
                var result = await dialog.ShowAsync();
            }
            if (msg.Equals("1")) //Redireccion desde editFacturaPage
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Has modificado un recordatorio de una Factura", "Recordatorio Editado");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Entendido") { Id = 1 });
                var result = await dialog.ShowAsync();
            }

        }

        private async void deleteRecordatorio(object sender, RoutedEventArgs e)
        {
            int index = lista.SelectedIndex;
            if (index != -1)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("¿Está seguro que desea borrar este recordatorio?", "Borrar Recordatorio");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Si") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });
                var result = await dialog.ShowAsync();

                if (result != null && result.Label == "Si")
                {
                    factDao.deleteFactura(facturas.Data.ElementAt(index).Id);
                    facturas.Data.Remove(facturas.Data.ElementAt(index));
                }
            }
        }

        private void editRecordatorio(object sender, RoutedEventArgs e)
        {
            if (lista.SelectedIndex != -1) //me aseguro de que un elemento este seleccionado para continuar, sino no hace ni mierda
            {
                rootFrame.Navigate(typeof(editFacturaPage), facturas.Data.ElementAt(lista.SelectedIndex)); //se envia el objeto seleccionado a editFacturaPage
            }
        }
    }
}
