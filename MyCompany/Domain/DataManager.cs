using MyCompany.Domain.Repositories.Interface;

namespace MyCompany.Domain
{
    public class DataManager//service class that manages centralized repositories
    {
        public ITextFieldsRepository TextFields { get; set; }
        public IServiceItemRepository ServiceItems { get; set; }

        public DataManager(ITextFieldsRepository textFieldsRepository, IServiceItemRepository serviceItemsRepository)
        {
            TextFields = textFieldsRepository;
            ServiceItems = serviceItemsRepository;
        }
    }
}
