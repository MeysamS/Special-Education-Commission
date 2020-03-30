using System;
using Comision.Model.Domain;

namespace Comision.Service.IService
{
    public interface ISettingService
    {
        Tuple<bool, string> AddOrUpdateSettings(Settings settings);
        Settings GetSettings();
        Settings Find(int id);
    }
}
