using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFacturas.Models
{
    public class Factura : INotifyPropertyChanged
    {
        private long id;

        public long Id
        {
            get { return id; }
            set
            {
                id = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }

        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Nombre"));

            }
        }

        private DateTime vence;

        public DateTime Vence
        {
            get { return vence; }
            set
            {
                vence = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Vence"));
            }
        }

        private DateTime alarma;

        public DateTime Alarma
        {
            get { return alarma; }
            set
            {
                alarma = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Alarma"));
            }
        }

        private long valor;

        public long Valor
        {
            get { return valor; }
            set
            {
                valor = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Valor"));
            }
        }

        private string estado;

        public string Estado
        {
            get { return estado; }
            set
            {
                estado = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Estado"));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
