using Api.Clinic.Enterprise;
using Library.Clinic.DTO;
using Library.Clinic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Clinic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<PatientDTO> Get()
        {
            return (IEnumerable<PatientDTO>)new PatientEC().Patients;
        }

        [HttpGet("{id}")]
        public PatientDTO? GetById(int id)
        {
            return new PatientEC().GetById(id);
        }

        [HttpDelete("{Id}")]
        public void Delete(int Id)
        {
            new PatientEC().Delete(Id);
        }

        [HttpPost("Search")]

        public List<PatientDTO> Search([FromBody] Query q)
        {
            return new PatientEC().Search(q?.Content ?? string.Empty)?.ToList() ?? new List<PatientDTO>();
        }

        [HttpPost("UpdatePatient")]
        public PatientDTO Update([FromBody] PatientDTO patient)
        {
            return new PatientEC().Update(patient);
        }

        [HttpPost]
        public PatientDTO Create([FromBody] PatientDTO patient)
        {
            return new PatientEC().Create(patient);
        }

    }
}
