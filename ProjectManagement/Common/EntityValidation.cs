using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ProjectManagement.Common
{
    public class EntityValidation
    {
        public static void EntityValidationError(DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
    }
}