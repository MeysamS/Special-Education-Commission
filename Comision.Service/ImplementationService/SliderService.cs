using System;
using System.Collections.Generic;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;

namespace Comision.Service.ImplementationService
{
    public class SliderService : ISliderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISliderRepository _sliderRepository;

        public SliderService(IUnitOfWork unitOfWork, ISliderRepository sliderRepository)
        {
            _unitOfWork = unitOfWork;
            _sliderRepository = sliderRepository;
        }
        public Tuple<bool, string> AddOrUpdate(Slider slider, StateOperation stateOperation, bool saveChange)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                    _sliderRepository.Add(slider);
                else
                    _sliderRepository.Update(slider);


                if (saveChange)
                    _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        public Tuple<bool, string> Add(Slider slider)
        {
            try
            {
                _sliderRepository.Add(slider);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }


        public Tuple<bool, string> Delete(int id)
        {
            try
            {
                _sliderRepository.Delete(x => x.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        public IList<Slider> GetListSlider()
        {
            return _sliderRepository.All().ToList();
        }
        public IList<string> GetListSliderPath(AddressUrlFile addressUrlFile)
        {
            return _sliderRepository.All().Select(s => addressUrlFile.SliderImage + s.ImageName).ToList();
        }


        public Slider Find(int id)
        {
            return _sliderRepository.Find(x => x.Id == id);
        }
    }
}
