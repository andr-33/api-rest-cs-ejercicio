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

        public bool DeleteContactById(int Id) {
            // Verifica si HttpContext.Current no es nulo, lo que indica que la función está siendo ejecutada en el contexto de una solicitud HTTP en ASP.NET.
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                // Obtiene la lista actual de contactos desde la memoria caché utilizando HttpContext.Current.Cache[cacheKey] y la convierte en una lista utilizando .ToList().
                var currentContactsList = (ctx.Cache[cacheKey] as Contact[]).ToList();

                // Filtra la lista de contactos para eliminar el contacto con el ID especificado.
                var newContactsList = currentContactsList.Where(contact => contact.Id != Id).ToList();

                // Compara el número de elementos en la lista original con el número de elementos en la lista filtrada para determinar si se eliminó algún contacto.
                if (currentContactsList.Count != newContactsList.Count)
                {
                    // Si se eliminó algún contacto, actualiza la lista de contactos en la memoria caché con la lista filtrada.
                    ctx.Cache[cacheKey] = newContactsList.ToArray();
                    return true;
                }
            }

            // Devuelve false si no se encontró ningún contacto con el ID especificado o si no se pudo acceder a HttpContext.Current.
            return false;
        }

        public bool UpdateContactById(int Id, Contact contactToUpdate) {
            // Verifica si HttpContext.Current no es nulo, lo que indica que la función está siendo ejecutada en el contexto de una solicitud HTTP en ASP.NET.
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                // Obtiene la lista actual de contactos desde la memoria caché utilizando HttpContext.Current.Cache[cacheKey] y la convierte en una lista utilizando .ToList().
                var currentContactsList = (ctx.Cache[cacheKey] as Contact[]).ToList();

                // Utiliza LINQ para encontrar el contacto específico que coincide con el ID proporcionado.
                var contactById = currentContactsList.Where(contact => contact.Id == Id).SingleOrDefault();

                // Verifica si se encontró un contacto con el ID especificado.
                if (contactById != null)
                {
                    // Crea un nuevo objeto Contact con los datos actualizados proporcionados en contactToUpdate.
                    var updatedContact = new Contact
                    {
                        Id = Id,
                        Name = contactToUpdate.Name,
                        PhoneNumber = contactToUpdate.PhoneNumber,
                    };

                    // Elimina el contacto existente de la lista y agrega el contacto actualizado.
                    currentContactsList.Remove(contactById);
                    currentContactsList.Add(updatedContact);

                    // Actualiza la lista de contactos en la memoria caché con la lista actualizada.
                    ctx.Cache[cacheKey] = currentContactsList.ToArray();

                    return true; // Devuelve true indicando que el contacto fue actualizado correctamente.
                }
            }

            return false; // Devuelve false si no se encontró ningún contacto con el ID especificado o si no se pudo acceder a HttpContext.Current.
        }
    }
}