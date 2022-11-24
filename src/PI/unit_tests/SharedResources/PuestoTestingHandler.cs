using PI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_tests.SharedResources
{
    public class PuestoTestingHandler : HandlerGenerico
    {
        // brief: metodo que inserta en la base de datos una lista semilla de puestos
        public List<PuestoModel> InsertarPuestosSemillaEnBase(DateTime fechaCreacion)
        {
            List<PuestoModel> puestosSemilla = new List<PuestoModel>();
            puestosSemilla.Add(new PuestoModel
            {
                Nombre = "Jefe",
                Plazas = 1,
                SalarioBruto = 4400m,
                Beneficios = 1500m,
                FechaAnalisis = fechaCreacion
            });
            puestosSemilla.Add(new PuestoModel
            {
                Nombre = "Empacador",
                Plazas = 25,
                SalarioBruto = 3698m,
                Beneficios = 45687m,
                FechaAnalisis = fechaCreacion
            });
            puestosSemilla.Add(new PuestoModel
            {
                Nombre = "Cajero",
                Plazas = 8,
                SalarioBruto = 4567m,
                Beneficios = 4578m,
                FechaAnalisis = fechaCreacion
            });

            return InsertarPuestosSemillaEnBase(puestosSemilla);
        }

        // brief: metodo que inserta en la base de datos una lista semilla de puestos que recibe por paramero
        // details: este metodo se usa si en el test es necesario meter puesto con condiciones especiales
        public List<PuestoModel> InsertarPuestosSemillaEnBase(List<PuestoModel> puestosSemilla)
        {
            foreach (var puesto in puestosSemilla)
            {
                InsertarPuestosSemillaEnBase(puesto);
            }

            return puestosSemilla;
        }

        // brief: metodo que inserta en la base de datos un solo puesto
        // details: el puesto debe de tener la fecha del analisis al que corresponde
        public PuestoModel InsertarPuestosSemillaEnBase(PuestoModel puesto)
        {
            string insert = " INSERT INTO PUESTO values ("
                + "'" + puesto.Nombre + "', "
                + "'" + puesto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', "
                + puesto.Plazas.ToString() + ", "
                + puesto.SalarioBruto.ToString().Replace(",", ".") + ", "
                + puesto.Beneficios.ToString().Replace(",", ".") + ")";

            base.EnviarConsultaGenerica(insert);

            return puesto;
        }

        // brief: metodo que retorna todos los puestos que hay en la base de datos de un analisis
        public List<PuestoModel> LeerPuestosDeBase(DateTime FechaCreacion)
        {
            List<PuestoModel> puestoEnBase = new List<PuestoModel>();

            DataTable resultadoConsulta = base.CrearTablaConsultaGenerico("SELECT * FROM PUESTO WHERE fechaAnalisis='" +
                FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by orden ASC");
            // iteramos por las filas de la consulta para crear los puestos
            foreach (DataRow fila in resultadoConsulta.Rows)
            {
                // ingresamos todos los datos de cada puesto
                PuestoModel puesto = new PuestoModel();
                Console.WriteLine(puesto.Nombre);
                puesto.Nombre = Convert.ToString(fila["nombre"]);
                puesto.Plazas = Convert.ToInt32(fila["cantidadPlazas"]);
                puesto.SalarioBruto = Convert.ToDecimal(fila["salarioBruto"]);
                puesto.FechaAnalisis = (DateTime)fila["fechaAnalisis"];
                puesto.Beneficios = Convert.ToDecimal(fila["beneficios"]);

                puestoEnBase.Add(puesto);
            }
            return puestoEnBase;
        }
    }
}
