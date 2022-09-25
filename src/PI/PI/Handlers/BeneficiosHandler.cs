using PI.Handler

class BeneficiosHandler : Handler{
        public List<BeneficioModel> ObtenerBeneficios(string nombrePuesto)
        {
            List < BeneficioModel > resultadoBeneficios = new List<BeneficioModel>();
            // extraer los beneficios
            string consulta = "SELECT * FROM BENEFICIO_PUESTO WHERE nombrePuesto='" + nombrePuesto + "'";
            DataTable tablaResultadoBeneficios = CrearTablaConsulta(consulta);
            foreach (DataRow beneficio in tablaResultadoBeneficios.Rows)
            {
                resultadoBeneficios.Add(new BeneficioModel
                {
                    nombreBeneficio = Convert.ToString(beneficio["nombreBeneficio"]),
                    monto = Convert.ToDecimal(beneficio["monto"]),
                    plazasPorBeneficio = Convert.ToInt16(beneficio["PlazaPorBeneficio"])
                });
            }

            return resultadoBeneficios;
        }

        public List<BeneficioModel> crearBeneficio (string nombreBeneficio, string fechaAnalisis, string nombre
            , decimal monto, int cantidadDePlazas) 
        {
            string consulta = "INSERT INTO BENEFICIO VALUES("
                +nombreBeneficio+","+fechaAnalisis+","+nombre+","+monto+","+cantidadDePlazas+")" + " SELECT * FROM BENEFICIO";
            DataTable tablaResultante = CrearTablaConsulta(consulta);
            return tablaResultante;
        }
}