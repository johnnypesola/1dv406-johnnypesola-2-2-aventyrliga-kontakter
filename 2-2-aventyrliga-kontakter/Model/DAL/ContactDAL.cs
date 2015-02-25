using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _2_2_aventyrliga_kontakter.Model
{
    public class ContactDAL : DALBase
    {

        /*
         * Klassen har de medlemmar som krävs för att implementera CRUD-funktionalitet.
         * Metoderna GetContactById, GetContacts och GetContactsPageWise används för att hämta en enskild kontaktuppgift,
         * alla kontaktuppgifter respektive kontakatuppgifter en sida i taget om t.ex. 20 kontakter.
         * 
         * InsertContact skapar en ny post i tabellen Contact. 
         * 
         * UpdateContact uppdaterar en befintlig kontaktuppgift,
         * 
         * och DeleteContact tar bort en.
         * 
         * Samtliga metoder exekverar de lagrade procedurerna enligt figur 4
         * 
         */

        public void DeleteContact(int contactId)
        {
            // Create connection object
            using (this.CreateConnection())
            {
                try
                {
                    SqlCommand cmd;

                    // Connect to database
                    cmd = this.Setup("Person.uspRemoveContact");

                    // Add parameter for Stored procedure
                    cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = contactId;

                    // Try to delete contact from database.
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    // Throw exception
                    throw new ApplicationException(DAL_ERROR_MSG);
                }
            }
        }

        public Contact GetContactById(int contactId)
        {
            // Create connection object
            using (this.CreateConnection())
            {
                try
                {
                    SqlCommand cmd;

                    // Connect to database
                    cmd = this.Setup("Person.uspGetContact", DALOptions.closedConnection);

                    // Add parameter for Stored procedure
                    cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = contactId;

                    // Open connection to database
                    connection.Open();

                    // Try to read response from stored procedure
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check if there is any return data to read
                        if (reader.Read())
                        {
                            // Get column indexes from known column names. Does not matter if columns change order.
                            int contactIdIndex = reader.GetOrdinal("ContactID");
                            int firstNameIndex = reader.GetOrdinal("FirstName");
                            int lastNameIndex = reader.GetOrdinal("LastName");
                            int emailAddressIndex = reader.GetOrdinal("EmailAddress");

                            // Create new Contact object from database values and return a reference
                            return new Contact
                            {
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                EmailAddress = reader.GetString(emailAddressIndex)
                            };
                        }
                    }

                    return null;
                }
                catch
                {
                    // Throw exception
                    throw new ApplicationException(DAL_ERROR_MSG);
                }
            } // Connection is closed here
        }

        public IEnumerable<Contact> GetContacts()
        {
            // Create connection object
            using (this.CreateConnection())
            {
                try
                {
                    List<Contact> contactsReturnList;
                    SqlCommand cmd;

                    // Create list object
                    contactsReturnList = new List<Contact>(50);

                    // Connect to database and execute given stored procedure
                    cmd = this.Setup("Person.uspGetContacts");

                    // Get all data from stored procedure
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Get column indexes from known column names. Does not matter if columns change order.
                        int contactIdIndex = reader.GetOrdinal("ContactID");
                        int firstNameIndex = reader.GetOrdinal("FirstName");
                        int lastNameIndex = reader.GetOrdinal("LastName");
                        int emailAddressIndex = reader.GetOrdinal("EmailAddress");

                        // Get all data rows
                        while (reader.Read())
                        {
                            // Create new Contact object from database values and add to list
                            contactsReturnList.Add(new Contact
                            {
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                EmailAddress = reader.GetString(emailAddressIndex)
                            });
                        }
                    }

                    // Remove unused list rows, free memory.
                    contactsReturnList.TrimExcess();

                    // Return list
                    return contactsReturnList;
                }
                catch
                {
                    throw new ApplicationException(DAL_ERROR_MSG);
                }
            }
        }

        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startPageIndex, out int totalRowCount)
        {
            // Create connection object
            using (this.CreateConnection())
            {
                try
                {
                    List<Contact> contactsReturnList;
                    SqlCommand cmd;

                    // Create list object
                    contactsReturnList = new List<Contact>(maximumRows);

                    // Connect to database and execute given stored procedure
                    cmd = this.Setup("Person.uspGetContactsPageWise", DALOptions.closedConnection);

                    // Add parameter for Stored procedure
                    cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = startPageIndex;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = maximumRows;
                    cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // Open DB connection
                    connection.Open();

                    // Get all data from stored procedure
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Get column indexes from known column names. Does not matter if columns change order.
                        int contactIdIndex = reader.GetOrdinal("ContactID");
                        int firstNameIndex = reader.GetOrdinal("FirstName");
                        int lastNameIndex = reader.GetOrdinal("LastName");
                        int emailAddressIndex = reader.GetOrdinal("EmailAddress");

                        // Get all data rows
                        while (reader.Read())
                        {
                            // Create new Contact object from database values and add to list
                            contactsReturnList.Add(new Contact
                            {
                                ContactId = reader.GetInt32(contactIdIndex),
                                FirstName = reader.GetString(firstNameIndex),
                                LastName = reader.GetString(lastNameIndex),
                                EmailAddress = reader.GetString(emailAddressIndex)
                            });
                        }
                    }

                    // Get total row count
                    totalRowCount = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);

                    //totalRowCount = 2000;

                    // Remove unused list rows, free memory.
                    contactsReturnList.TrimExcess();

                    // Return list
                    return contactsReturnList;
                }
                catch
                {
                    throw new ApplicationException(DAL_ERROR_MSG);
                }
            }
        }

        public void InsertContact(Contact contact)
        {
            // Create connection object
            using (this.CreateConnection())
            {
                try
                {
                    SqlCommand cmd;

                    // Connect to database
                    cmd = this.Setup("Person.uspAddContact", DALOptions.closedConnection);

                    // Add in parameters for Stored procedure
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = contact.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = contact.LastName;
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = contact.EmailAddress;

                    // Add out parameter for Stored procedure
                    cmd.Parameters.Add("@ContactID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // Open DB connection
                    connection.Open();

                    // Execute insert to database
                    cmd.ExecuteNonQuery();

                    // Place database insert id into contact object.
                    contact.ContactId = (int)cmd.Parameters["@ContactID"].Value;
                }
                catch
                {
                    // Throw exception
                    throw new ApplicationException(DAL_ERROR_MSG);
                }
            }
        }

        public void UpdateContact(Contact contact)
        {
            // Create connection object
            using (this.CreateConnection())
            {
                try
                {
                    SqlCommand cmd;

                    // Connect to database
                    cmd = this.Setup("Person.uspUpdateContact", DALOptions.closedConnection);

                    // Add in parameters for Stored procedure
                    cmd.Parameters.Add("@ContactID", SqlDbType.VarChar, 50).Value = contact.ContactId;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = contact.FirstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = contact.LastName;
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = contact.EmailAddress;

                    // Open DB connection
                    connection.Open();

                    // Execute insert to database
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    // Throw exception
                    throw new ApplicationException(DAL_ERROR_MSG);
                }
            }
        }
    }
}