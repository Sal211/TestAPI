using WebApplication1.Models.Connection;
using System.Data.SqlClient;

namespace SchoolManagement.Models 
{
    public class ClsConnection
    {
        private int Errcode;
        private string ErrMsg;      
        private SqlConnection con;

        public int _Errcode
        {
            get { return Errcode; }
        }
        public string _ErrMsg { get { return ErrMsg; } }   
        public SqlConnection _con { get { return con;  } set { con = value; } }
        public SqlCommand _cmd { get;set; }
        public SqlDataAdapter _Ad { get; set; }

        public ClsConnection()
        {
            getConnection();
        }
        private void getConnection()
        {
            try
            {
                string constr = ClsConstring.Constr;
                con = new SqlConnection(constr);
                if (con.State == System.Data.ConnectionState.Closed) con.Open();

                if (con.State == System.Data.ConnectionState.Open) Errcode = 0;
                else
                {
                    Errcode = 99999;
                    ErrMsg = "Unknow";
                }
            }catch(Exception ex)
            {
                Errcode = ex.HResult;
                ErrMsg = ex.Message;
            }
        }
    }
}
