using CalibrationTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CalibrationTask.Pages
{
    public class CalibrationDetailsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public EquipmentModel Equipment { get; set; }

        public List<CalibrationEntry> Calibration { get; set; }


        public CalibrationDetailsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(int Id)
        {
            Equipment = GetEquipmentByID(Id);
            Calibration = (GetCalibration(Id));
        }


        //Get One Equipment 
        public EquipmentModel GetEquipmentByID(int Id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var getEquipmentByIDCMD = new SqlCommand("GET_EQUIPMENT_DETAILS", connection);
                getEquipmentByIDCMD.CommandType = System.Data.CommandType.StoredProcedure;
                getEquipmentByIDCMD.Parameters.Add(new SqlParameter("@Id", Id));

                using (SqlDataReader reader = getEquipmentByIDCMD.ExecuteReader())
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
        }

        //Update One Equipment 
        public IActionResult OnPostScheduleCalibration(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var updateEquipmentCMD = connection.CreateCommand();
                updateEquipmentCMD.CommandType = CommandType.StoredProcedure;
                updateEquipmentCMD.CommandText = "UPDATE_EQUIPMENT";

                updateEquipmentCMD.Parameters.AddWithValue("@ID", id);
                updateEquipmentCMD.Parameters.AddWithValue("@Equipment_Desc", Equipment.Equipment_Desc);
                updateEquipmentCMD.Parameters.AddWithValue("@Model", Equipment.Model);
                updateEquipmentCMD.Parameters.AddWithValue("@Frequency", Equipment.Frequency);
                updateEquipmentCMD.Parameters.AddWithValue("@Remarks", Equipment.Remarks);
                updateEquipmentCMD.Parameters.AddWithValue("@Location", Equipment.Location);
                updateEquipmentCMD.Parameters.AddWithValue("@Serial_No", Equipment.Serial_No);
                updateEquipmentCMD.Parameters.AddWithValue("@Disabled_Date", Equipment.Disabled_Date);

                updateEquipmentCMD.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }

        //Get Calibration Register Table 
        public List<CalibrationEntry> GetCalibration(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var getEquipmentByIDCMD = new SqlCommand("Get_ALL_CALIBRATION", connection);
                getEquipmentByIDCMD.CommandType = System.Data.CommandType.StoredProcedure;
                getEquipmentByIDCMD.Parameters.Add(new SqlParameter("@EquipmentID", id));
                Console.WriteLine(id + " get all calibration id");
                var tableData = new List<CalibrationEntry>();
                using (SqlDataReader reader = getEquipmentByIDCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var entry = new CalibrationEntry
                        {
                            CAL_NO = reader.GetString(reader.GetOrdinal("CAL_NO")),
                            Source = reader.GetInt32(reader.GetOrdinal("Source_ID")),
                            PlanDate = reader.GetDateTime(reader.GetOrdinal("Plan_Date")),
                            CompletedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                            Status = reader.GetInt32(reader.GetOrdinal("Cal_Status")),
                        };

                        // Handling CompletedDate conversion
                        if (!reader.IsDBNull(reader.GetOrdinal("Completed_Date")))
                        {
                            entry.CompletedDate = reader.GetDateTime(reader.GetOrdinal("Completed_Date"));
                        }
                        else
                        {
                            entry.CompletedDate = DateTime.MinValue;
                        }

                        tableData.Add(entry);
                    }
                }
                return tableData;
            }
        }



        //public IActionResult OnPostScheduleCalibration(int id)
        //{
        //    DateTime start_date = ((DateTime)Equipment.Equipment_Start_Date).Date;
        //    var freq = Equipment.Frequency;
        //    int currentYear = start_date.Year;
        //    DateTime end_Date = start_date.AddMonths(freq);

        //    List<DateTime> generatedDates = new List<DateTime>();

        //    for (DateTime date = start_date; date <= end_Date; date = date.AddMonths(1))
        //    {
        //        if (date.Year == currentYear)
        //        {
        //            generatedDates.Add(date);
        //        }
        //    }
        //    return null;
        //}


        public IActionResult OnPost(int id)
        {
            //private EquipmentModel Equipment { get; set; }
            ////DateTime start_date = ((DateTime)Equipment.Equipment_Start_Date).Date;
            //DateTime? nullableStartDate = Equipment.Equipment_Start_Date;
            //DateTime start_date = nullableStartDate.HasValue ? nullableStartDate.Value : default(DateTime);
            //var freq = Equipment.Frequency;
            //int currentYear = start_date.Year;
            ////DateTime end_Date = start_date.AddMonths(freq);
            //DateTime end_Date = Convert.ToDateTime("2023 - 12 - 30 00:00:00");



            //List<DateTime> generatedDates = new List<DateTime>();

            //for (DateTime date = start_date; date <= end_Date; date = date.AddMonths(3))
            //{
            //    if (date.Year == currentYear)
            //    {
            //        generatedDates.Add(date);
            //    }
            //}
            //Console.WriteLine(id + " ID before");



            //using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            //{
            //    connection.Open();

            //    using (var addCMD = new SqlCommand("UPDATE_EQUIPMENT", connection))
            //    {
            //        addCMD.CommandType = System.Data.CommandType.StoredProcedure;
            //        addCMD.Parameters.AddWithValue("@Frequency", Equipment.Frequency);
            //        addCMD.ExecuteNonQuery();

            //    }
            //}




            Equipment = GetEquipmentByID(id);
            //Console.WriteLine(id + " ID after");

            //DateTime? nullableStartDate = Convert.ToDateTime("2023-4-30 00:00:00");
            var nullableStartDate = Equipment.Equipment_Start_Date;

            DateTime start_date = nullableStartDate.HasValue ? nullableStartDate.Value : default(DateTime);

            //var freq = Equipment.Frequency;

            Console.WriteLine(Equipment.Equipment_Start_Date + "HEEEEELLO");
            //Console.WriteLine(Equipment.Frequency + " freq befor");
            var freq = Equipment.Frequency;
            Console.WriteLine(freq + " freq after");

            int currentYear = start_date.Year;
            DateTime end_Date = Convert.ToDateTime("2023-12-30 00:00:00");

            List<DateTime> generatedDates = new List<DateTime>();

            for (DateTime date = start_date; date <= end_Date; date = date.AddMonths(freq))
            {
                if (date.Year == currentYear)
                {
                    generatedDates.Add(date);
                }
            }

            // Call the stored procedure to insert generatedDates into the database
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CREATE_CALIBRATION", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (DateTime date in generatedDates)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add("@PlanDate", SqlDbType.DateTime).Value = date;  // Set the correct DateTime parameter
                                                                                               // Add other parameters here as needed
                        command.Parameters.AddWithValue("@OrgID", 2); // Empty value
                        command.Parameters.AddWithValue("@CompID", 2); // Empty value
                        command.Parameters.AddWithValue("@CALNo", "cal"); // Empty value
                        command.Parameters.AddWithValue("@EquipmentNo", "num"); // Empty value
                        command.Parameters.AddWithValue("@EquipmentID", id); // Empty value
                        command.Parameters.AddWithValue("@SourceID", 2); // Empty value
                        command.Parameters.AddWithValue("@EmpID", 4); // Empty value
                        command.Parameters.AddWithValue("@SUPID", 4); // Empty value
                        command.Parameters.AddWithValue("@CompletedDate", "2023-08-30 10:00:00"); // Empty value
                        command.Parameters.AddWithValue("@CalibrationPeriod", "Annual"); // Empty value
                        command.Parameters.AddWithValue("@MeasurementRange", "0-100"); // Empty value
                        command.Parameters.AddWithValue("@Tolerance", "0.5"); // Empty value
                        command.Parameters.AddWithValue("@CalibrationMethod", "A"); // Empty value
                        command.Parameters.AddWithValue("@CalibrationStandard", "XYZ"); // Empty value
                        command.Parameters.AddWithValue("@CalibrationResult", "pass"); // Empty value
                        command.Parameters.AddWithValue("@Notes", "no"); // Empty value
                        command.Parameters.AddWithValue("@Remarks", "done"); // Empty value
                        command.Parameters.AddWithValue("@ApprovedBy", 7); // Empty value
                        command.Parameters.AddWithValue("@LastModifiedDate", "2023-08-30 10:00:00"); // Empty value
                        command.Parameters.AddWithValue("@LastModifiedBy", 2155); // Empty value
                        command.Parameters.AddWithValue("@Cal_Status", 0); // Empty value
                        command.Parameters.AddWithValue("@CreatedBy", " "); // Empty value

                        command.ExecuteNonQuery();
                    }
                }
            }

            return RedirectToPage("/index");
        }



    }









}
