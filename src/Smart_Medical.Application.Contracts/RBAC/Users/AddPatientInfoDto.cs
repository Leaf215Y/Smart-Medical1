using System;

namespace Smart_Medical.Application.Contracts.RBAC.Users
{
    public class AddPatientInfoDto
    {
        public string PatientName { get; set; }
        public int? Age { get; set; }
        public string AgeUnit { get; set; }
        public bool? Gender { get; set; }
        public string ContactPhone { get; set; }
        public string IdNumber { get; set; }
        public string VisitType { get; set; }
        public bool? IsInfectiousDisease { get; set; }
        public DateTime? DiseaseOnsetTime { get; set; }
        public DateTime? EmergencyTime { get; set; }
        public string VisitStatus { get; set; }
        public DateTime? VisitDate { get; set; }
    }
}