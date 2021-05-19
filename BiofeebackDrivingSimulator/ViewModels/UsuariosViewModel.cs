using BiofeebackDrivingSimulator.Datos;
using BiofeebackDrivingSimulator.Interface;
using BiofeebackDrivingSimulator.Models;
using BiofeebackDrivingSimulator.Services;
using BiofeebackDrivingSimulator.ViewModels.Base;
using BiofeebackDrivingSimulator.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class UsuariosViewModel : BaseViewModel
    {
        #region Variables Locales
        private UsuarioPlano usuario;
        #endregion

        #region Propiedades
        public UsuarioPlano Usuario
        {
            get { return this.usuario; }
            set { SetProperty(ref this.usuario, value); }
        }
        public List<Usuario> Usuarios { get; set; }
        public List<UsuarioPlano> UsuariosLocal { get; set; }
        #endregion

        #region Contructor
        public UsuariosViewModel()
        {
            this.usuario = new UsuarioPlano();
            Init();
            this.CommandAgregarNuevo = new RelayCommand<ICloseable>(this.AgregarNuevo);
            this.CommandConfiguracionComPort = new RelayCommand<ICloseable>(this.ConfiguracionComPort);
            this.DoubleClickCommand = new RelayCommand<ICloseable>(this.Editar);
        }
        #endregion

        #region Commands
        //public ICommand DoubleClickCommand => new DelegateCommand(Editar, null);
        public RelayCommand<ICloseable> CommandAgregarNuevo { get; private set; }
        public RelayCommand<ICloseable> DoubleClickCommand { get; private set; }
        public RelayCommand<ICloseable> CommandConfiguracionComPort { get; private set; }
        #endregion

        #region Metodos
        /// <summary>
        /// Método de inicialización de la clase el cual trae de la base 
        /// de datos todos los usuarios registrados actualmente
        /// </summary>
        public async void Init()
        {
            this.Usuarios = new List<Usuario>();
            this.UsuariosLocal = new List<UsuarioPlano>();
            try
            {
                //using (var entidades = new Entidades())
                //{
                //    this.Usuarios = entidades.Usuarios.Include("Sesiones").ToList();
                //}

                using (var context = new Entidades())
                {
                    UsuarioServices services = new UsuarioServices(context);
                    this.Usuarios = await services.ObtenerUsuariosAsync();

                    foreach (var item in this.Usuarios)
                    {
                        var usuarioLocal = new UsuarioPlano
                        {
                            Id = item.Id,
                            Apellidos = item.Apellidos,
                            Edad = item.Edad,
                            Nombres = item.Nombres
                        };

                        if (item.Sexo)
                        {
                            usuarioLocal.Sexo = "Femenino";
                        }
                        else
                        {
                            usuarioLocal.Sexo = "Masculino";
                        }

                        if (item.Sesiones == null)
                        {
                            usuarioLocal.Sesiones = 0;
                        }
                        else
                        {
                            usuarioLocal.Sesiones = item.Sesiones.Count;
                        }
                        this.UsuariosLocal.Add(usuarioLocal);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                MessageBox.Show(
                    "Ocurrió un problema al cargar los datos",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Método por el cual se va a otra vista para poder editar
        /// al usuario selecionado
        /// </summary>
        /// <param name="window"></param>
        public void Editar(ICloseable window)
        {
            try
            {
                var user = new Usuario
                {
                    Id = this.Usuario.Id,
                    Apellidos = this.Usuario.Apellidos,
                    Edad = this.Usuario.Edad,
                    Nombres = this.Usuario.Nombres,
                };

                if (this.Usuario.Sexo == "Masculino")
                {
                    user.Sexo = false;
                }
                else
                {
                    user.Sexo = true;
                }

                MainViewModel.GetInstance().RegistroUsuarioVm.Init(user);
                if (!Application.Current.Windows.OfType<RegistroUsuarioView>().Any())
                {
                    RegistroUsuarioView registroUsuarioView = new RegistroUsuarioView();
                    registroUsuarioView.Show();

                    if (window != null)
                    {
                        window.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                MessageBox.Show(
                    "Ocurrió un problema al cargar la nueva pestaña",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Método por el cual se va a la vista para
        /// registrar un nuevo usiario
        /// </summary>
        /// <param name="window"></param>
        private void AgregarNuevo(ICloseable window)
        {
            try
            {
                MainViewModel.GetInstance().RegistroUsuarioVm.Init();
                if (!Application.Current.Windows.OfType<RegistroUsuarioView>().Any())
                {
                    RegistroUsuarioView registroUsuarioView = new RegistroUsuarioView();
                    registroUsuarioView.Show();

                    if (window != null)
                    {
                        window.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                MessageBox.Show(
                        "Ocurrió un problema al cargar la nueva pestaña",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }

        }

        public void ConfiguracionComPort(ICloseable window) 
        {
            try
            {
                MainViewModel.GetInstance().PuertosVm.Init();
                if (!Application.Current.Windows.OfType<PuertosView>().Any())
                {
                    PuertosView puertosView = new PuertosView();
                    puertosView.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                        "Ocurrió un problema al cargar la configuración",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
