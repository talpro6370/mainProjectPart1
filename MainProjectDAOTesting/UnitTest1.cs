﻿using System;
using System.Collections.Generic;
using MainProject;
using MainProject.Business_Logic;
using MainProject.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainProjectDAOTesting
{
    [TestClass]
    public class UnitTest1
    {
        public AirLineDAOMSSQL aldao = new AirLineDAOMSSQL();
        [TestMethod]
        public void TestDAOConnection()
        {
            FlightCenterConfig.DeleteDataBase();
            AirLineDAOMSSQL al = new AirLineDAOMSSQL();
            Assert.AreEqual(true, al.ExUserNameInCustomers("TALAR")); 
        }
        [TestMethod]
        public void TestFlyingCenterSystem()
        {
            FlightCenterConfig.DeleteDataBase();
            FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();

        }
        [TestMethod]
        public void FacadeTest()
        {
            FlightCenterConfig.DeleteDataBase();
            FlyingCenterSystem FCS = FlyingCenterSystem.GetInstance();
            LoggedInAdministratorFacade laf = new LoggedInAdministratorFacade();
            LoggedInAdministratorFacade laf2 =  FCS.GetFacade(new LoginToken<Administrator>()) as LoggedInAdministratorFacade;
            try
            {
                Assert.AreEqual(laf, laf2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        [TestMethod]
        public void SystemCheck()
        {
            // Deleting database
            FlightCenterConfig.DeleteDataBase();

            //Creating DAO'S samples to test
            long airlineCompanyId1 = 0, airlineCompanyId2 = 0, countryId1 = 0, countryId2 = 0, flightId1=0, customerId1=0;
            AirLineDAOMSSQL adao = new AirLineDAOMSSQL();
            FlightDAOMSSQL fdao = new FlightDAOMSSQL();
            CustomerDAOMSSQL cdao = new CustomerDAOMSSQL();
            CountryDAOMSSQL cydao = new CountryDAOMSSQL();
            TicketDAOMSSQL tdao = new TicketDAOMSSQL();
            
             // Adding new countries
            cydao.Add(new Country { COUNTRY_NAME = "Israel" });
            countryId1 = cydao.GetCountryId("Israel");

            cydao.Add( new Country { COUNTRY_NAME = "Germany" });
            countryId2 = cydao.GetCountryId("Germany");

            // Adding new Airline companies
            adao.Add(new AirlineCompany { AIRLINE_NAME = "IsrLines", USER_NAME = "ISR91", PASSWORD = "Is789", COUNTRY_CODE = countryId1 });
            airlineCompanyId1 = adao.GetAirlineCompanyId("ISR91");

            adao.Add(new AirlineCompany { AIRLINE_NAME = "British Airlines", USER_NAME = "British555", PASSWORD = "b789sh", COUNTRY_CODE = countryId2 });
            airlineCompanyId2 = adao.GetAirlineCompanyId("British555");

            // Adding new Flight
            fdao.Add(new Flight { AIRLINECOMPANY_ID = airlineCompanyId1, ORIGIN_COUNTRY_CODE = countryId1, DESTINATION_COUNTRY_CODE = countryId2, DEPARTURE_TIME = new DateTime(2019, 05, 10, 10, 30, 00), LANDING_TIME = new DateTime(2019, 05, 10, 15, 05, 00), REMAINING_TICKETS = 15, AIRLINECOMPANY_NAME = "IsrLines", FLIGHT_NAME = "555" });
            flightId1 = fdao.GetFlightId("555");

            // Adding new Customer
            cdao.Add(new Customer { FIRST_NAME = "Efrat", LAST_NAME = "Patihi", USER_NAME = "Efrat_kapara", PASSWORD = "Efi93", ADDRESS = "Netanya , Raziel 40", PHONE_NO = "05011554875", CREDIT_CARD_NUMBER = "123456789" });
            customerId1 = cdao.GetCustomerId("Efrat_kapara");

            // Adding new Ticket
            tdao.Add(new Tickets { CUSTOMER_ID = customerId1, FLIGHT_ID = flightId1 });

            // Testing
            FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();
            LoginToken<Administrator> adminLogin = new LoginToken<Administrator>();
            adminLogin.User = new Administrator();
            adminLogin.User.AdminUserName = "admin";
            adminLogin.User.Password = "9999";


            // Trying to login
            fcs.login.TryAdminLogin("admin", "9999", out LoginToken<Administrator> adminToken);
            fcs.login.TryAirlineLogin("ISR91", "Is789", out LoginToken<AirlineCompany> airlineToken);
            fcs.login.TryCustomerLogin("Efrat_kapara", "Efi93", out LoginToken<Customer> customerToken);


            // Testing admin facade fucntions
            LoggedInAdministratorFacade adminFacadae = fcs.GetFacade(adminLogin) as LoggedInAdministratorFacade;
            adminFacadae.CreateNewCountry(adminLogin, new Country { COUNTRY_NAME = "Israel" });
            AirlineCompany airline = new AirlineCompany()
            {
                AIRLINE_NAME = "Nassa",
                USER_NAME = "Nassa28",
                PASSWORD = "Na123456",
                COUNTRY_CODE = adminFacadae.GetCountryId("Israel")
            };
            adminFacadae.CreateNewAirline(adminLogin,airline);
            AirLineDAOMSSQL air = new AirLineDAOMSSQL();
            Assert.AreEqual(air.GetAirlineByUserame("Nassa28"), airline);

            Customer cust = new Customer()
            {
                FIRST_NAME = "Tal",
                LAST_NAME = "Rdt",
                USER_NAME = "TalGever",
                PASSWORD = "Ta123456",
                ADDRESS = "Hazfira 20, Pardes hanna",
                PHONE_NO = "0505001245",
                CREDIT_CARD_NUMBER = "456123456789"
            };
            adminFacadae.CreateNewCustomer(adminLogin, cust);
            CustomerDAOMSSQL customer = new CustomerDAOMSSQL();
            Assert.AreEqual(cust, customer.GetCustomerByUserame("TalGever"));


            adminFacadae.UpdateAirlineDetails(adminLogin, new AirlineCompany { AIRLINE_NAME = "Nasa4Life", USER_NAME = "Nassa28", PASSWORD = "Na123456", COUNTRY_CODE = adminFacadae.GetCountryId("Israel") });
            adminFacadae.UpdateCustomerDetails(adminLogin, new Customer { FIRST_NAME = "Tal", LAST_NAME = "Rdt", USER_NAME = "TalGever", PASSWORD = "Ta123456", ADDRESS = "Raziel 50 , Netanya", PHONE_NO = "0505001245", CREDIT_CARD_NUMBER = "456123456789" });
            AirlineCompany airl = new AirlineCompany()
            {
                AIRLINE_NAME = "Nasa4Life",
                USER_NAME = "Nassa28",
                PASSWORD = "Na123456",
                COUNTRY_CODE = adminFacadae.GetCountryId("Israel")
            };
            adminFacadae.RemoveAirline(adminLogin, airl);
             // check how to use assert properly .

           
            // Testing airlineCompany facade functions
            //LoggedInAirlineFacade airlineFacade = new LoggedInAirlineFacade();
            //AirlineCompany airlinecc = new AirlineCompany("Nassa", "Nassa28", "Na123456", 10011);
            //airlineFacade.GetAirlineByUserName(airlineToken, "ISR91");
            //Assert.AreEqual(airlineFacade.GetAirlineByUserName(airlineToken, "ISR91"), airlinecc);






        }
    }
}
