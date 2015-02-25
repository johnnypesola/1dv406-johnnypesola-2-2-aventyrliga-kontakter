using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _2_2_aventyrliga_kontakter.Model;
using System.ComponentModel.DataAnnotations;

namespace _2_2_aventyrliga_kontakter
{
    public partial class Default : System.Web.UI.Page
    {
        private Service _service;

        // Service object property
        private Service Service
        {
            get 
            {
                // Create service object in case its null
                return _service ?? (_service = new Service());
            }
        }

        // Display success message function
        private void displaySuccessMessage(string message)
        {
            InfoPanel.Visible = true;
            InfoPanel.CssClass = "success-message";
            InfoPanelLiteral.Text = message;
        }

        // Display error message function
        private void displayErrorMessage(Exception exception)
        {
            // If there are any validationresults contained within the exception
            if(exception.Data["ValidationResults"] != null)
            {
                var validationResults = exception.Data["ValidationResults"] as IEnumerable<ValidationResult>;
                if (validationResults != null && validationResults.Any())
                {
                    foreach (var validationResult in validationResults)
                    {
                        foreach (var memberName in validationResult.MemberNames)
                        {
                            ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                // Display error from normal exception
                ModelState.AddModelError(String.Empty,
                    (exception.Message != null ? exception.Message : "Ett oväntat fel inträffade då uppgifter behandlades."));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Contact created message
            if (Session["contact_created"] != null)
            {
                displaySuccessMessage("Kontakten lades till med ett lyckat resultat.");
                Session.Remove("contact_created");
            }
            // Contact updated message
            else if (Session["contact_updated"] != null)
            {
                displaySuccessMessage("Kontakten uppdaterades med ett lyckat resultat.");
                Session.Remove("contact_updated");
            }
            // Contact deleted message
            else if(Session["contact_deleted"] != null)
            {
                displaySuccessMessage("Kontakten raderades med ett lyckat resultat.");
                Session.Remove("contact_deleted");
            }


            if(Request.QueryString["showcontact"] != null)
            {

            }

        }

        // Get contacts page wise
        public IEnumerable<Contact> ContactListView_GetData(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            try
            {
                // Get contacts page wise
                return Service.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
            }
            catch (Exception exception)
            {
                // Display error message
                displayErrorMessage(exception);

                // Defaults in case of error
                totalRowCount = 0;
                return null;
            }
        }

        // Create contact
        public void ContactListView_InsertItem(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert new contact
                    Service.SaveContact(contact);

                    // Mark contact created
                    Session["contact_created"] = true;
                    
                    // Reload page
                    Response.Redirect(Request.RawUrl);
                }
                catch (Exception exception)
                {
                    // Display Error message
                    displayErrorMessage(exception);
                }
            }
        }

        // Update contact
        public void ContactListView_UpdateItem(int contactId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Try to find contact
                    Contact contact = Service.GetContact(contactId);
                    if (contact == null)
                    {
                        // Contact not found
                        displayErrorMessage(new ApplicationException(String.Format("Kontakt med id {0} hittas ej.", contactId)));

                        return;
                    }

                    // Update contact object with new info.
                    if (TryUpdateModel(contact))
                    {
                        // Save contact object
                        Service.SaveContact(contact);

                        // Mark contact updated
                        Session["contact_updated"] = true;

                        // Reload page
                        Response.Redirect(Request.RawUrl);
                    }
                }
                catch (Exception exception)
                {
                    // Display error message
                    displayErrorMessage(exception);
                }
            }
        }

        // Delete contact
        public void ContactListView_DeleteItem(int contactId)
        {
            try
            {
                // Delete contact
                Service.DeleteContact(contactId);

                // Mark contact as updated
                Session["contact_deleted"] = true;

                // Reload page
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exception)
            {
                // Display error message
                displayErrorMessage(exception);
            }
        }
    }
}