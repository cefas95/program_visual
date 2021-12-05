using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica.ClassLibrary; //reglas del negocio
using AccesoDatos_ClassLibrary; // ORM objetos 

namespace program_visual.FORMULARIOS
{
    public partial class UsuarioForm : Form
    {
        public UsuarioForm()
        {
            InitializeComponent();
        }

        private void UsuarioForm_Load(object sender, EventArgs e)
        {
            loadUser();
            loadRol();
        }

        private void loadUser()
        {
            try
            {
                List<Usuario> listaUsuarios = new List<Usuario>();
                listaUsuarios = LogicaUsuarioClass.getAllaUser();
                if (listaUsuarios != null && listaUsuarios.Count > 0)
                {
                    dgvUsuario.DataSource = listaUsuarios.Select(data => new
                    {

                        CODIGO = data.usu_id,
                        APELLIDOS = data.usu_apellidos,
                        NOMBRES = data.usu_nombres,
                        CORREO = data.usu_correo,
                        ROL = data.Rol.rol_descripcion
                    }).ToList();

                }
            }
            catch 
            {

                MessageBox.Show("Error al cargar los usuarios", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadRol()
        {
            try
            {
                List<Rol> listaRoles = new List<Rol>();
                listaRoles = LogicaRolClass.getAllaRol();
                if (listaRoles != null && listaRoles.Count > 0)
                {
                    listaRoles.Insert(0, new Rol
                    {
                        rol_id = 0,
                        rol_descripcion = "Seleccione Rol"

                    });
                    cmbRol.DataSource = listaRoles;
                    cmbRol.DisplayMember = "rol_descripcion";
                    cmbRol.ValueMember = "rol_id";
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Error al cargar rol", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpair();
        }
        private void limpair()
        {
            lblCodigo.Text = " ";
            txtNombres.Clear();
            txtApellidos.Clear();
            txtCorreo.Clear();
            txtContraseña.Clear();
            txtConfContraseña.Clear();
            cmbRol.SelectedIndex = 0;
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblCodigo.Text))
            {
                modificarUsuario();
            }
            else
            {

                GuardarUsuario(); 
            }
        }

        private string validations(Usuario usuario)
        {
            string mensaje = string.Empty;
            if (string.IsNullOrEmpty(usuario.usu_apellidos))
            {
                mensaje += "* Aplellidos Obligatorios \n";
            }
            if (string.IsNullOrEmpty(usuario.usu_nombres))
            {
                mensaje += "* Nombres Obligatorios \n";
            }
            if (string.IsNullOrEmpty(lblCodigo.Text))
            {
                if (string.IsNullOrEmpty(usuario.usu_password) || string.IsNullOrEmpty(txtConfContraseña.Text))
                {
                    mensaje += "* Contraseña Obligatoria \n";
                } 
            }
            if (string.IsNullOrEmpty(usuario.usu_correo))
            {
                mensaje += "* Correo Obligatorio \n";
            }
            if (usuario.rol_id ==0)
            {
                mensaje += "* Rol Obligatorio \n";
            }
            if (!string.IsNullOrEmpty(txtContraseña.Text) && !string.IsNullOrEmpty(txtConfContraseña.Text))
            {
                if (!txtContraseña.Text.Equals(txtConfContraseña.Text))

                {
                    mensaje += "Contraseñas no son iguales \n";
                }
            }
            if (!string.IsNullOrEmpty(usuario.usu_correo)) 
            {
                if (!Logica.ClassLibrary.Utilidades.ValidacionClass.email_bien_escrito(usuario.usu_correo))
                {
                    mensaje += "* Correo invalido \n";
                }
            }
            return mensaje;
        }
        private void GuardarUsuario()
        {

            try
            {
                Usuario usuario = new Usuario();
                usuario.usu_apellidos = txtApellidos.Text.ToUpper();
                usuario.usu_nombres = txtNombres.Text;
                usuario.usu_password = txtContraseña.Text.ToUpper();
                usuario.usu_correo = txtCorreo.Text;
                usuario.rol_id = int.Parse(cmbRol.SelectedValue.ToString());

                //validaciones
                string validationmessage = validations(usuario);

                if (!string.IsNullOrEmpty(validationmessage))
                {
                    MessageBox.Show(validationmessage, "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //encripado
                usuario.usu_password = Logica.ClassLibrary.Utilidades.EncriptarClass.GetMD5(usuario.usu_password);

                bool resultSave = LogicaUsuarioClass.saveUser(usuario);
                if (resultSave)
                {
                    MessageBox.Show("Usuario guradado correctamente", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadUser();
                    limpair();
                }
                else
                {
                    MessageBox.Show("Error al guardar Usuario", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al guardar Usuario", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void modificarUsuario()
        {

            try
            {
                if (!string.IsNullOrEmpty(lblCodigo.Text))
                {
                    var resMessage = MessageBox.Show("Desea modificar el usuario", "sistema de matriculacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resMessage.ToString() == "Yes")
                    {
                        Usuario usuario = new Usuario();
                        // usuario = LogicaUsuarioClass.getUsersXId(Convert.ToInt32(lblCodigo.Text));
                        usuario.usu_id = Convert.ToInt32(lblCodigo.Text);
                        usuario.usu_apellidos = txtApellidos.Text.ToUpper();
                        usuario.usu_nombres = txtNombres.Text;                       
                        usuario.usu_correo = txtCorreo.Text;
                        usuario.rol_id = int.Parse(cmbRol.SelectedValue.ToString());

                        //validaciones
                        string validationmessage = validations(usuario);

                        if (!string.IsNullOrEmpty(validationmessage))
                        {
                            MessageBox.Show(validationmessage, "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        //encripado

                        if (!string.IsNullOrEmpty(txtContraseña.Text))
                        {
                            usuario.usu_password = txtContraseña.Text.ToUpper();
                            usuario.usu_password = Logica.ClassLibrary.Utilidades.EncriptarClass.GetMD5(usuario.usu_password); 
                        }

                        bool resultSave = LogicaUsuarioClass.saveUser(usuario);
                        if (resultSave)
                        {
                            MessageBox.Show("Usuario modificado correctamente", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadUser();
                            limpair();
                        }
                        else
                        {
                            MessageBox.Show("Error al modificar Usuario", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }  
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al modificar Usuario", "Sistema de matriculacion vehicular", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void eliminarUsuario()
        {
            try
            {
                if (!string.IsNullOrEmpty(lblCodigo.Text))
                {
                    var resMessage = MessageBox.Show("Desea eliminar el usuario", "sistema de matriculacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resMessage.ToString() == "Yes")
                    {
                        Usuario usuario = new Usuario();
                        usuario = LogicaUsuarioClass.getUsersXId(Convert.ToInt32(lblCodigo.Text));
                        if (usuario != null)
                        {
                            if (LogicaUsuarioClass.deleteUser(usuario))
                            {
                                MessageBox.Show("Usuario Eliminado correctamente");
                                loadUser();
                                limpair();
                            }
                        }
                    }
                }
            }
            catch (Exception )
            {

                MessageBox.Show("no se puede eliminar el regristro");
            }
        }

        private void dgvUsuario_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var codigoUsu = dgvUsuario.Rows[e.RowIndex].Cells["CODIGO"].Value;
            var correoUsu = dgvUsuario.Rows[e.RowIndex].Cells["CORREO"].Value;
            var apellidoUsu = dgvUsuario.Rows[e.RowIndex].Cells["APELLIDOS"].Value;
            var nombreoUsu = dgvUsuario.Rows[e.RowIndex].Cells["NOMBRES"].Value;
            var rolUsu = dgvUsuario.Rows[e.RowIndex].Cells["ROL"].Value;

            if (!string.IsNullOrEmpty(codigoUsu.ToString()))

            {
                lblCodigo.Text = codigoUsu.ToString();
                txtCorreo.Text = correoUsu.ToString();
                txtNombres.Text = nombreoUsu.ToString();
                txtApellidos.Text = apellidoUsu.ToString();
                cmbRol.SelectedIndex = cmbRol.FindString(rolUsu.ToString());
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminarUsuario();
        }
    }
}
