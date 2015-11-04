using AppFacturas.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFacturas.DataBase
{
    public class FacturaDao
    {
        SQLiteConnection con;

        public FacturaDao(SQLiteConnection con)
        {
            this.con = con;
            string sql = "CREATE TABLE IF NOT EXISTS factura (id INTEGER PRIMARY KEY AUTOINCREMENT, nombre TEXT, vence DATETIME, alarma DATETIME, valor INTEGER, estado TEXT)";
            using (var statement = con.Prepare(sql))
            {
                statement.Step();
            }
        }
        public void insertFactura(Factura factura)
        {
            string sql = "INSERT INTO factura (nombre, vence, alarma, valor, estado) VALUES(?,?,?,?,?)";
            using (var statement = con.Prepare(sql))
            {
                statement.Bind(1, factura.Nombre);
                statement.Bind(2, string.Format("{0:yyyy-MM-dd hh:mm:ss}", Convert.ToDateTime(factura.Vence)));
                statement.Bind(3, string.Format("{0:yyyy-MM-dd hh:mm:ss}", Convert.ToDateTime(factura.Alarma)));
                statement.Bind(4, factura.Valor);
                statement.Bind(5, factura.Estado);
                statement.Step();
            }
        }
        public void updateFactura(Factura factura)
        {
            string sql = "UPDATE factura SET nombre=?, vence=?, alarma=?, valor=? ,estado=? WHERE id=?";
            using (var statement = con.Prepare(sql))
            {
                statement.Bind(1, factura.Nombre);
                statement.Bind(2, string.Format("{0:yyyy-MM-dd hh:mm:ss}", Convert.ToDateTime(factura.Vence)));
                statement.Bind(3, string.Format("{0:yyyy-MM-dd hh:mm:ss}", Convert.ToDateTime(factura.Alarma)));
                statement.Bind(4, factura.Valor);
                statement.Bind(5, factura.Estado);
                statement.Bind(6, factura.Id);
                statement.Step();
            }
        }
        public void deleteFactura(long id)
        {
            string sql = "DELETE FROM factura WHERE id=?";
            using (var statement = con.Prepare(sql))
            {
                statement.Bind(1, id);
                statement.Step();
            }
        }
        public Factura getById(long id)
        {
            Factura factura = null;
            string sql = "SELECT * FROM factura WHERE id=?";
            using (var statement = con.Prepare(sql))
            {
                statement.Bind(1, id);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    factura = getFacturaWithStatement(statement);
                }
            }
            return factura;
        }
        public List<Factura> getAll()
        {
            string sql = "SELECT * FROM factura";
            return getListWithSql(sql);
        }
        public List<Factura> getAllByNombre(string nombre)
        {
            string sql = "SELECT * FROM factura WHERE nombre LIKE '%" + nombre + "%'";
            return getListWithSql(sql);
        }
        private Factura getFacturaWithStatement(ISQLiteStatement statement)
        {
            Factura factura = new Factura();
            factura.Id = (long)statement[0];
            factura.Nombre = (string)statement[1];

            factura.Vence = FormatDateTime(statement[2]);

            factura.Alarma = FormatDateTime(statement[3]);

            factura.Valor = (long)statement[4];
            factura.Estado = (string)statement[5];
            return factura;
        }
        private List<Factura> getListWithSql(string sql)
        {
            List<Factura> facturas = new List<Factura>();
            using (var statement = con.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    facturas.Add(getFacturaWithStatement(statement));
                }
            }
            return facturas;
        }
        private DateTime FormatDateTime(Object statement)
        {
            DateTime fecha;

            string fechaprueba = statement.ToString();
            string[] cadena = (fechaprueba.Split(' '));
            string[] date = (cadena[0].Split('-'));
            string[] time = (cadena[1].Split(':'));

            int year = Convert.ToInt32(date[0]);
            int month = Convert.ToInt32(date[1]);
            int day = Convert.ToInt32(date[2]);

            int hour = Convert.ToInt32(time[0]);
            int minute = Convert.ToInt32(time[1]);
            int second = Convert.ToInt32(time[2]);

            fecha = new DateTime(year, month, day, hour, minute, second);

            return fecha;
        }
    }
}

