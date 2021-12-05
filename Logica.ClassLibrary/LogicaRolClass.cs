using AccesoDatos_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.ClassLibrary
{
   public class LogicaRolClass
    {
        private static DcmantenimientoDataContext dc = new DcmantenimientoDataContext();

        public static List<Rol> getAllaRol()
        {
            try { 
                // cargando datos al data grill viw
                var lista = dc.Rol.Where(data => data.rol_status == 'A')
                                                         .OrderBy(ord => ord.rol_descripcion);
            return lista.ToList();
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al obtener el usuario" + ex.Message);
            }
        }
    }
}
