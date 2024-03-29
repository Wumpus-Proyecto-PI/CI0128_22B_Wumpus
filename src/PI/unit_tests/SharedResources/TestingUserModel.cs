﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_tests.SharedResources
{
    // brief: clase que contiene el id y el email de nuestro usuario de testing
    // deatils: el usuariod e tesing es wumpustest@gmail.com
    public static class TestingUserModel
    {
        // hashcode que se asigan por identity al crear un usuario
        // este se usa para unir las tablas de negocio y de usuario de identity
        public static string UserId { get; } = "81727ffe-84fc-4263-b7e4-e763664968d9";

        // correo del usuario de etsting que se creo
        public static string UserCorreo { get; } = "wumpustest@gmail.com";

        // contraseña de este usuario de pruebas
        public static string UserContrasena { get; } = "wumpus";
    }
}
