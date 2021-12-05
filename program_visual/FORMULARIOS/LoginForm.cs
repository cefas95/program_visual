using AccesoDatos_ClassLibrary;
using program_visual.FORMULARIOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace program_visual
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
           
        }

      
        private void btningresar_Click(object sender, EventArgs e)
        {
            login();
        }

        private void login()
        {
            try
            {
                string correo = textcorreo.Text.TrimStart().TrimEnd();
                string clave =textclave.Text;
               // string encriptar = Logica.ClassLibrary(clave);

                if (!string.IsNullOrEmpty(correo) && !string.IsNullOrEmpty(clave))
                {
                    Usuario usuario = new Usuario();
                    usuario = Logica.ClassLibrary.LogicaUsuarioClass.getUserXLogin(correo, Logica.ClassLibrary.Utilidades.EncriptarClass.GetMD5(clave));
                    if (usuario!= null)
                    {
                        var datauser = usuario.usu_nombres + " " + usuario.usu_apellidos;
                        //interpolacion
                        var datauser2 = $"{usuario.usu_nombres}  {usuario.usu_apellidos}";

                        MessageBox.Show("Bienvenido al sistema \n Rol: " + usuario.Rol.rol_descripcion 
                           + " \n Usiario:" + datauser2, "Sistema de matriculacion vehicular", MessageBoxButtons.OK);

                        UsuarioForm Usufr =new UsuarioForm();
                        Usufr.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Error en Correo o Clave", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
               
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
