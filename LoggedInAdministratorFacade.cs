using MainProject.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Business_Logic
{
   public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                _airlineDAO = new AirLineDAOMSSQL();
                _airlineDAO.Add(airline);
            }
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                _customerDAO = new CustomerDAOMSSQL();
                _customerDAO.Add(customer);
            }
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                _airlineDAO = new AirLineDAOMSSQL();
                _airlineDAO.Remove(airline);
            }
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                _customerDAO = new CustomerDAOMSSQL();
                _customerDAO.Remove(customer);
            }
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                _airlineDAO = new AirLineDAOMSSQL();
                _airlineDAO.Update(airline);
            }
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                _customerDAO = new CustomerDAOMSSQL();
                _customerDAO.Update(customer);
            }
        }
        public void CreateNewCountry(LoginToken<Administrator> token, Country country)
        {
            if(token !=null)
            {
                _countryDAO = new CountryDAOMSSQL();
                _countryDAO.Add(country);
            }
        }
        public long GetCountryId(string countryName)
        {
            long id = 0;
            using (SqlConnection conn = new SqlConnection(FlightCenterConfig.connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCountryIdByName", conn);
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@countryName", countryName));
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                while (reader.Read() == true)
                {
                    id = (long)reader["ID"];
                }
                cmd.Connection.Close();
            }

            return id;
        }
    }
}
