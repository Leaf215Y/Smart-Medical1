using System;

namespace Smart_Medical.Appointment
{
    public class UserPatientDto
    {
        public Guid PatientId { get; set; }
        public string VisitId { get; set; }
        public string PatientName { get; set; }
        public int Gender { get; set; }
        public int? Age { get; set; }
        public string AgeUnit { get; set; }
        public string ContactPhone { get; set; }
        public string IdNumber { get; set; }
        public string VisitType { get; set; }
        public bool IsInfectiousDisease { get; set; }
        public DateTime? DiseaseOnsetTime { get; set; }
        public DateTime? EmergencyTime { get; set; }
        public string VisitStatus { get; set; }
        public DateTime VisitDate { get; set; }
        // 可根据需要扩展
    }
}
