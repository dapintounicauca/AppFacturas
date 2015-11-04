using AppFacturas.DataBase;
using AppFacturas.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppFacturas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFacturaPage : Page
    {
        Frame rootFrame;
        SQLiteConnection con;
        FacturaDao factDao;

        public AddFacturaPage()
        {
            this.InitializeComponent();
            con = new SQLiteConnection(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "FacturasBD.sqlite"));
            factDao = new FacturaDao(con);
            rootFrame = Window.Current.Content as Frame;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += AddFacturaPage_BackRequested;
        }

        private void AddFacturaPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void saveRecordatorio(object sender, RoutedEventArgs e)
        {
            if (validarDatos() == 1) //si los datos son validados
            {
                Factura factura = new Factura();

                factura = getInfoFactura(); //obtengo la infomacion de los controles y retonrno el objeto factura

                factDao.insertFactura(factura); //inserto en la base de datos el nuevo objeto

                var facturas = App.Current.Resources["facturas"] as Facturas; //obtengo la referencia de la coleccion de datos
                facturas.Data.Add(factura);  //actualizo la coleccion con el objeto que fue insertado en la base de datos

                //redirigo a MainPage con bandera 0 para mensaje de insertado con exito
                rootFrame.Navigate(typeof(MainPage), "0");
            }
            if (validarDatos() == 2) //problemas con la fecha 
            {
                var dialog = new Windows.UI.Popups.MessageDialog("No te podemos notificar " + (txtDia.SelectedIndex + 1) + " días antes, el día límite es el " + txtVence.Text, "Algo sucede!");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Entendido") { Id = 1 });
                var result = await dialog.ShowAsync();
            }
            if (validarDatos() == 0)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Falta información para crear el recordatorio", "Falta Información!");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Entendido") { Id = 1 });
                var result = await dialog.ShowAsync();
            }
        }

        public Factura getInfoFactura()
        {
            Factura fact = new Factura();

            fact.Nombre = getNombreEmpresa();
            fact.Vence = getFechaVence();
            fact.Alarma = getFechaAlarma();
            fact.Estado = "Pendiente";
            fact.Valor = Convert.ToInt32(txtValor.Text);

            return fact;
        }
        public int validarDatos() //si todos los datos ingresados son correctos 1 para continuar, 0 notificar que faltan datos, o 2 para problema con la fecha
        {
            if (txtNombre.SelectedIndex != -1 && txtDia.SelectedIndex != -1 && txtValor.Text != "")
            {
                if ((Convert.ToInt32(txtVence.Text)) < (DateTime.Now.Day + (txtDia.SelectedIndex + 1))) //si la fecha de vencimiento es mayor al dia actual mas los dias de recordatorio
                    return 2; //notificar problema con la fecha
                else
                    return 1;//campos validados
            }
            return 0;
        }
        public DateTime getFechaAlarma()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int alarmDay = Convert.ToInt32(txtDia.SelectedItem.ToString());
            int day = Convert.ToInt32(txtVence.Text) - alarmDay;
            int hour = txtHora.Time.Hours;
            int minute = txtHora.Time.Minutes;

            return new DateTime(year, month, day, hour, minute, 00);
        }
        public DateTime getFechaVence()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, (Convert.ToInt32(txtVence.Text)), 23, 59, 59);
        }
        public string getNombreEmpresa()
        {
            Empresa emp = txtNombre.SelectedValue as Empresa;
            return emp.Nombre;
        }
    }
}
