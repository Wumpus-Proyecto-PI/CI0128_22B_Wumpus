﻿using Microsoft.EntityFrameworkCore;
using PI.EntityModels;
using PI.Services;

namespace PI.EntityHandlers
{
    public class EstructuraOrgHandler : EntityHandler
    {
        public EstructuraOrgHandler(DataBaseContext context) : base(context) { }

        public async Task EliminarPuesto(Puesto puestoAEliminar)
        {
            Contexto.Puestos.Remove(puestoAEliminar);
            await Contexto.SaveChangesAsync();
        }

        public async Task<List<Puesto>> ObtenerListaDePuestos(DateTime fechaAnalisis)
        {
            return await Contexto.Puestos.Where(x => x.FechaAnalisis == fechaAnalisis).OrderByDescending(x => x.SalarioBruto * x.CantidadPlazas).ToListAsync();
        }

        public async Task ActualizarPuesto(string nombrePuesto, Puesto puestoAInsertar)
        {
            Puesto PuestoAActualizar = await Contexto.Puestos.Where(x => x.Nombre == nombrePuesto && x.FechaAnalisis == puestoAInsertar.FechaAnalisis).FirstOrDefaultAsync();
            if (PuestoAActualizar != null)
            {
                Contexto.Puestos.Remove(PuestoAActualizar);
                await Contexto.Puestos.AddAsync(new Puesto
                {
                    Nombre = puestoAInsertar.Nombre,
                    CantidadPlazas = puestoAInsertar.CantidadPlazas,
                    SalarioBruto = puestoAInsertar.SalarioBruto,
                    Beneficios = puestoAInsertar.Beneficios,
                    FechaAnalisis = puestoAInsertar.FechaAnalisis
                });
            } else
            {
                PuestoAActualizar.CantidadPlazas = puestoAInsertar.CantidadPlazas;
                PuestoAActualizar.SalarioBruto = puestoAInsertar.SalarioBruto;
                PuestoAActualizar.Beneficios = puestoAInsertar.Beneficios;
            }

            await Contexto.SaveChangesAsync();
        }

        public async Task<bool> ExistePuestoEnBase(string nombrePuesto, DateTime fechaAnalisis)
        {
            bool encontrado = false;
            List<Puesto> puestos = await Contexto.Puestos.Where(x => x.Nombre == nombrePuesto && x.FechaAnalisis == fechaAnalisis).ToListAsync();
            if (puestos.Count > 0 && puestos[0].Nombre != null)
            {
                encontrado = true;
            }
            return encontrado;
        }

        public async Task InsertarPuesto(string nombrePuesto, Puesto puestoAInsertar)
        {
            if (FormatManager.EsAlfanumerico(nombrePuesto) && FormatManager.EsAlfanumerico(puestoAInsertar.Nombre))
            {
                // primero se revisa si ya existe el puesto en la base
                if (await ExistePuestoEnBase(nombrePuesto, puestoAInsertar.FechaAnalisis))
                {
                    // si existe el puesto lo actualizamos
                    await ActualizarPuesto(nombrePuesto, puestoAInsertar);
                }
                else
                {
                    await Contexto.Puestos.AddAsync(puestoAInsertar);

                    // realizamos la consulta
                    // este método es heredado del padre y permite enviar consultas de actualización, borrado e inserción   
                    await Contexto.SaveChangesAsync();
                }
            }
        }

    }
}