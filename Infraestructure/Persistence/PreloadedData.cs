using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class PreloadedData
    {
        public static void Preload(ModelBuilder modelBuilder)
        {
            // Precarga de datos para las tablas iniciales
            modelBuilder.Entity<FieldType>().HasData(
                new FieldType { FieldTypeID = 1, Description = "pasto" },
                new FieldType { FieldTypeID = 2, Description = "sintetico" },
                new FieldType { FieldTypeID = 3, Description = "Cemento" }
            );


            modelBuilder.Entity<Field>().HasData(
                new Field { FieldID = Guid.NewGuid(), Name = "Campo 1", Size = "5", FieldTypeID = 1 },
                new Field { FieldID = Guid.NewGuid(), Name = "Campo 2", Size = "7", FieldTypeID = 2 },
                new Field { FieldID = Guid.NewGuid(), Name = "Campo 3", Size = "11", FieldTypeID = 1 });


        }
    }
}
