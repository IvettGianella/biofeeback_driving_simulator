using BiofeebackDrivingSimulator.Datos;
using BiofeebackDrivingSimulator.Interface;
using BiofeebackDrivingSimulator.Models;
using BiofeebackDrivingSimulator.Services;
using BiofeebackDrivingSimulator.ViewModels.Base;
using BiofeebackDrivingSimulator.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class RegistroUsuarioViewModel : BaseViewModel
    {
        #region Variables Locales
        private string nombres = "";
        private string apellidos = "";
        private string edad = "";
        private string modelSexo;
        private string btnRegistrarEditar = "";
        private string visible;
        private string idView;
        private SesionListBox sesionLb;
        #endregion

        #region Propiedades
        public string IdView
        {
            get { return this.idView; }
            set { SetProperty(ref this.idView, value); } 
        }
        public string Nombres
        {
            get { return this.nombres; }
            set { SetProperty(ref this.nombres, value); }
        }
        public string Apellidos
        {
            get { return this.apellidos; }
            set { SetProperty(ref this.apellidos, value); }
        }
        public string Edad
        {
            get { return this.edad; }
            set { SetProperty(ref this.edad, value); }
        }

        public List<string> ListaSexo { get; set; }
        public string ModelSexo
        {
            get { return this.modelSexo; }
            set { SetProperty(ref this.modelSexo, value); }
        }
        public List<SesionListBox> Sesiones { get; set; }
        public SesionListBox SesionLb 
        {
            get { return this.sesionLb; }
            set { SetProperty(ref this.sesionLb, value); }
        }
        public string BtnRegistrarEditar
        {
            get { return this.btnRegistrarEditar; }
            set { SetProperty(ref this.btnRegistrarEditar, value); }
        }
        public int Id { get; set; }
        public string Visible
        {
            get { return this.visible; }
            set { SetProperty(ref this.visible, value); }
        }
        public Usuario Usuario { get; set; }
        public bool Muestra { get; set; }
        #endregion

        #region Constructor
        public RegistroUsuarioViewModel()
        {
            this.Init();
            this.SesionLb = new SesionListBox();
            this.BtnRegistrarEditar = "Registrar";
            this.Id = 0;
            this.Visible = "Hidden";
            this.Usuario = new Usuario();
            this.RegresarCommand = new RelayCommand<ICloseable>(this.Regresar);
            this.SesionCommand = new RelayCommand<ICloseable>(this.Session);
        }
        #endregion

        #region Commands
        //public RelayCommand<ICloseable> RegistroCommand { get; private set; }
        public RelayCommand<ICloseable> RegresarCommand { get; private set; }
        public RelayCommand<ICloseable> SesionCommand { get; private set; }
        public ICommand RegistroCommand => new DelegateCommand(Registro, null);
        public ICommand DoubleClickCommand => new DelegateCommand(Resultado, null);
        public ICommand ComparacionCommand => new DelegateCommand(Comparacion, null);
        #endregion

        #region Metodos
        /// <summary>
        /// Método de inicialización para registrar un nuevo usuario
        /// </summary>
        public void Init()
        {
            this.ListaSexo = new List<string>();
            ListaSexo.Add("Masculino");
            ListaSexo.Add("Femenino");
            this.ModelSexo = ListaSexo.FirstOrDefault();
            this.IdView = "0";
            this.Nombres = "";
            this.Apellidos = "";
            this.Edad = "";
            this.BtnRegistrarEditar = "Registrar";
            this.Visible = "Hidden";
            this.Id = 0;
            this.Muestra = true;
        }

        /// <summary>
        /// Método para editar un usuario y existente
        /// </summary>
        /// <param name="usuario">Objeto por el cual se obtienen
        /// los datos de usuario como Id, nombre, entre otros</param>
        public void Init(Usuario usuario) 
        {
            this.Usuario = usuario;
            this.Id = this.Usuario.Id;
            this.IdView = this.Usuario.Id.ToString();
            this.Nombres = this.Usuario.Nombres;
            this.Apellidos = this.Usuario.Apellidos;
            this.Edad = this.Usuario.Edad.ToString();
            this.Muestra = this.Usuario.EsMuestra;

            if (this.Usuario.Sexo)
            {
                this.ModelSexo = "Femenino";
            }
            else 
            {
                this.ModelSexo = "Masculino";
            }

            this.BtnRegistrarEditar = "Editar";
            this.Visible = "Visible";

            this.Sesiones = new List<SesionListBox>();
            using (var entidades = new Entidades())
            {
                //this.Usuario = entidades.Usuarios.Find(this.Id);
                this.Usuario = entidades.Usuarios
                                 .Where(u => u.Id == this.Id)
                                 .Include(u => u.Sesiones)
                                 .FirstOrDefault();
            }

            if (this.Usuario.Sesiones != null) 
            {
                foreach (var item in this.Usuario.Sesiones)
                {
                    var sesion = new SesionListBox
                    {
                        Id = item.Id,
                        Fecha = item.Fecha,
                        Cometario = item.Comentarios
                    };
                    this.Sesiones.Add(sesion);
                }
            }
            this.Sesiones.OrderBy(s => s.Fecha);
        }

        /// <summary>
        /// Método por el cual se guardan los datos nuevos o actualizados
        /// de un usuario
        /// </summary>
        /// <param name="obj"></param>
        public async void Registro(Object obj)
        {
            if (string.IsNullOrEmpty(this.Nombres) ||
                string.IsNullOrEmpty(this.Apellidos) ||
                string.IsNullOrEmpty(this.Edad))
            {
                Logger.Log.Info("Todos los campos son requeridos, RegistroUsuarioViewModel");
                MessageBox.Show(
                    "Todos los campos son requeridos",
                    "Alerta",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                try
                {
                    if (this.BtnRegistrarEditar == "Registrar")
                    {
                        var usuario = new Usuario
                        {
                            Apellidos = this.Apellidos,
                            Edad = Convert.ToInt32(this.Edad),
                            Nombres = this.Nombres,
                            EsMuestra = true
                        };

                        if (this.ModelSexo == "Masculino")
                        {
                            usuario.Sexo = false;
                        }
                        else
                        {
                            usuario.Sexo = true;
                        }

                        if (this.Id == 0)
                        {
                            using (var context = new Entidades())
                            {
                                UsuarioServices services = new UsuarioServices(context); 
                                var newUser = await services.AgregarUsuario(usuario);
                                this.Id = newUser.Id;
                                this.IdView = this.Id.ToString();
                            }

                            MessageBox.Show(
                            "El usuario se registro correctamente",
                            "Mensaje",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                            this.BtnRegistrarEditar = "Editar";
                            this.Visible = "Visible";
                        }
                    }
                    else 
                    {
                        using (var context = new Entidades())
                        {
                            UsuarioServices services = new UsuarioServices(context);
                            bool sexoEdit = false;
                            if (this.ModelSexo == "Masculino")
                            {
                                sexoEdit = false;
                            }
                            else
                            {
                                sexoEdit = true;
                            }
                            await services.EditarUsuario(
                                            this.Id, 
                                            this.Nombres, 
                                            this.Apellidos, 
                                            Convert.ToInt32(this.Edad),
                                            true,
                                            sexoEdit);
                        }

                        MessageBox.Show(
                        "Datos del usuario editados correctamente",
                        "Mensaje",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    MessageBox.Show(
                        "Ocurrió un problema al guardar los datos del usuario",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Logger.Log.Error("Mensaje: ", ex);
                }
            }
        }

        public void Regresar(ICloseable window)
        {
            MainViewModel.GetInstance().UsuariosVm.Init();
            if (!Application.Current.Windows.OfType<UsuariosView>().Any())
            {
                UsuariosView usuariosView = new UsuariosView();
                usuariosView.Show();
            }

            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Método por el cual se crea una nueva sesión y se
        /// envia a la vista de registro de simulación
        /// </summary>
        /// <param name="window"></param>
        public async void Session(ICloseable window)
        {
            try
            {
                var newSesion = new Sesion
                {
                    Fecha = System.DateTime.Now,
                    Comentarios = "",
                };

                using (var context = new Entidades())
                {
                    SesionServices sesionServices = new SesionServices(context);
                    var sesion = await sesionServices.AgregarSesion(this.Id, newSesion);
                    newSesion.Id = sesion.Id;
                }

                MainViewModel.GetInstance().RegistroSimulacionVm.Init(newSesion);
                RegistroSimulacionView registroSimulacionView = new RegistroSimulacionView();
                registroSimulacionView.Show();

                if (window != null)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }

        /// <summary>
        /// Método por el cual se envia a la vista de resultados 
        /// del registro de los sensores
        /// </summary>
        /// <param name="obj"></param>
        public void Resultado(Object obj) 
        {
            try
            {
                MainViewModel.GetInstance().ResultadoVm.Init(this.SesionLb.Id);
                ResultadoView resultadoView = new ResultadoView();
                resultadoView.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }

        public void Comparacion(Object obj) 
        {
            try
            {
                if (this.Sesiones.Count == 2)
                {
                    MainViewModel.GetInstance().ComparacionVm.Init(this.Sesiones[0].Id, this.Sesiones[1].Id);
                    ComparacionView comparacionView = new ComparacionView();
                    comparacionView.Show();
                }
                else 
                {
                    MessageBox.Show(
                    "Solo es posible mostrar estos graficos si hay dos sesiones",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }
        #endregion
    }
}
