using CalibrationTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CalibrationTask.Pages
{
    public class DeleteEquipmentModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public EquipmentModel? EquModel { get; set; }
        public DeleteEquipmentModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public IActionResult OnGet(int id)
        {
            EquModel = GetById(id);
            return Page();
        }
        private EquipmentModel GetById(int id)
        {
            var equRecord = new EquipmentModel();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandType = CommandType.StoredProcedure;
                tableCmd.CommandText = "GET_EQUIPMENT_DETAILS";
                tableCmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;

                using (SqlDataReader reader = tableCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new EquipmentModel()
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            Equipment_No = reader.GetString(reader.GetOrdinal("Equipment_No")),
                            Equipment_Desc = reader.GetString(reader.GetOrdinal("Equipment_Desc")),
                            Serial_No = reader.GetString(reader.GetOrdinal("Serial_No")),
                            Model = reader.GetString(reader.GetOrdinal("Model")),
                            Location = reader.GetString(reader.GetOrdinal("Location")),
                            Equipment_Start_Date = reader.GetDateTime(reader.GetOrdinal("Equipment_Start_Date")),
                        };
                    }
                    else
                    {
                        // No equipment found with the given ID
                        return null;
                    }
                }
            }
            return equRecord;
        }

        public IActionResult OnPost(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandType = CommandType.StoredProcedure;
                deleteCmd.CommandText = "DELETE_EQUIPMENT";
                deleteCmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;

                deleteCmd.ExecuteNonQuery();
            }

            return RedirectToPage("./Index");
        }
    }
}
