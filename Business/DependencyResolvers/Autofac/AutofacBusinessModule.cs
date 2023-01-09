using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;

namespace Business.DependencyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EfProductRepository>().As<IProductRepository>();
        builder.RegisterType<EfCategoryRepository>().As<ICategoryRepository>();
        builder.RegisterType<ProductManager>().As<IProductService>();
        builder.RegisterType<CategoryManager>().As<ICategoryService>();
        builder.RegisterType<NorthwindContext>().As<NorthwindContext>();
        builder.RegisterType<EfUserRepository>().As<IUserRepository>();
        builder.RegisterType<UserManager>().As<IUserService>();
    }
}