using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class StudentRepository : MainRepository<Student>, IStudentRepository
    {
        readonly IDbSet<Student> _student;
        IUnitOfWork _unitOfWork;
        public StudentRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _student = _unitOfWork.Set<Student>();
        }
    }
}
