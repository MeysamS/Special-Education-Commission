using System;
using System.Data.Entity;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;
using Comision.Service.IService;

namespace Comision.Service.ImplementationService
{
    public class SettingService:ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IUniversityRepository _universityRepository;

        public SettingService(IUnitOfWork unitOfWork, ISettingsRepository settingsRepository,
            IUniversityRepository universityRepository)
        {
            _unitOfWork = unitOfWork;
            _settingsRepository = settingsRepository;
            _universityRepository = universityRepository;
        }
        public Tuple<bool, string> AddOrUpdateSettings(Settings settings)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _settingsRepository.AddOrUpdate(settings);
                var uv = _universityRepository.Find(q=>q.Id== settings.UniversityId);
                uv.Name = settings.University.Name;
                uv.Address = settings.University.Address;
                uv.Logo = settings.University.Logo;
                _universityRepository.Update(uv);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        public Settings Find(int id)
        {
            return _settingsRepository.Find(x => x.UniversityId == id);
        }

        public Settings GetSettings()
        {
            try
            {
                var s = _settingsRepository.All().Include(x => x.University).FirstOrDefault();
                return s;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
