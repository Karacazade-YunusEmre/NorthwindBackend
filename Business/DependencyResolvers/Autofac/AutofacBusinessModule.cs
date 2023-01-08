using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EfProductRepository>().As<IProductRepository>();
        builder.RegisterType<EfCategoryRepository>().As<ICategoryRepository>();
        builder.RegisterType<ProductManager>().As<IProductService>();
        builder.RegisterType<CategoryManager>().As<ICategoryService>();
    }
}