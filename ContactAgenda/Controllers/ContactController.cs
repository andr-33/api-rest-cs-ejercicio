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

            if (IsAdded == true)
            {
                return Request.CreateResponse<string>(HttpStatusCode.OK, "Se agrego el contacto");
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Conflict);
            }
        }
    }
}
