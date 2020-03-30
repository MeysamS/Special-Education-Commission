using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Comision.Service
{
    public class ServiceSetupDependency : Autofac.Module
    {
        //با طراحی ماژولار تنظیمات Autofac و استفاده از Interface‌ها برای ارتباط با دیگر قسمتها، 
        //دیگر نیازی نیست دسترسی‌های بی‌موردی از یک لایه را  به سایر قسمت‌ها داد 
        //و دیگر نیازی نیست شما نگران این باشید که ممکن است یکی  از توسعه دهندگان به‌دلایلی مانند کم تجربگی،
        //کاری را خارج از زیرساختی که شما یا گروه پیاده سازی زیرساخت، پیاده سازی کرده‌اید انجام دهد.
        protected override void Load(ContainerBuilder builder)
        {
           // base.Load(builder);

           // builder.RegisterAssemblyModules(typeof(Repository.RepositorySetupDependency).Assembly);
           // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
           //.Where(x => x.Namespace.EndsWith("ImplementationService"))
           //.AsImplementedInterfaces();

           //// builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
           ////.Where(x => x.Namespace.EndsWith("Business.Core"))
           ////.AsImplementedInterfaces();

           //// builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
           //// .Where(x => x.Namespace.EndsWith("Business.Businesses"))
           //// .AsImplementedInterfaces();
        }
    }
}
