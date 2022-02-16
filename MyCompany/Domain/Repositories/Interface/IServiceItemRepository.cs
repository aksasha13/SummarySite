using MyCompany.Domain.Entities;
using System.Linq;
using System;

namespace MyCompany.Domain.Repositories.Interface
{
    public interface IServiceItemRepository
    {
        IQueryable<ServiceItem> GetServiceItems();
        ServiceItem GetServiceItemById(Guid id);
        void SaveServiceItem(ServiceItem entity);
        void DeleteServiceItem(Guid id);
    }
}
