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
            List<string> datospedido = this.repo.GetPedidos(nombre);
            foreach (string pedido in datospedido)
            {
                this.lstpedidos.Items.Add(pedido);

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

                this.txtcodigopedido.Clear();
                this.txtfechaentrega.Clear();
                this.txtformaenvio.Clear();
                this.txtimporte.Clear();
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //select
            string codigo = this.lstpedidos.SelectedItem.ToString();

            //AQUI VA EL CODIGO CON EL PROCEDURE DE SP_DATOS_PEDIDO
            List<Pedido> datospedido = this.repo.GetDatosPedido(codigo);
            foreach(Pedido pedido in datospedido)
            {
                this.txtcodigopedido.Text = pedido.CodigoPedido;
                this.txtfechaentrega.Text = pedido.FechaEntrega;
                this.txtformaenvio.Text = pedido.FormaEnvio;
                this.txtimporte.Text = pedido.Importe.ToString();
            }

            
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {

            string codigopedido = this.txtcodigopedido.Text;
            string codigocliente = this.cmbclientes.SelectedItem.ToString();
            DateTime fechaentrega = DateTime.Parse(this.txtfechaentrega.Text.ToString());
            string formatoenvio = this.txtformaenvio.Text;
            int importe = int.Parse(this.txtimporte.Text);

            this.repo.InsertPedidos(codigopedido, codigocliente, fechaentrega, formatoenvio, importe);
            this.LoadPedido();
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string codped = this.lstpedidos.SelectedItem.ToString();
            this.repo.DeletePedido(codped);
            this.LoadPedido();
        }
    }
}
