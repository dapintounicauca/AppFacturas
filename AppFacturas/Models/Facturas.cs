using AppFacturas.DataBase;
using AppFacturas.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFacturas.Models
{
    public class Facturas
    {
        private ObservableCollection<Factura> data;

        public ObservableCollection<Factura> Data
        {
            get
            {
                if (data == null)
                {
                    data = new ObservableCollection<Factura>();
                    var rutaBD = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "FacturasBD.sqlite");
                    SQLiteConnection con = new SQLiteConnection(rutaBD);
                    FacturaDao factDao = new FacturaDao(con);
                    List<Factura> listaFacturas = new List<Factura>();

                    listaFacturas = factDao.getAll();

                    for (int i = 0; i < listaFacturas.Count; i++)
                    {
                        data.Add(listaFacturas.ElementAt(i));
                    }
                }
                return data;
            }
            set { data = value; }
        }
    }
}
