using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAgenda.Models;

namespace ContactAgenda.Services
{
    public class ContactRepository
    {
        private const string cacheKey = "contactsStore";
        public ContactRepository() {
            var ctx = HttpContext.Current;

            if (ctx != null) {
                if (ctx.Cache[cacheKey] == null) {

                    var contacts = new Contact[] {
                        new Contact{
                            Id = 0,
                            Name = "Franklin",
                            PhoneNumber = "1234567890",
                        },
                        new Contact{
                            Id = 1,
                            Name = "Diego Weco",
                            PhoneNumber = "1234567899",
                        }
                    };

                    ctx.Cache[cacheKey] = contacts;
                }
            }
        }

        public Contact[] GetContacts() {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return ctx.Cache[cacheKey] as Contact[];
            }

            return new Contact[0];
        }

        public bool AddContact(Contact contact) {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                var contacts = (ctx.Cache[cacheKey] as Contact[]).ToList();
                contacts.Add(contact);
                ctx.Cache[cacheKey] = contacts.ToArray();
                return true;
            }

            return false;
        }
    }
}