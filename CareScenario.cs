using System;
using System.IO;
using System.Collections.Generic;

public class CareScenario
{
    //fields

    public int DailyCostOfChildcare { get; private set; }
    public int DaysInCare { get; private set; }
    public Family MyFamily { get; private set; }
    public ChildCareCentre PreferredCentre { get; private set; }


    //constructors 
    public CareScenario(Family baseFamily)
    {
        MyFamily = baseFamily;
    }


    //methods

    public void RegisterCareDetails(DataCentre searchCentre)
    {
        System.Console.WriteLine("\nWould you like to choose a preferred childcare centre?  This will automatically calculate costs per day.  Please enter Y or N.");
        string choice;

        //get cost details

        do
        {
            choice = Console.ReadLine();

            if(choice == "Y" || choice == "y")
            {
                searchCentre.Search();
                PreferredCentre = searchCentre.SelectPreferredCentre();
                System.Console.WriteLine($"\nThanks.  The daily cost of {PreferredCentre.CostPerDay} will be used in the calculations.");
                DailyCostOfChildcare = PreferredCentre.CostPerDay;
            } 
            else if (choice == "N" || choice == "n")
            {
                do
                {
                    System.Console.WriteLine($"\nPlease enter the daily cost of childcare you will pay for.");
                    try
                    {
                        DailyCostOfChildcare = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (System.Exception)
                    {
                        System.Console.WriteLine("Invalid entry, please try again.");
                        DailyCostOfChildcare = 0;
                        
                    }
                } while (DailyCostOfChildcare == 0);
            }
            else 
            {
                System.Console.WriteLine("Invalid response, please type in Y or N:");
                choice = "a";
            }
        } while (choice == "a");

        //get number of days

        do
        {
            System.Console.WriteLine($"\nHow many days a week of care will you pay for?");
            try 
            { 
                DaysInCare = Convert.ToInt32(Console.ReadLine());
                if(DaysInCare < 0 || DaysInCare > 5)
                {
                    System.Console.WriteLine("Please enter a number between 0 and 5.");
                    DaysInCare = -1;
                }
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Invalid response, please enter a number between 0 and 5.");
                DaysInCare = -1;
            }
        } while (DaysInCare == -1);
    }

    public void RegisterIncomeDetails()
    {
        MyFamily.ConfirmIncome();
    }

    public void PrintCareDetails()
    {
        MyFamily.PrintCareSummary();

        if(PreferredCentre != null)
        {
            System.Console.WriteLine($"You have chosen to model {DaysInCare} days per week of care at the {PreferredCentre.ServiceName}, at a cost of ${DailyCostOfChildcare} per day.");
        }
        else
        {
            System.Console.WriteLine($"You have chosen to model {DaysInCare} days per week of care, at a cost of ${DailyCostOfChildcare} per day.\n");
        }
    }

}