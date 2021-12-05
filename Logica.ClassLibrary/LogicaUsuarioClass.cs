using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos_ClassLibrary;
namespace Logica.ClassLibrary
{
    public class LogicaUsuarioClass
    {
        private static DcmantenimientoDataContext dc = new DcmantenimientoDataContext();

        public static List<Usuario> getAllaUser()
        {
            try
            {
                // cargando datos al data grill viw
                var lista = dc.Usuario.Where(data => data.usu_status == 'A')
                                          .OrderBy(ord => ord.usu_apellidos);
                return lista.ToList();
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al obtener el usuario" + ex.Message);
            }
        }
        public static Usuario getUsersXId(int idUsiario)
        {
            try
            {
                var usuario = dc.Usuario.Where(data => data.usu_status == 'A'
                                                          && data.usu_id.Equals(idUsiario)).FirstOrDefault();
                return usuario;
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al obtener el usuario " + ex.Message);
            }
        }
        public static Usuario getUserXLogin(string email, string password)
        {
            try
            {
                var usuario = dc.Usuario.Where(data => data.usu_status == 'A'
                                                       && data.usu_correo.Equals(email)
                                                       && data.usu_password.Equals(password)
                                                       ).FirstOrDefault();
                return usuario;
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al obtener el usuario " + ex.Message);
            }
        }
        public static bool saveUser(Usuario dataUsiario)
        {
            try
            {
                bool result = false;
                dataUsiario.usu_add = DateTime.Now;
                dataUsiario.usu_status = 'A';
                dc.Usuario.InsertOnSubmit(dataUsiario);
                dc.SubmitChanges();

                result = true;
                return result;
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al guardar el usuario " + ex.Message);
            }
        }

        public static bool updateUser(Usuario dataUsiario)
        {
            try
            {
                bool result = false;
                dc.SubmitChanges();

                result = true;
                return result;
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al instertar el usuario " + ex.Message);
            }
        }
        public static bool updateUser2(Usuario dataUsiario)
        {
            try
            {
                bool result = false;
                dataUsiario.usu_update = DateTime.Now;

                dc.ExecuteCommand("UPDATE [dbo].[Usuario]" +
                                   "SET[usu_correo] ={ 0} " +
                                     // ",[usu_password] = { 1}" +
                                      ",[usu_apellidos] ={ 1} " +
                                      ",[usu_nombres] = { 2}" +
                                      ",[usu_update] = { 3}" +
                                      ",[rol_id] = { 4}" +
                                 "WHERE[usu_id] { 5}", new object[]
                                 {
                                     dataUsiario.usu_correo,
                                    // dataUsiario.usu_password,
                                     dataUsiario.usu_apellidos,
                                     dataUsiario.usu_nombres,
                                     dataUsiario.usu_update,
                                     dataUsiario.rol_id,
                                     dataUsiario.usu_id

                                 });
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dc.Usuario);

                dc.SubmitChanges();
                result = true;
                return result;
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al instertar el usuario " + ex.Message);
            }
        }

        public static bool deleteUser(Usuario dataUsiario)
        {
            try
            {
                bool result = false;

                dataUsiario.usu_status = 'I';
                dc.SubmitChanges();

                result = true;
                return result;
            }


            catch (global::System.Exception ex)
            {

                throw new ArgumentException("error al eliminar el usuario " + ex.Message);
            }
        }
    }
}
