using MyCompany.Domain.Entities;
using System.Linq;
using System;
using MyCompany.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MyCompany.Domain.Repositories.EntityFramework
{
    public class EFTextFieldsRepository:ITextFieldsRepository
    {
        private readonly AppDbContext context;
        public EFTextFieldsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<TextField> GetTextFields()//Take all entries from TextFields
        {
            return context.TextFields;
        }

        public TextField GetTextFieldById(Guid id)//Take one entry from TextField
        {
            return context.TextFields.FirstOrDefault(x => x.Id == id);
        }

        public TextField GetTextFieldByCodeWord(string codeWord)//Take one entry from TextField by codeWord
        {
            return context.TextFields.FirstOrDefault(x => x.CodeWord == codeWord);
        }

        public void SaveTextField(TextField entity)//if id is default marks with a flag that this is a new object,else marks with a flag that this is changed
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteTextField(Guid id)
        {
            context.TextFields.Remove(new TextField() { Id = id });//Delete TextField by id 
            context.SaveChanges();
        }
    }
}
