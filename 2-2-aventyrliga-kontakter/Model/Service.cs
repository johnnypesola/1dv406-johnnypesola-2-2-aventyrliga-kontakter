using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using _2_2_aventyrliga_kontakter.Model;

namespace _2_2_aventyrliga_kontakter.Model
{
    public class Service
    {

        /*
         * Medlemmarna i klassen Service använder presentationslogiklagret vid implementation av CRUD-funktionaliteten (Create Read Update Delete).
         */

    // Fields
        private ContactDAL _contactDAL;

    // Properties
        private ContactDAL ContactDAL {
            get {
                return _contactDAL ?? (_contactDAL = new ContactDAL());
            }
        }

    // Methods
        public void DeleteContact(Contact contact)
        {
            DeleteContact(contact.ContactId);
        }

        public void DeleteContact(int contactId)
        {
            if(contactId < 0)
            {
                throw new ApplicationException("Ogiltigt kontakt ID påträffades vid borttagning.");
            }

            ContactDAL.DeleteContact(contactId);

        }

        public Contact GetContact(int contactId)
        {
            if (contactId < 0)
            {
                throw new ApplicationException("Ogiltigt kontakt ID påträffades vid hämtning.");
            }

            return ContactDAL.GetContactById(contactId);
        }

        public IEnumerable<Contact> GetContacts()
        {
            return ContactDAL.GetContacts();
        }

        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int TotalRowCount)
        {
            // Calculate correct startpageIndex
            int startPageIndex = (startRowIndex / maximumRows) + 1;

            // Get contacts from DAL
            return ContactDAL.GetContactsPageWise(maximumRows, startPageIndex, out TotalRowCount);
        }

        public void SaveContact(Contact contact)
        {
            /*
             * Metoden Save används både då en ny kontaktuppgift ska läggas till i tabellen Contact och då en befintlig kontaktuppgift ska uppdateras.
             * Genom att undersöka värdet egenskapen ContactId har för Contact-objektet kan det bestämmas om det är fråga om en helt ny post, eller en uppdatering.
             * Har ContactId värdet 0 (standardvärdet för fält av typen int) är det en ny post. Är värdet större än 0 måste det vara en befintlig post som ska uppdateras.
             * 
             * Innan en post skapas eller uppdateras måste Contact-objektet valideras. Misslyckas valideringen ska ett undantag av typen ApplicationException kastas.
             * Genom egenskapen Data i klassen ApplicationException och metoden Add kan en referens till samlingen med valideringsresultat skickas med undantaget,
             * som tas omhand och behandlas i presentationslogiklagret.
             */

            // Preparare validation return data
            ICollection<ValidationResult> validationResults;

            // Try to validate given data
            if(contact.Validate(out validationResults))
            {
                // If a new contact should be created
                if(contact.ContactId == 0)
                {
                    ContactDAL.InsertContact(contact);
                }
                // Existing contact should be updated
                else
                {
                    ContactDAL.UpdateContact(contact);
                }
            }
            // Validation failed
            else
            {
                // Create exception
                ApplicationException exception = new ApplicationException("Kontaktobjektet innehöll felaktiga värden. Var god försök igen.");
                
                // Add validation data to exception.
                exception.Data.Add("ValidationResults", validationResults);

                throw exception;
            }
        }
    }
}