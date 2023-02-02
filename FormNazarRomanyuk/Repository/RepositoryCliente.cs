using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using FormNazarRomanyuk.Helpers;
using FormNazarRomanyuk.Models;
using System.Data;
using static System.ComponentModel.Design.ObjectSelectorEditor;
#region PROCEDURE
//CREATE PROCEDURE SP_EMPRESA
//AS 
//SELECT EMPRESA FROM CLIENTES
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


//CREATE PROCEDURE SP_DATO_PEDIDO
//(@CODPEDIDO NVARCHAR(50))
//AS
//SELECT * FROM PEDIDOS WHERE CODIGOPEDIDO = @CODPEDIDO
//GO

//CREATE PROCEDURE SP_INSERT_PEDIDOS
//(@CODPEDIDO NVARCHAR(50),
//@NOMBREEMP NVARCHAR(50),
//@FECHA DATE,
//@FORMAENVIO NVARCHAR(50),
//@IMPORTE INT)
//AS
//DECLARE @CODCLI NVARCHAR(50)
//SELECT @CODCLI = CODIGOCLIENTE FROM CLIENTES WHERE EMPRESA = @NOMBREEMP
//INSERT INTO PEDIDOS VALUES (@CODPEDIDO, @CODCLI, @FECHA, @FORMAENVIO, @IMPORTE)
//GO


//CREATE PROCEDURE SP_DELETE_PEDIDO
//(@CODPED NVARCHAR(50))
//AS
//DELETE FROM PEDIDOS WHERE CODIGOPEDIDO = @CODPED
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


        public List<string> GetPedidos(string codcliente)
        {
            SqlParameter pamnombre = new SqlParameter("@NOMBRE", codcliente);
            this.com.Parameters.Add(pamnombre);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_CLIENTE";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<string> datospedido = new List<string>();
            while (this.reader.Read())
            {
                string codigopedido = this.reader["CODIGOPEDIDO"].ToString();
                datospedido.Add(codigopedido);

            }

            this.cn.Close();
            this.reader.Close();
            this.com.Parameters.Clear();

            return datospedido;
        }


        public List<Pedido> GetDatosPedido(string codigo)
        {
            SqlParameter pamcodigo = new SqlParameter("@CODPEDIDO", codigo);
            this.com.Parameters.Add(pamcodigo);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DATO_PEDIDO";

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

        public int InsertPedidos(string codped,string nombre, DateTime fecha, string formenvio, int importe)
        {
            SqlParameter pamcodped = new SqlParameter("@CODPEDIDO", codped);
            this.com.Parameters.Add(pamcodped);

            SqlParameter pamnombre = new SqlParameter("@NOMBREEMP", nombre);
            this.com.Parameters.Add(pamnombre);

            SqlParameter pamfecha = new SqlParameter("@FECHA", fecha);
            this.com.Parameters.Add(pamfecha);

            SqlParameter pamformenvio = new SqlParameter("@FORMAENVIO", formenvio);
            this.com.Parameters.Add(pamformenvio);

            SqlParameter pamimporte = new SqlParameter("@IMPORTE", importe);
            this.com.Parameters.Add(pamimporte);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_PEDIDOS";

            this.cn.Open();
            int update = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return update;

        }


        public int DeletePedido(string cod)
        {
            SqlParameter pamcod = new SqlParameter("@CODPED", cod);
            this.com.Parameters.Add(pamcod);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_PEDIDO";

            this.cn.Open();
            int delete = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return delete;
        }
    }

}
