namespace CalibrationTask.Models
{
    public class CalibrationEntry
    {
        public string? CAL_NO { get; set; }
        public int Source { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime CompletedDate { get; set; }

        public string? CompletedBy { get; set; }
        public int? Status { get; set; }
        public bool IsCompleted { get; set; }
    }
}