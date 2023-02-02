using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormNazarRomanyuk.Repository;
using FormNazarRomanyuk.Models;

namespace FormNazarRomanyuk
{
    public partial class FormPractica : Form
    {
        RepositoryCliente repo;
        public FormPractica()
        {
            InitializeComponent();
            this.repo = new RepositoryCliente();
            this.LoadEmpresa();
        }

        private void LoadEmpresa()
        {
            List<string> empresas = this.repo.GetClientes();
            this.cmbclientes.Items.Clear();
            foreach(string empresa in empresas)
            {
                this.cmbclientes.Items.Add(empresa);
            }
        }
        private void LoadPedido()
        {
            string nombre = this.cmbclientes.SelectedItem.ToString();
            this.lstpedidos.Items.Clear();
            List<Pedido> datospedido = this.repo.GetPedidos(nombre);
            foreach (Pedido pedido in datospedido)
            {
                this.lstpedidos.Items.Add(pedido.CodigoPedido);

            }
        }
            private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbclientes.SelectedIndex != -1)
            {
                string nombre = this.cmbclientes.SelectedItem.ToString();
                List<Cliente> datoscliente = this.repo.GetDatosCliente(nombre);
               
                foreach(Cliente cliente in datoscliente)
                {
                    this.txtempresa.Text = cliente.Empresa;
                    this.txtcontacto.Text = cliente.Contacto;
                    this.txtcargo.Text = cliente.Cargo;
                    this.txtciudad.Text = cliente.Ciudad;
                    this.txttelefono.Text = cliente.Telefono.ToString();
                    this.LoadPedido();
                }
                
            
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //select
            string nombre = this.lstpedidos.SelectedItem.ToString();

            //AQUI VA EL CODIGO CON EL PROCEDURE DE SP_DATOS_PEDIDO
            //List<Pedido> datospedido = this.repo.GetPedidos(nombre);
            //foreach (Pedido pedido in datospedido)
            //{
            //    this.txtcodigopedido.Text = pedido.CodigoPedido;
            //    this.txtfechaentrega.Text = pedido.FechaEntrega;
            //    this.txtformaenvio.Text = pedido.FormaEnvio;
            //    this.txtimporte.Text = pedido.Importe.ToString();

            //}
        }
    }
}
