using System;
using System.Collections.Generic;
using Comision.Model.Domain;
using Comision.Service.Enum;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface ISliderService
    {
        Tuple<bool, string> AddOrUpdate(Slider slider, StateOperation stateOperation, bool saveChange);
        Tuple<bool, string> Add(Slider slider);
        Tuple<bool, string> Delete(int id);
        IList<Slider> GetListSlider();
        IList<string> GetListSliderPath(AddressUrlFile addressUrlFile);
        Slider Find(int id);
    }
}
