using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ICommand
{
    public interface IFieldCommand
    {
        Task InsertField(Field field);
        Task UpdateField(Field field);
    }
}
