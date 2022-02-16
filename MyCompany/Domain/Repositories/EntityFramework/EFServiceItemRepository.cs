using MyCompany.Domain.Entities;
using System.Linq;
using System;
using MyCompany.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.Domain.Repositories.EntityFramework
{
    public class EFServiceItemRepository: IServiceItemRepository
    {
        private readonly AppDbContext context;
        public EFServiceItemRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ServiceItem> GetServiceItems()//Take all entries from ServiceItems
        {
            return context.ServiceItems;
        }

        public ServiceItem GetServiceItemById(Guid id)//Take one entry from ServiceItems
        {
            return context.ServiceItems.FirstOrDefault(x => x.Id == id);
        }

        public void SaveServiceItem(ServiceItem entity)//if id is default marks with a flag that this is a new object,else marks with a flag that this is changed
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteServiceItem(Guid id)//Delete ServiceItems by id
        {
            context.ServiceItems.Remove(new ServiceItem() { Id = id });
            context.SaveChanges();
        }
    }
}
