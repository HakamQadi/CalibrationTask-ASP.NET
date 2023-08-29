namespace CalibrationTask.Models
{
    public class EquipmentModel
    {
        public int ID { get; set; }
        public int Org_ID { get; set; }
        public int Comp_ID { get; set; }
        public string? Equipment_No { get; set; }
        public string? Equipment_Desc { get; set; }
        public string? Serial_No { get; set; }
        public string? Model { get; set; }
        public string? Location { get; set; }
        public int Frequency { get; set; }
        public DateTime? Equipment_Start_Date { get; set; }
        public string? Remarks { get; set; }
        public DateTime? Disabled_Date { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}