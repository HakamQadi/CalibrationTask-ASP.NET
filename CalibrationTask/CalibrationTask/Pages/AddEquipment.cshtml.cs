using CalibrationTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CalibrationTask.Pages
{
    public class AddEquipmentModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public AddEquipmentModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet()
        {
            Equipment = new EquipmentModel();
            return Page();
        }
        [BindProperty]
        public EquipmentModel? Equipment { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                using (var addCMD = new SqlCommand("ADD_EQUIPMENT", connection))
                {
                    addCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    addCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    addCMD.Parameters.AddWithValue("@Org_ID", Equipment.Org_ID);
                    addCMD.Parameters.AddWithValue("@Comp_ID", Equipment.Comp_ID);
                    addCMD.Parameters.AddWithValue("@Equipment_No", Equipment.Equipment_No);
                    addCMD.Parameters.AddWithValue("@Equipment_Desc", Equipment.Equipment_Desc);
                    addCMD.Parameters.AddWithValue("@Serial_No", Equipment.Serial_No);
                    addCMD.Parameters.AddWithValue("@Model", Equipment.Model);
                    addCMD.Parameters.AddWithValue("@Location", Equipment.Location);
                    addCMD.Parameters.AddWithValue("@Frequency", Equipment.Frequency);
                    addCMD.Parameters.AddWithValue("@Equipment_Start_Date", Equipment.Equipment_Start_Date);
                    addCMD.Parameters.AddWithValue("@Remarks", Equipment.Remarks);
                    addCMD.ExecuteNonQuery();

                }
                connection.Close();


            }
            return RedirectToPage("./Index");

        }
    }
}
