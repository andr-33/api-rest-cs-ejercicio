using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactAgenda.Models;
using ContactAgenda.Services;

namespace ContactAgenda.Controllers
{
    public class ContactController : ApiController
    {
        private ContactRepository contactRepository;

        public ContactController() {
            contactRepository = new ContactRepository();
        }

        [HttpGet]
        [Route("contact/getall")]
        public Contact[] Get() {
            return contactRepository.GetContacts();
        }

        [HttpPost]
        [Route("contact/add")]
        public HttpResponseMessage Post(Contact contact) {
            bool IsAdded = contactRepository.AddContact(contact);

            //Si IsAdded es igual a true
            if (IsAdded)
            {
                return Request.CreateResponse<string>(HttpStatusCode.OK, "Se agrego el contacto");
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Conflict);
            }
        }

        [HttpDelete]
        [Route("contact/delete/{Id}")]
        public HttpResponseMessage Delete(int Id) {
            // Llama al método DeleteContactById del repositorio de contactos para eliminar el contacto con el ID especificado.
            bool IsDeleted = contactRepository.DeleteContactById(Id);

            // Verifica si el contacto fue eliminado correctamente.
            if (IsDeleted)
            {
                // Si el contacto fue eliminado correctamente, devuelve una respuesta HTTP 200 OK con un mensaje indicando que el contacto fue eliminado.
                return Request.CreateResponse<string>(HttpStatusCode.OK, "Contacto eliminado");
            }
            else
            {
                // Si el contacto no pudo ser eliminado (porque no se encontró en la base de datos, por ejemplo), devuelve una respuesta HTTP 404 Not Found con un mensaje indicando que el contacto no se pudo eliminar.
                return Request.CreateResponse<string>(HttpStatusCode.NotFound, "No se pudo eliminar el contacto");
            }
        }

        [HttpPut]
        [Route("contact/update/{Id}")]
        public HttpResponseMessage Put(int Id, Contact contact)
        {
            // Llama al método UpdateContactById del repositorio de contactos para actualizar el contacto con el ID especificado.
            bool IsUpdated = contactRepository.UpdateContactById(Id, contact);

            // Verifica si el contacto fue actualizado correctamente.
            if (IsUpdated)
            {
                // Si el contacto fue actualizado correctamente, devuelve una respuesta HTTP 200 OK con un mensaje indicando que el contacto fue actualizado.
                return Request.CreateResponse<string>(HttpStatusCode.OK, "Contacto actualizado");
            }
            else
            {
                // Si el contacto no pudo ser actualizado (porque no se encontró en la base de datos, por ejemplo), devuelve una respuesta HTTP 404 Not Found con un mensaje indicando que el contacto no se pudo actualizar.
                return Request.CreateResponse<string>(HttpStatusCode.NotFound, "No se pudo actualizar el contacto");
            }
        }
    }
}
