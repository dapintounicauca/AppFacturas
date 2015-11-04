using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFacturas.Models
{
    public class Empresas
    {
        private ObservableCollection<Empresa> empresaList;

        public ObservableCollection<Empresa> EmpresasList
        {
            get
            {
                if (empresaList == null)
                {
                    empresaList = new ObservableCollection<Empresa>();

                    Empresa emp1 = new Empresa() { Nombre = "Acueducto Y Alcantarillado", Vence = 7 };
                    Empresa emp2 = new Empresa() { Nombre = "Claro", Vence = 19 };
                    Empresa emp3 = new Empresa() { Nombre = "Compañía Energética de Occidente", Vence = 15 };
                    Empresa emp4 = new Empresa() { Nombre = "Emtel SAESP", Vence = 10 };
                    Empresa emp5 = new Empresa() { Nombre = "Movistar", Vence = 9 };

                    empresaList.Add(emp1);
                    empresaList.Add(emp2);
                    empresaList.Add(emp3);
                    empresaList.Add(emp4);
                    empresaList.Add(emp5);
                }
                return empresaList;
            }
            set { empresaList = value; }
        }
    }
}
