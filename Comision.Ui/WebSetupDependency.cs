using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace Comision.Ui
{
    public class WebSetupDependency : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
          //  base.Load(builder);
          //  builder.RegisterAssemblyModules(typeof(Service.ServiceSetupDependency).Assembly);
          //  builder.RegisterControllers(Assembly.GetExecutingAssembly());
          //  //سایر تنظیمات
          //  var container = builder.Build();

          ////نکته‌ای را که باید در اینجا در نظر بگیرید، CommonDependencyResolver است که مسئول ثبت Container در قسمت Common است. به این صورت دیگر تنها لایه‌ای که به Container دسترسی دارد، لایه Web نیست.در واقع با ثبت Container در قسمت Common شما دسترسی کنترل شده‌ای را از Container به سایر لایه‌های سیستم داده‌اید و در این حالت در صورتیکه لایه‌های دیگر مانند DataAccess یا Business به Container نیاز پیدا کنند، می‌توانند از طریق CommonDependencyResolver این دسترسی را داشته باشند.
          ////CommonDependencyResolver.SetContainer(container);
          //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
    public class AutofacConfig
    {
        ////با طراحی ماژولار تنظیمات Autofac و استفاده از Interface‌ها برای ارتباط با دیگر قسمتها، 
        ////دیگر نیازی نیست دسترسی‌های بی‌موردی از یک لایه را  به سایر قسمت‌ها داد 
        ////و دیگر نیازی نیست شما نگران این باشید که ممکن است یکی  از توسعه دهندگان به‌دلایلی مانند کم تجربگی،
        ////کاری را خارج از زیرساختی که شما یا گروه پیاده سازی زیرساخت، پیاده سازی کرده‌اید انجام دهد.
        //public static void SetupContainer()
        //{
        //    var builder = new ContainerBuilder();
        //    builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
        //    var container = builder.Build();
        //    //CommonDependencyResolver.SetContainer(container);
        //    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        //}
    }
}