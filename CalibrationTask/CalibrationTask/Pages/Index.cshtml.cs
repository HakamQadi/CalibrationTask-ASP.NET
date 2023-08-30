using CalibrationTask.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CalibrationTask.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<EquipmentModel> EquipmentsList { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            EquipmentsList = GetAllEquipments();
        }

        private List<EquipmentModel> GetAllEquipments()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var getEquipmentsCMD = new SqlCommand("GET_ALL_EQUIPMENTS", connection);
                getEquipmentsCMD.CommandType = System.Data.CommandType.StoredProcedure;
                var equipmentsData = new List<EquipmentModel>();

                using (SqlDataReader reader = getEquipmentsCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        equipmentsData.Add(new EquipmentModel()
                        {
                            ID = reader.GetInt32(9),
                            Equipment_No = reader.GetString(0),
                            Equipment_Desc = reader.GetString(1),
                            Serial_No = reader.GetString(7),
                            Model = reader.GetString(3),
                            Location = reader.GetString(6),
                            Equipment_Start_Date = reader.GetDateTime(2),
                        });

                    }
                    return equipmentsData;
                }
            }
        }


    }
}