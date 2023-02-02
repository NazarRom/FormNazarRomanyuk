using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using FormNazarRomanyuk.Helpers;
using FormNazarRomanyuk.Models;
using System.Data;
#region PROCEDURE
//CREATE PROCEDURE SP_CLIENTES
//AS 
//SELECT * FROM CLIENTES
//GO


//CREATE PROCEDURE SP_EMPRESA_DATOS
//(@NOMBRE NVARCHAR(50))
//AS
//SELECT * FROM CLIENTES WHERE EMPRESA = @NOMBRE
//GO

//CREATE PROCEDURE SP_PEDIDOS_CLIENTE
//(@NOMBRE NVARCHAR(50))
//AS
//DECLARE @CODCLIENTE NVARCHAR(50)
//SELECT @CODCLIENTE = CODIGOCLIENTE FROM CLIENTES WHERE EMPRESA = @NOMBRE
//SELECT * FROM PEDIDOS WHERE CODIGOCLIENTE = @CODCLIENTE
//GO

#endregion
namespace FormNazarRomanyuk.Repository
{
    public class RepositoryCliente
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryCliente()
        {
            //realizamos la conexion extraida de los helpers
            string connectionString = HelperConfiguration.GetConnectionString();
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<string> GetClientes()
        {
            //defino el tipo
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_EMPRESA";

            //abro conexion y el reader
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<string> empresas = new List<string>();
            while (this.reader.Read())
            {
                string empresa = this.reader["EMPRESA"].ToString();
                empresas.Add(empresa);
            }
            //cierro conexion y el reader
            this.cn.Close();
            this.reader.Close();
            //devuelvo la lista
            return empresas;
        }

        public List<Cliente> GetDatosCliente(string nombre)
        {
            //defino parametros
            SqlParameter pamnombre = new SqlParameter("@NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);

            //tipo
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_EMPRESA_DATOS";

            //conexion
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<Cliente> datosCliente = new List<Cliente>();
            while (this.reader.Read())
            {
                string codigocliente = this.reader["CODIGOCLIENTE"].ToString();
                string empresa = this.reader["EMPRESA"].ToString();
                string contacto = this.reader["CONTACTO"].ToString();
                string cargo = this.reader["CARGO"].ToString();
                string ciudad = this.reader["CIUDAD"].ToString();
                int telefono = int.Parse(this.reader["TELEFONO"].ToString());
                Cliente cliente = new Cliente();
                cliente.CodigoCliente = codigocliente;
                cliente.Empresa = empresa;
                cliente.Contacto = contacto;
                cliente.Cargo = cargo;
                cliente.Ciudad = ciudad;
                cliente.Telefono = telefono;
                datosCliente.Add(cliente);

            }
            //cierrro cn param y reader
            this.cn.Close();
            this.reader.Close();
            this.com.Parameters.Clear();

            return datosCliente;

        }

        public List<Pedido> GetPedidos(string nombre)
        {
            SqlParameter pamnombre = new SqlParameter("@NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_CLIENTE";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<Pedido> datospedido = new List<Pedido>();
            while (this.reader.Read())
            {
                string codigopedido = this.reader["CODIGOPEDIDO"].ToString();
                string codigocliente = this.reader["CODIGOCLIENTE"].ToString();
                string fechaentrega = this.reader["FECHAENTREGA"].ToString();
                string formatoenvio = this.reader["FORMAENVIO"].ToString();
                int importe = int.Parse(this.reader["IMPORTE"].ToString());

                Pedido pedido = new Pedido();
                pedido.CodigoPedido = codigopedido;
                pedido.CodigoCliente = codigocliente;
                pedido.FechaEntrega = fechaentrega;
                pedido.FormaEnvio = formatoenvio;
                pedido.Importe = importe;
                datospedido.Add(pedido);
            }

            this.cn.Close();
            this.reader.Close();
            this.com.Parameters.Clear();

            return datospedido;
        }

    }

}
