using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using SchoolManagement.Models;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;
using WebApplication1.Models.Connection;
namespace WebApplication1.Controllers
{
    [BasicAuth]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
       
        private int PostDataStudent(Dictionary<string, object> DataStudent, string query)
        {
            ClsConnection con = new ClsConnection();
            if (con._Errcode == 0)
            {
                try
                {
                    con._cmd = new SqlCommand(query, con._con);
                    AddParameters(con._cmd, DataStudent);
                    return con._cmd.ExecuteNonQuery() > 0 ? 1 : 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return -1;
                }
                finally
                {
                    con._con.Close();
                }
            }
            return 999;
        }

        private void AddParameters(SqlCommand command, Dictionary<string, object> DataStudent)
        {
            foreach (var student in DataStudent)
            {
                command.Parameters.AddWithValue(student.Key, student.Value);
            }
        }
        private List<Student> GetDataStudent(string query)
        {
            ClsConnection con = new ClsConnection();
            DataTable dt = new DataTable();
            List<Student> lstStudents = new List<Student>();
            if (con._Errcode == 0)
            {
                con._Ad = new SqlDataAdapter(query, con._con);
                con._Ad.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    var objStudent = new Student
                    {
                        Age = int.Parse(row["Age"].ToString()),
                        Name = row["Name"].ToString(),
                        ID = int.Parse(row["ID"].ToString())
                    };
                    lstStudents.Add(objStudent);
                }
                con._con.Close();
            }          
             return lstStudents;            
        }

        private int FindStudent(int Id)
        {
            ClsConnection con = new ClsConnection();
            if(con._Errcode == 0)
            {
                try
                {
                    string query = "sp_FindStudent '" + Id + "'";
                    con._cmd = new SqlCommand(query, con._con);
                    int count = (int)con._cmd.ExecuteScalar();
                    return count;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return -1;
                }
            }
            return 0;
        }
        [HttpGet]
        public IActionResult GetStudent()
        {
            string query = "EXEC sp_GetStudent";
            return GetDataStudent(query) != null ?  Ok(GetDataStudent(query)) : BadRequest("Database connection error.");
        }
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("PostStudent")]
        public IActionResult PostStudent([FromBody] Student objStudent)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            string query = "EXEC sp_InsertStudent @Name, @Age";
            Dictionary<string, object> DataStudent = new Dictionary<string, object>
            {
                { "@Name", objStudent.Name },
                { "@Age", objStudent.Age }
            };

            try
            {
                int result = PostDataStudent(DataStudent, query);
                return (result > 0 ? NoContent() : result == -1 ? StatusCode(500, "Error occurred while inserting data.") : StatusCode(500, "Database connection error."));             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("UpdateStudent")]
        public IActionResult UpdateStudent([FromBody] Student objStudent)
        {
            if (FindStudent(objStudent.ID) <= 0) return NotFound("Student Not Found");
            string query = "EXEC sp_UpdateStudent @ID,@Name, @Age";
            Dictionary<string, object> DataStudent = new Dictionary<string, object>
            {
                { "@Name", objStudent.Name },
                { "@Age", objStudent.Age },
                { "ID", objStudent.ID }
            };

            try
            {
                int result = PostDataStudent(DataStudent, query);
                return (result > 0 ? NoContent() : result == -1 ? StatusCode(500, "Error occurred while inserting data.") : StatusCode(500, "Database connection error."));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
         [HttpPost("DeleteStudent")]
        public IActionResult DeleteStudent([FromBody]int ID)
        {
            if (FindStudent( ID) <= 0) return NotFound("Student Not Found");
            ClsConnection con = new ClsConnection(); 
            if(con._Errcode == 0)
            {
                string query = "Exec sp_DeleteStudent '" + ID + "'";
                con._cmd = new SqlCommand(query, con._con);
                con._cmd.ExecuteNonQuery();
            }
            else
            {
                return BadRequest("Database connection error.");
            }   
            return NoContent();
        }
  
        [HttpGet]
        [Route("SearchStudent/{Id}")]

        public IActionResult GetStudentByID(int Id)
        {
            if (FindStudent(Id) <= 0) return NotFound("Student Not Found");
            string query = "EXEC sp_GetStudentById '"+Id+"'";
            return GetDataStudent(query) != null ?  NoContent() : BadRequest("Database connection error.");
        }
    }


}
